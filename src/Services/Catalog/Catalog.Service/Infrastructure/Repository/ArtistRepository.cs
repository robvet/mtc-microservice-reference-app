﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catalog.service.Contracts;
using catalog.service.Domain.Entities;
using catalog.service.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;

namespace catalog.service.Infrastructure.Repository
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(DataContext ctx) : base(ctx)
        {
        }

        public async Task<Artist> GetById(Guid guidId, string correlationToken)
        {
            var result = await Get().Where(x => x.GuidId == guidId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Artist>> GetAll(string correlationToken)
        {
            // Important to return empty product list if no products exist
            // Avoids errors in UX
            if (IsEmpty())
            {
                return new List<Artist>();
            }

            return await Task.FromResult(Get().ToList());
        }
    }
}