using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public interface ICatalogDbContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
