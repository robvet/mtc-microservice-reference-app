using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order.domain.Contracts;
using order.infrastructure.nosql.Configurations;
using order.infrastructure.nosql.Persistence;
using order.infrastructure.nosql.Persistence.Contracts;
using order.infrastructure.nosql.Persistence.Repositories;
using System;

namespace order.service.Extensions
{
    public static class NoSqlStoreExtensions
    {
        public static IServiceCollection RegisterNoSqlStore(this IServiceCollection services,
            IConfiguration configuration)
        {
            // new Cosmos DB Repository Code configuration
            var connectionString = configuration["cosmosconnection"];
            CosmosClient client = new CosmosClient(connectionString);
            CosmosDbConfiguration cosmosDbConfig = configuration.GetSection(CosmosDbConfiguration.SectionKey).Get<CosmosDbConfiguration>();
            services.AddSingleton<ICosmosDbContainerFactory>(c => new CosmosDbContainerFactory(client, cosmosDbConfig.DatabaseName, cosmosDbConfig.Containers));

            return services;
        }
    }
}
