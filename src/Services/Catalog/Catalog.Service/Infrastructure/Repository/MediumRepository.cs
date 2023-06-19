using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catalog.service.Contracts;
using catalog.service.Domain.Entities;
using catalog.service.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;

namespace catalog.service.Infrastructure.Repository
{
    public class MediumRepository : BaseRepository<Medium>, IMediumRepository
    {
        public MediumRepository(DataContext ctx) : base(ctx)
        {
        }

        public async Task<Medium> GetById(Guid guidId, string correlationToken)
        {
            var result = await Get().Where(x => x.GuidId == guidId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Medium>> GetAll(string correlationToken)
        {
            // Important to return empty product list if no products exist
            // Avoids errors in UX
            if (IsEmpty())
            {
                return new List<Medium>();
            }

            return await Task.FromResult(Get().ToList());
        }
    }
}