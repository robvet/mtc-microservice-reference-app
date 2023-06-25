namespace order.infrastructure.nosql.Persistence.Repositories;

using Microsoft.Azure.Documents;
using order.domain.AggregateModels.OrderAggregate;
using order.infrastructure.nosql;
using order.infrastructure.nosql.Persistence.Contracts;

public class Order2Repository : CosmosDbRepository<OrderDto>, IOrder2Repository
{
    public Order2Repository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory)
    {
    }

    public override string ContainerName => "OrderCollection";

    public async Task<IEnumerable<OrderDto>> GetAll(string correlationId)
    {
        //return await GetItemsAsync($"SELECT * FROM c where c.Department.id=\"{departmentId}\"");
        return await GetItemsAsync();
    }

    public async Task<OrderDto> GetById(string Id, string correlationId)
    {
        var result = await GetByIdAsync(Id);
        return result;

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
}
