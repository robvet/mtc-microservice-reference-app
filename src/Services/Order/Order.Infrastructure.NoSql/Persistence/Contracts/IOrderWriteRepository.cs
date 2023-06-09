﻿namespace order.infrastructure.nosql.Persistence.Contracts
{
    using order.domain.Models.OrderAggregateModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderWriteRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> CreateOrder(Order order, string correlationId);
    }
}
