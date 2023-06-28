namespace order.infrastructure.nosql.Persistence.Repositories;

using order.infrastructure.nosql.Persistence.Contracts;
using Microsoft.Extensions.Logging;
using order.domain.Models.OrderAggregateModels;

public class OrderWriteRepository : CosmosDbRepository<Order>, IOrderWriteRepository
{
    public override string ContainerName => "OrderCollection";
    private readonly ILogger<OrderWriteRepository> _logger;

    public OrderWriteRepository(ICosmosDbContainerFactory cosmosDbContainerFactory, 
                                ILogger<OrderWriteRepository> logger) 
                                        : base(cosmosDbContainerFactory)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> CreateOrder(Order order, string correlationId)
    {
        Order? newOrder = null;

        try
        {
            // ** Implement Idempotency check **
            // If the order already exists, don't create it again.'
            // Based decision on correlationId. If the correlationId already exists, then we've already created the order.
            // But, we most likely exceeded the Service Bus PeekLock timeout, which is 30 seconds and the message has been
            // made available again on the queue. So, we need to check if the order already exists in the database.
            var ordersCreateWithCorrelationId = await GetItemsAsync($"SELECT * FROM c where c.CorrelationToken=\"{correlationId}\"");

            if (ordersCreateWithCorrelationId.Count() == 0)
            {
                newOrder = await AddAsync(order);
                _logger.LogInformation($"Created new order in CosmosDbRepository for Correlation Token {correlationId} ");
            }
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error creating new order in CosmosDbRepository with correlationId {correlationId}";
            _logger.LogError(errorMessage, ex);
            throw new Exception(errorMessage, ex);
        }

        if (newOrder != null)
        {
            return new List<Order> { newOrder };
        }
        else
        {
            _logger.LogInformation($"Didn't create order in CosmosDbRepository as Correlation Token {correlationId} already exists in database ");
            return new List<Order>();
        }
    }
}

   
