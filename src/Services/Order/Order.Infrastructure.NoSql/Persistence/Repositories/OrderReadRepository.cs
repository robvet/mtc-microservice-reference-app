namespace order.infrastructure.nosql.Persistence.Repositories;

using order.domain.Models.ReadModels;
using Microsoft.Extensions.Logging;
using order.infrastructure.nosql.Persistence.Contracts;

public class OrderReadRepository : CosmosDbRepository<OrderReadModel>, IOrderReadRepository
{
    public override string ContainerName => "OrderCollection";
    private readonly ILogger<OrderReadRepository> _logger;


    public OrderReadRepository(ICosmosDbContainerFactory cosmosDbContainerFactory,
                               ILogger<OrderReadRepository> logger)
                                    : base(cosmosDbContainerFactory)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<OrderReadModel>> GetAll(string correlationId)
    {
        //return await GetItemsAsync($"SELECT * FROM c where c.Department.id=\"{departmentId}\"");
        _logger.LogInformation($"Fetched all orders in CosmosDbRepository for Correlation Token {correlationId} ");
        return await GetItemsAsync();
    }

    public async Task<OrderReadModel> GetByOrderId(string Id, string correlationId)
    {
        try
        {
            var query = $"SELECT * FROM c where c.OrderId=\"{Id}\"";

            var response = await GetItemsAsync(query);

            _logger.LogInformation($"Fetching order {Id} in CosmosDbRepository for Correlation Token {correlationId} ");

            // Single says it better be one and only one orderId, else I'll throw an exception
            return response.SingleOrDefault();

        }
        catch (Exception ex)
        {
            var errorMessage = $"Exception thrown in OrderReadRepository.GetByOrderId {ex.Message} for CorrelationId {correlationId}";
            _logger.LogError(errorMessage, ex);
            throw new Exception(errorMessage, ex);
        }



        //return await GetItemsAsync(query);
        //return await GetByIdAsync(query);
        //return await GetByIdAsync(query);


        //return await GetItemsAsync($"SELECT * FROM c where c.Order.id=\"{Id}\"");

        //   var query = new SqlQuerySpec(
        //"SELECT * FROM c WHERE c.productId = @productId",
        //new SqlParameterCollection(new[] { new SqlParameter("@productId", Id) }));

        //   return await GetItemsAsync(query.QueryText);
    }

    public async Task<OrderReadModel> GetByResourceId(string Id, string correlationId)
    {
        return await GetByIdAsync(Id);

        //var result = await GetByIdAsync(Id);
        //return result;

        //var query = $"SELECT * FROM c where c.OrderId=\"{Id}\"";
        //return await GetItemsAsync(query);
        //return await GetByIdAsync(query);
        //return await GetByIdAsync(Id);


        //return await GetItemsAsync($"SELECT * FROM c where c.Order.id=\"{Id}\"");

        //   var query = new SqlQuerySpec(
        //"SELECT * FROM c WHERE c.productId = @productId",
        //new SqlParameterCollection(new[] { new SqlParameter("@productId", Id) }));

        //   return await GetItemsAsync(query.QueryText);
    }



    //public async Task<long> GetEmployeesCountByDepartmentIdAsync(int departmentId)
    //{
    //    return await GetScalarValueAsync<long>($"SELECT VALUE COUNT(1) FROM c where c.Department.id=\"{departmentId}\"");
    //}


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
