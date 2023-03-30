using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest.TransientFaultHandling;
using Ordering.API.Dtos;
using Ordering.API.Queries;
using Utilities;

namespace Ordering.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    public class OrderingController : Controller
    {
        private readonly IOrderQueries _orderQueries;
        private readonly TelemetryClient _telemetryClient;
        

        public OrderingController(IOrderQueries orderQueries, TelemetryClient telemetryClient)
        {
            _orderQueries = orderQueries;
            _telemetryClient = telemetryClient;
        }

        /// <summary>
        ///     Get Detail for Specified Order - Ver 1.0
        /// </summary>
        /// <param name="orderId">Identifier for an order</param>
        /// <returns>Details for specified Order</returns>
        [ProducesResponseType(typeof(OrderDto), 200)]
        [HttpGet("v{version:apiVersion}/Order/{orderId}", Name = "GetOrdersRoute")]
        //[HttpGet("v/Order/{orderId}", Title = "GetOrdersRoute")]
        public async Task<IActionResult> GetOrder(string orderId, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForNullOrEmpty(orderId, "orderid");
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var order = await _orderQueries.GetOrder(orderId, correlationToken);

            if (order == null)
                return BadRequest("Order does not exist");

            return new ObjectResult(RestMapper.MapToOrderDto(order));
        }

        /// <summary>
        ///     Gets All Orders
        /// </summary>
        /// <param name="orderId">Identifier for an order</param>
        /// <returns>Details for specified Order</returns>
        [ProducesResponseType(typeof(OrderDto), 200)]
        [HttpGet("Orders", Name = "GetAllOrdersRoute")]
        //[HttpGet("v{version:apiVersion}/Orders", Title = "GetAllOrdersRoute")]
        public async Task<IActionResult> GetOrders([FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            _telemetryClient.TrackEvent(
                $"Publishing EmptyBasketEvent from CheckOutEventHandler in Ordering.API for Request {correlationToken} ");



            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var orders = await _orderQueries.GetOrders(correlationToken);

            if (orders == null || orders.Count < 1)
                return BadRequest("Orders do not exist");

            return new ObjectResult(RestMapper.MapToOrdersDto(orders));
        }

        /// <summary>
        ///     Simulate 500 Error
        /// </summary>
        /// <param name="orderId">Identifer for an order</param>
        /// <param name="correlationToken">Tracks request - Can be any value</param>
        /// <returns>Details for specified Order</returns>
        
        [ProducesResponseType(typeof(OrderDto), 200)]
        [HttpGet("Orders/SimulateError", Name = "SimulateErrorRoute")]
        public async Task<IActionResult> SimulateError([FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}