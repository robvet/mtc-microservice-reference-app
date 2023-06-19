using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Basket.Service.Contracts;
using Basket.Service.Domain.Entities;
using Basket.Service.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedUtilities.Utilties;

namespace Basket.Service.Controllers
{
    /// <summary>
    ///     Microservice that manages the Shopping BasketEntity experience
    /// </summary>
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketBusinessServices _basketBusinessServices;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketBusinessServices basketBusinessServices,
                                ILogger<BasketController> logger)
        {
            _basketBusinessServices = basketBusinessServices;
            _logger = logger;
        }

        /// <summary>
        ///     Gets all shopping products.        /// </summary>
        /// <returns>List of line items that make up a shopping basket</returns>
        [ProducesResponseType(typeof(List<GenericSummaryDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("Baskets", Name = "GetAllBasketsRoute")]
        //public async Task<IActionResult> GetAllBaskets()
        public async Task<IActionResult> GetAllBaskets([FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var baskets = await _basketBusinessServices.GetAllBaskets(correlationToken);

            if (baskets == null || baskets.Count < 1)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
                 
            var response = Mapper.MapToBasketDto(baskets);

            if (response == null || response.Count < 1)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            
            return new ObjectResult(Mapper.MapToBasketDto(baskets));
        }

        /// <summary>
        ///     Gets all Products from Catalog read model
        /// </summary>
        /// <returns>List of line items that make up a shopping basket</returns>
        [ProducesResponseType(typeof(List<GenericSummaryDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("Products", Name = "GetAllProductsRoute")]
        //public async Task<IActionResult> GetAllBaskets()
        public async Task<IActionResult> GetAllProducts([FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var products = await _basketBusinessServices.GetAllProducts(correlationToken);

            if (products == null)
                return BadRequest("Products do not exist");
            else if (products.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToProductDto(products));
        }

        /// <summary>
        ///     Get all line items for a specified shopping basket
        /// </summary>
        /// <param name="basketId">Identifier for user shopping basket</param>
        /// <returns>List of line items that make up a shopping basket</returns>
        [ProducesResponseType(typeof(List<BasketItemDto>), 200)]
   [ProducesResponseType(400)]
        [HttpGet("Basket/{basketId}", Name = "GetbasketRoute")]
        public async Task<IActionResult> GetBasket(Guid basketId, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForValidGuid(basketId, "BasketId");
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var basket = await _basketBusinessServices.GetBasketById(basketId, correlationToken);

            return basket == null
                ? NotFound($"Basket {basketId} does not exist")
                    : new ObjectResult(Mapper.MapToBasketDto(basket));
        }

        /// <summary>
        ///     Get all line items for a specified shopping basket
        /// </summary>
        /// <param name="basketId">Identifier for user shopping basket</param>
        /// <returns>List of line items that make up a shopping basket</returns>
        [ProducesResponseType(typeof(BasketSummaryDto), 200)]
        [ProducesResponseType(400)]
        [HttpGet("BasketSummary/{basketId}", Name = "GetBasketSummaryRoute")]
        public async Task<IActionResult> GetBasketSummary(Guid basketId, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForValidGuid(basketId, "BasketId");
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var basket = await _basketBusinessServices.GetBasketById(basketId, correlationToken);

            if (basket == null)
                return BadRequest($"BasketEntity {basketId} does not exist for Request {correlationToken}");

            return new ObjectResult(new BasketSummaryDto
            {
                BasketId = basket.BasketId,
                ItemCount = basket.Items.Count
            });
        }

        /// <summary>
        ///     Adds a new line item to the user's shopping basket
        /// </summary>
        /// <param name="product">ProductEntity Information</param>
        /// <param name="basketId">Identifier for user shopping basket</param>
        /// <param name="productId">ProductEntity Identifier</param>
        /// <param name="productGuidId">ProductEntity Guid Identifier</param>
        /// <returns>The newly-created line item</returns>
        [ProducesResponseType(typeof(BasketItemDto), 200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<IActionResult> AddBasketItem(Guid basketId, Guid productId, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForValidGuid(productId, "ProductId");
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var basket = await _basketBusinessServices.AddItemToBasket(basketId, productId, correlationToken);

            if (basket == null)
                return BadRequest($"BasketEntity: Cloud not add item to basket {basketId} for Request {correlationToken}");

            return new ObjectResult(new BasketSummaryDto
            {
                BasketId = basket.BasketId,
                CorrelationId = correlationToken,
                ItemCount = basket.ItemCount
            });
        }

        /// <summary>
        ///     Removes line item from the shopping basket
        /// </summary>
        /// <param name="basketId">Identifier for user shopping basket</param>
        /// <param name="productId">ProductEntity Identifier</param>
        /// <returns>Summary of shopping basket state</returns>
        [ProducesResponseType(typeof(BasketItemRemove), 200)]
        [ProducesResponseType(400)]
        [HttpDelete("{basketId}/lineitem/{productId}")]
        public async Task<IActionResult> DeleteLineItem(Guid basketId, Guid productId, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForValidGuid(basketId, "BasketId");
            Guard.ForValidGuid(productId, "ProductId");
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // returns custom entity ==> BasketItemRemovedEntity
            var basketItemRemoved = await _basketBusinessServices.RemoveItemFromBasket(basketId, productId, correlationToken);

            if (basketItemRemoved == null)
                return BadRequest($"BasketEntity: Cloud not remove line item {productId} from basket {basketId} for Request {correlationToken}");

            return Ok(basketItemRemoved);
        }

        /// <summary>
        ///     Converts Shopping BasketEntity to Order
        /// </summary>
        /// <param name="checkoutDto">New Order information</param>
        /// <returns>The newly-created line item</returns>
        [ProducesResponseType(typeof(CheckoutDto), 200)]
        [ProducesResponseType(400)]
        [HttpPost("CheckOut")]
        public async Task<IActionResult> PostCheckOut([FromBody] CheckoutDto checkoutDto, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForNullObject(checkoutDto, "NewOrder");
            Guard.ForValidGuid(checkoutDto.BasketId, "BasketId in New Order");
            Guard.ForNullOrEmpty(correlationToken, "CorrleationToken");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call into service
            var checkout = await _basketBusinessServices.Checkout(checkoutDto, correlationToken);

            // Ensure that ShoppingBasket exists before executing Checkout operation
            //var basket = await _basketBusinessServices.GetBasketById(checkoutDto.BasketId, correlationToken);

            //if (basket.Count < 1)
            //    return null;

            // Map OrderDto to Order
            //var newOrder = Mapper.MapToOrderInformation(checkoutDto);

            // Call into service
            //await _basketBusinessServices.Checkout(newOrder, correlationToken);

            // Returns 202 status code
            //return new ObjectResult(new CheckoutDto());
            //return Accepted("Order is being created");

            if (checkout == null)
                return BadRequest($"Could not checkout for Basket {checkoutDto.BasketId} for Request {correlationToken}");

            return Accepted(new BasketSummaryDto{
                BasketId = checkout.CheckoutSystemId, 
                BuyerEmail = checkout.BuyerEmail,
                CorrelationId = checkout.CorrelationId}
            );
        }

        /// <summary>
        ///     Removes entire shopping basket, including all line items
        /// </summary>
        /// <param name="basketId">Identifier for user shopping basket</param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [HttpDelete]
        public async Task<HttpStatusCode>  Delete(Guid basketId, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForValidGuid(basketId, "BasketId");
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            await _basketBusinessServices.EmptyBasket(basketId, correlationToken, false);
            return HttpStatusCode.NoContent;
        }
    }
}