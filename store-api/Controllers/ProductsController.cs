using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_api.Objects;

namespace store_api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("get")]
        public async Task<List<StoreItem>> GetProducts()
        {
            return await Task.FromResult(new List<StoreItem>
            {
                new StoreItem
                {
                    Name = "Cat Food",
                    ProductDescription = "It's food for cats.",
                    Price = 14.99m
                },
                new StoreItem
                {
                    Name = "Dog Food",
                    ProductDescription = "It's food for dogs",
                    Price = 12.99m
                }
            });
        }
    }
}
