using System.Collections.Generic;
using System.Threading.Tasks;
using store_api.Objects;

namespace store_api.CloudDatastore.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> UpdateProduct(Product product);
        Task<bool> InsertProduct(Product product);
        Task DeleteProduct(long key);
    }
}