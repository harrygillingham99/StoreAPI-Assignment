using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using Swashbuckle.AspNetCore.Annotations;

namespace store_api.Controllers
{
    [Route("session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ISessionRepository _sessionRepository;

        public SessionController(ILogger<ProductsController> logger, ISessionRepository sessionRepository)
        {
            _logger = logger;
            _sessionRepository = sessionRepository;
        }

        [HttpPost("current-basket")]
        [SwaggerResponse(200, "Success", typeof(Basket))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<Basket>> GetCurrentBasket([FromBody]AuthedRequest token)
        {
            try
            {
                var requestUid = await token.Verify();

                if (requestUid == null)
                    return Unauthorized();

                return Ok((await _sessionRepository.GetCurrentBasket("thisIsATestNewUser")));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting categories");
                throw;
            }
        }

        [HttpPut("update-basket")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<bool>> UpdateBasket([FromBody] AuthedRequestWrapper<Basket> basketToUpdate)
        {
            try
            {
                var requestUid = await basketToUpdate.Verify();

                if (requestUid == null)
                    return Unauthorized();

                basketToUpdate.Request.UserUid = requestUid;

                return Ok(await _sessionRepository.UpdateBasket(basketToUpdate.Request));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }

        [HttpPost("place-order")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<bool>> OrderItems([FromBody] AuthedRequestWrapper<Basket> basketToOrder)
        {
            try
            {
                var requestUid = await basketToOrder.Verify();

                if (requestUid == null)
                    return Unauthorized();

                basketToOrder.Request.UserUid = requestUid;

                return Ok(await _sessionRepository.OrderItems(basketToOrder.Request));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }

        [HttpPost("historic-orders")]
        [SwaggerResponse(200, "Success", typeof(List<Basket>))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<List<Basket>>> GetHistoricOrders([FromBody]AuthedRequest token)
        {
            try
            {
                var requestUid = await token.Verify();

                if (requestUid == null)
                    return Unauthorized();

                return Ok(await _sessionRepository.GetHistoricOrders(requestUid));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting historic orders");
                throw;
            }
        }

    }
}
