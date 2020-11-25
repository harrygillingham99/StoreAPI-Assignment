using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL;
using store_api.Objects;

namespace store_api.Controllers
{
    [Route("categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IStoreRepository _storeRepository;

        public CategoriesController(ILogger<ProductsController> logger, IStoreRepository storeRepository)
        {
            _logger = logger;
            _storeRepository = storeRepository;
        }

        [HttpGet("")]
        public async Task<List<Categories>> GetCategories()
        {
            try
            {
                return (await _storeRepository.GetCategories()).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting categories");
                throw;
            }
        }

        [HttpPost("")]
        public async Task<bool> AddCategory([FromBody] Categories categoryToAdd)
        {
            try
            {
                return (await _storeRepository.AddCategories(categoryToAdd));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }
    }
}
