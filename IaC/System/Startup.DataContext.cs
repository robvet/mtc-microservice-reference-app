using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tools.Database;

namespace Tools
{
    public static class DataContextExtensions
    {
        public static IServiceCollection ConfigureDataContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (configuration == null)
                throw new Exception("configuration is null");

            var connectionString = configuration["catalogdbsecret"];
 
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<DataContext>(options => { options.UseSqlServer(connectionString); });

            return services;
        }
    }
}