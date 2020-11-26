﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using store_api.Objects.Helpers;
using static store_api.Objects.DbKinds;

namespace store_api.CloudDatastore.DAL.Repositories
{
    public class ProductRepository : Repository, IProductRepository
    {
        private const DbCollections Kind = DbCollections.Products;

        public ProductRepository(
            ILogger<ProductRepository> logger) : base(logger, Kind)
        {
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var result = await Get();
            return result.Select(entity => new Product
            {
                DataStoreId = entity.Key.ToId(),
                Id = (int) entity.Properties["Id"].IntegerValue,
                CategoryId = (int) entity.Properties["CategoryId"].IntegerValue,
                Name = entity.Properties["Name"].StringValue,
                Description = entity.Properties["Description"].StringValue,
                ImageUrl = entity.Properties["ImageUrl"].StringValue,
                PricePerUnit = (decimal) entity.Properties["PricePerUnit"].DoubleValue,
                DateCreated = entity.Properties["DateCreated"].TimestampValue.ToDateTime()
            });
        }

        public Task<bool> UpdateProduct(Product product)
        {
            return Update(product, product.DataStoreId.ToKey(Kind));
        }

        public Task<bool> InsertProduct(Product product)
        {
            return Insert(product);
        }

        public Task DeleteProduct(long id)
        {
            return Delete(id.ToKey(Kind));
        }
    }
}