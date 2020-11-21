using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_api.Objects;

namespace store_api.Controllers
{
    [ApiController]
    [Route("session")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public SessionController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("clear-basket")]
        public async Task<IActionResult> ClearBasket(VoidRequest requester)
        {
            return Ok();
        }
        [HttpPost("add-to-basket")]
        public async Task<IActionResult> AddToBasket(RequestWrapper<ItemSelection> request)
        {
            return Ok();
        }
    }

    
}