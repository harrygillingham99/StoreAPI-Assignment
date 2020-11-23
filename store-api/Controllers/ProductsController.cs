using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_api.Objects;
using store_api.SqlServer.DAL;

namespace store_api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IStoreRepository _storeRepository;

        public ProductsController(ILogger<ProductsController> logger, IStoreRepository storeRepository)
        {
            _logger = logger;
            _storeRepository = storeRepository;
        }

        [HttpGet("get")]
        public async Task<List<Product>> GetProducts()
        {
            try
            {
                return (await _storeRepository.GetProducts()).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting products");
                throw;
            }
            
        }

        [HttpPut("update")]
        public async Task<bool> UpdateProduct([FromBody] Product product)
        {
            try
            {
                return await _storeRepository.UpdateProduct(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception updating products");
                throw;
            }
        }

        [HttpPost("insert")]
        public async Task<bool> InsertProduct([FromBody] Product product)
        {
            try
            {
                return await _storeRepository.InsertProduct(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception inserting products");
                throw;
            }
        }

        [HttpDelete("expire/{id}")]
        public async Task<bool> ExpireProduct([FromRoute] int id)
        {
            try
            {
                return await _storeRepository.ExpireProduct(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception expiring products");
                throw;
            }
        }
    }
}
