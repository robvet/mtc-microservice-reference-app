//namespace CosmosDbRepositoryDemo.Application.Interfaces.Persistence;

//using order.domain.AggregateModels.OrderAggregate;
//using order.domain.Contracts;

//public interface IOrder2Repository : IRepository<Order>
//{
//    Task<IEnumerable<Order>> GetEmployeesByDepartmentIdAsync(int departmentId);
//    Task<long> GetEmployeesCountByDepartmentIdAsync(int departmentId);
//}

namespace order.infrastructure.nosql.Persistence.Contracts
{
    using order.domain.AggregateModels.OrderAggregate;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrder2Repository : IRepository<OrderDto>
    {
        Task<IEnumerable<OrderDto>> GetAll(string correlationId);

        Task<IEnumerable<OrderDto>> GetByOrderId(string Id, string correlationId);

        Task<OrderDto> GetByResourceId(string Id, string correlationId);
        //Task<IEnumerable<Order>> GetEmployeesByDepartmentIdAsync(int departmentId);
        //Task<long> GetEmployeesCountByDepartmentIdAsync(int departmentId);
    }
}
