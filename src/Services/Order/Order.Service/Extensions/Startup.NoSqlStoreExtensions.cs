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
            // Register repository class. Note how we pass connection information
            services.AddScoped<IOrderRepository, OrderRepository>(x =>
            {
                return new OrderRepository(
                    new DataStoreConfiguration(
                        configuration["cosmosendpoint"] ??
                        throw new ArgumentNullException("Cosmos endpoint for ordering is Null"),
                        configuration["cosmoskeysecret"] ??
                        throw new ArgumentNullException("Cosmos endpoint for ordering is Null")));
                //configuration["cosmosendpoint"],
                //configuration["cosmoskeysecret"]));
                //configuration["CosmosPrimaryKey"]));
            });


            // new repo code
            CosmosDbConfiguration cosmosDbConfig = configuration.GetSection(CosmosDbConfiguration.SectionKey).Get<CosmosDbConfiguration>();
            CosmosClient client = new CosmosClient(cosmosDbConfig.ConnectionString);
            services.AddSingleton<ICosmosDbContainerFactory>(c => new CosmosDbContainerFactory(client, cosmosDbConfig.DatabaseName, cosmosDbConfig.Containers));

            return services;
        }
    }
}
