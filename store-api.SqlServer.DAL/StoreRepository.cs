using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using store_api.Objects;

namespace store_api.SqlServer.DAL
{
    public class StoreRepository : Repository, IStoreRepository
    {

        public StoreRepository(IOptions<ConnectionStrings> connectionStrings, 
            ILogger<StoreRepository> logger) : base(connectionStrings.Value.SqlServer, logger)
        {
          
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await Do(query => query.QueryAsync<Product>(Sql.GetProducts));
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            return await ExpireProduct(product.Id) && await InsertProduct(product);
        }

        public async Task<bool> InsertProduct(Product product)
        {
            return await Do(query => query.ExecuteAsync(Sql.InsertProduct, new
            {
                product.Name,
                product.Description,
                product.ImageUrl,
                CategoryTypeId = product.CategoryId,
                product.PricePerUnit
            })) > 0;
        }

        public async Task<bool> ExpireProduct(int id)
        {
            return await Do(query => query.ExecuteAsync(Sql.ExpireProduct, new {Id = id})) >0;
        }
    }

    public interface IStoreRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> UpdateProduct(Product product);
        Task<bool> InsertProduct(Product product);
        Task<bool> ExpireProduct(int id);
    }
}
       
