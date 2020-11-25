using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.Extensions.Logging;
using store_api.Objects;
using store_api.Objects.Helpers;
using static store_api.Objects.DbKinds;

namespace store_api.CloudDatastore.DAL
{
    public class StoreRepository : Repository, IStoreRepository
    {
       
        public StoreRepository(
            ILogger<StoreRepository> logger) : base(logger)
        {
           
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var result = await Get(DbCollections.Products);
            return result.Select(entity => new Product
            {
                DataStoreId = entity.Key.ToId(),
                Id = (int)entity.Properties["Id"].IntegerValue,
                CategoryId = (int)entity.Properties["CategoryId"].IntegerValue,
                Name = entity.Properties["Name"].StringValue,
                Description = entity.Properties["Description"].StringValue,
                ImageUrl = entity.Properties["ImageUrl"].StringValue,
                PricePerUnit = (decimal)entity.Properties["PricePerUnit"].DoubleValue,
                DateCreated = entity.Properties["DateCreated"].TimestampValue.ToDateTime()
            });
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            await DeleteProduct(product.DataStoreId);
            return  await InsertProduct(product);
        }

        public async Task<bool> InsertProduct(Product product)
        {
            return await Insert(product, DbCollections.Products);
        }

        public async Task DeleteProduct(long id)
        {
             await Delete(id.ToKey(DbCollections.Products));
        }

        public async Task<IEnumerable<Categories>> GetCategories()
        {
            var result = await Get(DbCollections.Categories);
            return result.Select(entity => new Categories
            {
                DataStoreId = entity.Key.ToId(),
                Id = (int) entity.Properties["Id"].IntegerValue,
                Category = entity.Properties["Category"]
                    .StringValue
            });
        }

        public async Task<bool> AddCategories(Categories categoryToAdd)
        {
            return await Insert(categoryToAdd, DbCollections.Categories);
        }
    }
}
       
