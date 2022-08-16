using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context,ILogger<OrderContextSeed> logger){
            if (!context.Orders.Any())
            {
                context.AddRange(GetPreConfigurationOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("seed database associated with context {DbContextName}",typeof(OrderContext).Name);
            }

        }

        private static IEnumerable<Order> GetPreConfigurationOrders()
        {
            return new List<Order>()
            {
                new Order(){UserName="khaled",FirstName="Tared",LastName="ahmed",EmailAddress="",AddressLine="",Country="",TotalPrice=350 }
            };
        }
    }
}
