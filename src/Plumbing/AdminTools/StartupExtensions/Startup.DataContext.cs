using System;
using AdminTools.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminTools.StartupExtensions
{
    public static class DataContextExtensions
    {
        public static IServiceCollection ConfigureDataContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (configuration == null)
                throw new Exception("configuration is null");

            var runningOnMono = Type.GetType("Mono.Runtime") != null;
            var configInMemory = configuration["Data:UseInMemoryStore"] != null &&
                                 configuration["Data:UseInMemoryStore"]
                                     .Equals("true", StringComparison.OrdinalIgnoreCase);
            var useInMemoryStore = runningOnMono || configInMemory;

            var connectionStrging = configuration["CatalogConnectionString"];
            if (useInMemoryStore || string.IsNullOrEmpty(connectionStrging))
                services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<DataContext>(options => { options.UseInMemoryDatabase(Guid.NewGuid().ToString()); });
            else
                services.AddEntityFrameworkSqlServer()
                    .AddDbContext<DataContext>(options => { options.UseSqlServer(connectionStrging); });

            return services;
        }
    }
}