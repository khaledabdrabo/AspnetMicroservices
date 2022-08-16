using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Odering.API.Extensions
{
    public static class HostExtensions
    {
        public static IServiceCollection MigrateDatabase<TContext>(this IServiceCollection serviceCollection, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            var provider=serviceCollection.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context { DbContextName}", typeof(TContext).Name);

                    var retry = Policy
                        .Handle<SqlException>()
                        .WaitAndRetry(retryCount: 5,
                                      sleepDurationProvider: re => TimeSpan.FromSeconds(Math.Pow(2, re)),
                                      onRetry: (exception, retryCount, context) =>
                                      {
                                          logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                                      });

                    retry.Execute(() => InvokeSeeder(seeder, context, services));
                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {

                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                }
            }
            return serviceCollection;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
