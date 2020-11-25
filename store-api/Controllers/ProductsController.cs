using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL;
using store_api.Objects;

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

        [HttpGet("")]
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

        [HttpPut("")]
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

        [HttpPost("")]
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

        [HttpDelete("")]
        public async Task<ActionResult> DeleteProduct([FromBody] long key)
        {
            try
            {
                await _storeRepository.DeleteProduct(key);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception expiring products");
                throw;
            }
        }
    }
}
