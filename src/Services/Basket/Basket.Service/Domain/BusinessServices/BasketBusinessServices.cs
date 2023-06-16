using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Service.Contracts;
using Basket.Service.Domain.Entities;
using Basket.Service.Dtos;
using Basket.Service.Events;
using EventBus.Bus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using RestCommunicator;
using ServiceLocator;
using SharedUtilities.TokenGenerator;

namespace Basket.Service.Domain.BusinessServices
{
    /// <summary>
    ///     Shopping basket domain business object
    /// </summary>
    public class BasketBusinessServices : IBasketBusinessServices
    {
        private readonly IDistributedCacheRepository _distributedCacheRepository;
        private const string ProductPartitionKey = "MusicProduct";
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IEventBusSubscriber _eventBusSubscriber;
        private readonly ILogger<BasketBusinessServices> _logger;
        private readonly IAzureTableStorageRespository<ProductTableEntity> _productTableRepository;
        private readonly TelemetryClient _telemetryClient;
        private readonly IRestClient _restClient;

        public BasketBusinessServices(IDistributedCacheRepository distributedCacheRepository,
            IAzureTableStorageRespository<ProductTableEntity> productTableRepository,
            ILogger<BasketBusinessServices> logger,
            IEventBusPublisher eventBusPublisher,
            IEventBusSubscriber eventBusSubscriber,
            IRestClient restClient,
            TelemetryClient telemetryClient)
        {
            _distributedCacheRepository = distributedCacheRepository;
            _productTableRepository = productTableRepository;
            _logger = logger;
            _eventBusPublisher = eventBusPublisher;
            _eventBusSubscriber = eventBusSubscriber;
            _restClient = restClient;
            _telemetryClient = telemetryClient;
        }

        /// <summary>
        ///     Gets specified shopping basket by BasketEntity Id
        /// </summary>
        /// <param name="correlationToken">Tracks request - can be any value</param>
        /// <param name="basketId">Id of shopping basket</param>
        ///// <returns>BasketItemEntity</returns>
        public async Task<List<Entities.BasketEntity>> GetAllBaskets(string correlationToken)
        {
            //return await _distributedCacheRepository.GetAllBaskets(correlationToken, _telemetryClient);
            return await _distributedCacheRepository.GetAll<BasketEntity>(correlationToken, _telemetryClient);
        }

        /// <summary>
        /// Gets specified shopping basket by BasketEntity Id
        /// </summary>
        /// <param name="correlationToken">Tracks request - can be any value</param>
        /// <param name="basketId">Id of shopping basket</param>
        /// <returns>BasketItemEntity</returns>
        public async Task<Entities.BasketEntity> GetBasketById(Guid basketId, string correlationToken)
        {
            return await _distributedCacheRepository.GetAsync<BasketEntity>(basketId, _telemetryClient, "GetBasketById", correlationToken);
            // return await _distributedCacheRepository.GetBasketAsync(basketId, correlationToken, _telemetryClient);
        }

        /// <summary>
        ///     Add single line item to shopping basket
        /// </summary>
        /// <param name="productId">Id of productEntity to add</param>
        /// <param name="correlationToken">Tracks request - can be any value</param>
        /// <param name="basketId">Id of shopping basket</param>
        /// <returns>BasketItemEntity</returns>
        public async Task<BasketEntity> AddItemToBasket(Guid basketId, Guid productId, string correlationToken)
        {
            //* Materialized View Pattern
            //* Fetch product entity data from a read-only store contained in shopping basket service.
            //* ProductEntity ID is row key in underlying Azure table.
            //* Returns a ProductTableEntity class
            ProductTableEntity productTableEntity = null;

            ProductEntity productEntity;

            // Query Catalog Read store (materialized view) for information about the selected product
            try
            {
                
                
                productEntity = await _distributedCacheRepository.GetAsync<ProductEntity>(productId, _telemetryClient, "AddItemToBasket", correlationToken);




                //productTableEntity = await _productTableRepository.GetItem(ProductPartitionKey, productId.ToString(), correlationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message,
                "Exception throw in AddItemToBasket() in BasketBusinessServices : {message}", ex.Message);

                throw new Exception($"Error in AddItemToBasket() in BasketBusinessServices : {ex.Message}");
            }

            // Fallback logic
            if (productEntity == null)
            {
                // Fallback:
                // If product not available from local read store, fetch it from catalog service by
                // making direct HTTP call to Catalog Service.
                _logger.LogInformation(
                   $"Product {productId} not found in local read store. Making direct call to Catalog.");

                var product = await _restClient.GetAsync<ProductEntity>(ServiceEnum.Catalog, $"api/Catalog/Music/{productId}", correlationToken);

                if (product == null)
                    throw new Exception(
                        $"Cannot add item to shopping basket: ProductEntity #{productId} does not exist for Request {correlationToken}.  Have you created the ProductEntity Read Model for the Shopping BasketEntity microservice?");

                // Transform product into an entity class for table storage
                productTableEntity = new ProductTableEntity
                {
                    // parition key is constant
                    PartitionKey = ProductPartitionKey,
                    // row key is productId
                    //RowKey = product.Data.Id.ToString(),
                    
                    Title = product.Data.Title,
                    //Id = product.Data.Id,
                    GenreName = product.Data.Genre,
                    ArtistName = product.Data.Artist,
                    Price = product.Data.Price.ToString()
                };

                // Add product entity tolocal read store, implementing a cache-aside pattern
                await _productTableRepository.Insert(productTableEntity, correlationToken);

                _logger.LogInformation(
                    $"Added productEntity information for item {productId} for Request {correlationToken} to the read model.");
            }

            BasketEntity basket = null;
            
            // Does basketID exist?
            if (basketId == Guid.Empty)
            {
                basketId = Guid.NewGuid(); // TokenGenerator.GenerateId(TokenGeneratorEnum.Basket);
            }
            else
            {
                //basket = await _distributedCacheRepository.GetBasketAsync(basketId, correlationToken, _telemetryClient);
                basket = await _distributedCacheRepository.GetAsync<BasketEntity>(basketId, _telemetryClient, "AddItemToBasket", correlationToken);
            }

            // Get basket from cache
            if (basket == null)
            {
                // BasketEntity is null, add basket and product to it
                basket = new BasketEntity
                {
                    BuyerId = Guid.Empty,
                    BasketId = basketId,
                    CorrelationToken = correlationToken,
                    Count = 1,
                    Items =
                    {
                        new BasketItemEntity
                        {
                            BasketParentId = basketId,
                            CorrelationToken = correlationToken,
                            DateCreated = DateTime.Now,
                            Title = productEntity.Title,
                            UnitPrice = productEntity.Price.ToString(),
                            Artist = productEntity.Artist,
                            Genre = productEntity.Genre,
                            Condition = productEntity.Condition,
                            Medium = productEntity.Medium,
                            Status = productEntity.Status,
                            Quantity = 1,
                            ProductId = productId,
                        }
                    }
                };

                await _distributedCacheRepository.UpdateAsync<BasketEntity>(basket, basketId, correlationToken, _telemetryClient);
                //basket = await _distributedCacheRepository.UpdateBasketAsync(basket, correlationToken, _telemetryClient);

                _logger.LogInformation($"Created new shopping basket {basketId} and added productEntity {productEntity.Title} for Request {correlationToken} ");
            }
            else 
            {
                // BasketEntity is not null
                // Determine if the same productEntity has already been added to the basket
                var itemInBasket = basket.Items.FirstOrDefault(x => x.ProductId == productId);

                if (itemInBasket == null)
                {
                    // ProductEntity does not exist in basket, add it
                    basket.Items.Add(new BasketItemEntity()
                    {
                        BasketParentId = basketId,
                        CorrelationToken = correlationToken,
                        DateCreated = DateTime.Now,
                        Title = productEntity.Title,
                        UnitPrice = productEntity.Price.ToString(),
                        Artist = productEntity.Artist,
                        Genre = productEntity.Genre,
                        Condition = productEntity.Condition,
                        Medium = productEntity.Medium,
                        Status = productEntity.Status,
                        Quantity = 1,
                        ProductId = productId,
                    });

                    _logger.LogInformation($"Added productEntity Id {productId} to shopping basket {basketId} for request {correlationToken}");
                }
                else
                {
                    // Idempotency write-check
                    // Ensure that update with same correlation token does not already exist. 
                    // This could happen if we've already committed the write, but have gotten caught-up in retry logic.
                    if (itemInBasket.CorrelationToken != null && itemInBasket.CorrelationToken == correlationToken)
                    {
                        _logger.LogWarning($"ProductEntity Id {productId} already added to shopping basket {basketId} for Request {correlationToken}");
                        return basket;
                    }

                    // ProductEntity already exists in basket. Increment its count.
                    //basket.Items.FirstOrDefault(x => x.ProductId == Int32.Parse(productTableEntity.RowKey)).Quantity++;

                    basket.Items.FirstOrDefault(x => x.ProductId == productId).Quantity++;


                    _logger.LogInformation($"Added productEntity Id {productId} to existing shopping basket {basketId} for request {correlationToken}");
                }

                // Increment basket count
                basket.Count++;

                await _distributedCacheRepository.UpdateAsync<BasketEntity>(basket, basketId, correlationToken, _telemetryClient);
                //basket = await _distributedCacheRepository.UpdateBasketAsync(basket, correlationToken, _telemetryClient);
            }

            return basket;
        }

        /// <summary>
        ///     Removes single line item from shopping basket
        /// </summary>
        /// <param name="productId">Id of productEntity to add</param>
        /// <param name="correlationToken">Tracks request - can be any value</param>
        /// <param name="basketId">Id of shopping basket</param>
        /// <returns>BasketItemRemoveEntity</returns>
        public async Task<BasketItemRemovedEntity> RemoveItemFromBasket(Guid basketId, Guid productId, string correlationToken)
        {
            //Get reference to shopping basket item
            var currentBasket = await _distributedCacheRepository.GetAsync<BasketEntity>(basketId, _telemetryClient, "RemoveItemFromBasket", correlationToken);
            //var currentBasket = await _distributedCacheRepository.GetBasketAsync(basketId, correlationToken, _telemetryClient);

            //Get the right item matching the prodcut
            var itemInBasket = currentBasket.Items.FirstOrDefault(x => x.ProductId == productId);

            if (itemInBasket == null)
            {
             
                _logger.LogWarning("Could not locate shopping basket {id} for request {token}", productId,
                  correlationToken);
                return null;
            }
            else
            {
                if (itemInBasket.Quantity > 1)
                {

                    if (itemInBasket.CorrelationToken != correlationToken)
                    {
                        itemInBasket.Quantity--;
                        itemInBasket.CorrelationToken = correlationToken;
                        await _distributedCacheRepository.UpdateAsync<BasketEntity>(currentBasket, basketId, correlationToken, _telemetryClient);
                        //await _distributedCacheRepository.UpdateBasketAsync(currentBasket, correlationToken, _telemetryClient);
                    }
                }
                else
                {
                    currentBasket.Items.Remove(itemInBasket);
                    await _distributedCacheRepository.UpdateAsync<BasketEntity>(currentBasket, basketId, correlationToken, _telemetryClient);
                    //await _distributedCacheRepository.UpdateBasketAsync(currentBasket, correlationToken, _telemetryClient);
                     _logger.LogInformation("Removed both productEntity Id {id} and shopping basket {basket} for request {token}",
                        productId, basketId, correlationToken);
                }
            }

            // Calculate basket count and total

            var basket = await _distributedCacheRepository.GetAsync<BasketEntity>(basketId, _telemetryClient, "RemoveItemFromBasket", correlationToken);
            //var basket = await _distributedCacheRepository.GetBasketAsync(basketId, correlationToken, _telemetryClient);

            // Construct return type
            var basketItemRemoved = new BasketItemRemovedEntity
            {
                Message = $"{itemInBasket} has been removed from your shopping basket.",
                DeleteId = productId,
                // ItemCount is the remaining number of identical items that were just removed
                ItemCount = itemInBasket.Quantity,
                //BasketCount = basket.Quantity,
                BasketTotal = basket.Items.Sum(x => decimal.Parse(x.UnitPrice))
            };

            return basketItemRemoved;
        }

        /// <summary>
        ///     Removes entire shopping basket
        /// </summary>
        /// <param name="basketId">Identifier for BasketEntity</param>
        /// <param name="correlationToken">Tracks request - can be any value</param>
        /// <param name="hasOrderBeenCreated">Flag that indicates is basket emptied for new order</param>
        public async Task EmptyBasket(Guid basketId, string correlationToken, bool hasOrderBeenCreated)
        {
            //Empty BasketEntity
            await _distributedCacheRepository.DeleteAsync<BasketEntity>(basketId, _telemetryClient);
            //await _distributedCacheRepository.DeleteBasketAsync(basketId, _telemetryClient);
        }

        /// <summary>
        ///     Invokes check-out process
        /// </summary>
        /// <param name="checkout">User information to create new order</param>
        /// <param name="correlationToken">Tracks request - can be any value</param>
        /// <returns></returns>
        public async Task<CheckoutEntity> Checkout(CheckoutDto checkout, string correlationToken)
        {
            // The shopping basket service is responsible for managing the user's shopping experirence.
            // It does not create or manage with orders. 
            // Construct and publish the checkout event so that other services can manage those tasks.
            var basketId = checkout.BasketId;

            // TODO: Capture UserID from Security Prinicpal
            //var userName = ClaimsPrincipal.Current.Identity.Name ?? "Generic User";
            //https://davidpine.net/blog/principal-architecture-changes/

            // Get user's shopping basket from storage
            var basket = await GetBasketById(basketId, correlationToken);

            if (basket == null)
            {
                _logger.LogError($"Shopping Basket {basketId} not found. Correlation Token: {correlationToken}");
                throw new Exception($"Shopping Basket {basketId} not found. Correlation Token: {correlationToken}");
            }

            // Create the OrderInformationModel
            var orderInformationModel = new OrderInformationModel();

            // Create buyer information object
            var buyer = new OrderInformationModel.BuyerInformation()
            {
                Username = checkout.Username,
                FirstName = checkout.FirstName,
                LastName = checkout.LastName,
                Address = checkout.Address,
                City = checkout.City,
                State = checkout.State,
                PostalCode = checkout.PostalCode,
                Email = checkout.Email,
                Phone = checkout.Phone,
            };

            // Create payment information object
            var payment = new OrderInformationModel.PaymentInformation()
            {
                CreditCardNumber = checkout.CreditCardNumber,
                SecurityCode = checkout.SecurityCode,
                CardholderName = checkout.CardholderName,
                ExpirationDate = checkout.ExpirationDate
            };

            // Create event class that will contain shopping basket, buyer, and payment information
            // to create a new order.

            orderInformationModel.BasketId = basketId;
            // Generate system checkoutId using snowflake
            orderInformationModel.CheckoutId = Guid.NewGuid();// TokenGenerator.GenerateId(TokenGeneratorEnum.Checkout);
            orderInformationModel.Total = basket.Items.Sum(x => decimal.Parse(x.UnitPrice) * x.Quantity);
            orderInformationModel.Buyer = buyer;
            orderInformationModel.Payment = payment;

            foreach (var item in basket.Items)
                orderInformationModel.LineItems.Add(new OrderInformationModel.LineItem() 
                {
                    ProductId = item.ProductId,
                    Title = item.Title,
                    Artist = item.Artist,
                    Genre = item.Genre,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                });

            var checkoutEvent = new CheckOutEvent
            {
                OrderInformationModel = orderInformationModel,
                CorrelationToken = correlationToken
            };
       

            _logger.LogInformation($"Check-out operation invoked for shopping basket {basketId} for Request {correlationToken}");

            //************** Publish Event  *************************
            // Publish customer checkout event
            // Ordering subscribes to event and creates order. 
            // Once created, Ordering publishes event to have BasketEntity remove basket.
            await _eventBusPublisher.Publish<CheckOutEvent>(checkoutEvent);

            return (new CheckoutEntity
            {
                CheckoutSystemId = checkoutEvent.OrderInformationModel.CheckoutId,
                BuyerEmail = checkoutEvent.OrderInformationModel.Buyer.Email,
                CorrelationId = correlationToken
            });
        }
        /// <summary>
        ///     Event handler that listens for PrdouctChanged Events from the catalog service
        ///     and synchronizes the Catalog Read Table in BasketEntity accordingly.
        ///     Handles both productEntity inserts and updates.
        /// </summary>
        /// <param name="productEntity">ProductEntity Information</param>
        /// <param name="correlationId">Tracks request - can be any value</param>
        /// <returns></returns>
        public async Task ProductChanged(ProductEntity productEntity, string correlationId)
        {
            //if (productEntity == null)
            //{
            //    _logger.LogWarning($"Missing productEntity information for synchronization event in BasketEntity");
            //    throw new Exception($"Missing productEntity information for synchronization event in BasketEntity");
            //}

            //// Transform to ProductTableEntity objects
            //var productTableEntity = new ProductTableEntity
            //{
            //    PartitionKey = ProductPartitionKey,
            //    RowKey = productEntity.Id.ToString(),
            //    Title = productEntity.Title,
            //    ArtistName = productEntity.Artist,
            //    GenreName = productEntity.Genre,
            //    Price = productEntity.Price.ToString(),
            //    //Upc = productEntity.Upc
            //};

            //try
            //{
            //    // Determine if record already exists
            //    var currentReadTableItems =
            //        await _productTableRepository.GetItem(ProductPartitionKey,
            //            productTableEntity.RowKey, correlationId);

            //    if (currentReadTableItems == null)
            //    {
            //        // Insert resource
            //        await _productTableRepository.Insert(productTableEntity, correlationId);

            //        _logger.LogInformation(
            //            $"Inserting ProductEntity {productEntity.Id}, {productTableEntity.Title} into Catalog ReadModel for Request {correlationId}" );
            //    }
            //    else
            //    {
            //        // Update resource
            //        await _productTableRepository.Update(productTableEntity, correlationId);

            //        _logger.LogInformation(
            //            $"Updating ProductEntity {productEntity.Id}, {productTableEntity.Title} to Catalog ReadModel  for Request {correlationId}");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(new EventId(ex.HResult),
            //        ex,
            //        "Exception throw in ProductChanged() in BasketEntity : {message}", ex.Message);

            //    throw new Exception($"Error in ProductChanged() in BasketEntity : {ex.Message}");
            //}
            
            await Task.CompletedTask;
        }
    }
}