using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tools.Database;

namespace Tools.StartupExtensions
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

            var connectionString = configuration["CatalogConnectionString"];
            if (useInMemoryStore || string.IsNullOrEmpty(connectionString))
                services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<DataContext>(options => { options.UseInMemoryDatabase(""); });
            //else
            //    services.AddEntityFrameworkSqlServer()
            //        .AddDbContext<DataContext>(options => { options.UseSqlServer(connectionStrging); });

            else
                services.AddEntityFrameworkSqlServer()
                    .AddDbContext<DataContext>(options =>
                    {
                        options.UseSqlServer(connectionString, sqlOptions =>
                        {
                            sqlOptions.CommandTimeout(120).EnableRetryOnFailure(maxRetryCount: 5);
                        });
                    });

            return services;
        }
    }
}