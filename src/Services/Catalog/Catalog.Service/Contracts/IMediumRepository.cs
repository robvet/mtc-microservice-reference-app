using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Domain.Entities;

namespace catalog.service.Contracts
{
    public interface IMediumRepository : IRepository<Medium>
    {
        Task<List<Medium>> GetAll(string correlationToken);
        Task<Medium> GetById(Guid guidId, string correlationToken);
    }
}