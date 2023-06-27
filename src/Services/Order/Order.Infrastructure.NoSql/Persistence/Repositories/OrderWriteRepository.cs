namespace order.infrastructure.nosql.Persistence.Repositories;

using Microsoft.Azure.Documents;
using order.domain.AggregateModels.OrderAggregate;
using order.infrastructure.nosql;
using order.infrastructure.nosql.Persistence.Contracts;
using Microsoft.Extensions.Logging;

public class OrderWriteRepository : CosmosDbRepository<Order>, IOrderWriteRepository
{
    public override string ContainerName => "OrderCollection";
    private readonly ILogger<OrderWriteRepository> _logger;

    public OrderWriteRepository(ICosmosDbContainerFactory cosmosDbContainerFactory, ILogger<OrderWriteRepository> logger) : base(cosmosDbContainerFactory)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> CreateOrder(Order order, string correlationId)
    {
        Order? newOrder = null;

        try
        {
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

    //public async Task<IEnumerable<Order>> GetByOrderId(string Id, string correlationId)
    //{

    //    var query = $"SELECT * FROM c where c.OrderId=\"{Id}\"";
    //    return await GetItemsAsync(query);
    //    //return await GetItemsAsync(query);
    //    //return await GetByIdAsync(query);
    //    //return await GetByIdAsync(query);


    //    //return await GetItemsAsync($"SELECT * FROM c where c.Order.id=\"{Id}\"");

    //    //   var query = new SqlQuerySpec(
    //    //"SELECT * FROM c WHERE c.productId = @productId",
    //    //new SqlParameterCollection(new[] { new SqlParameter("@productId", Id) }));

    //    //   return await GetItemsAsync(query.QueryText);
    //}



    //public async Task<Order> GetByResourceId(string Id, string correlationId)
    //{
    //    return await GetByIdAsync(Id);

    //    //var result = await GetByIdAsync(Id);
    //    //return result;

    //    //var query = $"SELECT * FROM c where c.OrderId=\"{Id}\"";
    //    //return await GetItemsAsync(query);
    //    //return await GetByIdAsync(query);
    //    //return await GetByIdAsync(Id);


    //    //return await GetItemsAsync($"SELECT * FROM c where c.Order.id=\"{Id}\"");

    //    //   var query = new SqlQuerySpec(
    //    //"SELECT * FROM c WHERE c.productId = @productId",
    //    //new SqlParameterCollection(new[] { new SqlParameter("@productId", Id) }));

    //    //   return await GetItemsAsync(query.QueryText);
    //}



    //public async Task<long> GetEmployeesCountByDepartmentIdAsync(int departmentId)
    //{
    //    return await GetScalarValueAsync<long>($"SELECT VALUE COUNT(1) FROM c where c.Department.id=\"{departmentId}\"");
    //}
}
