using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using store_api.Objects;

namespace store_api.CloudDatastore.DAL
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> UpdateProduct(Product product);
        Task<bool> InsertProduct(Product product);
        Task DeleteProduct(long key);
        Task<IEnumerable<Categories>> GetCategories();
        Task<bool> AddCategories(Categories categoryToAdd);
    }
}