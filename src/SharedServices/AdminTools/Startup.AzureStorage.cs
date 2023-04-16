using AdminTools.Entities;
using AdminTools.TableStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminTools
{
    public static class AzureStorage
    {
        public static IServiceCollection ConfigureAzureTableStorage(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Repository that encapsualtes Azure Table Storage
            services.AddScoped<IBaseRespository<ProductEntity>>(factory =>
            {
                return new BaseRespository<ProductEntity>(
                    new AzureTableSettings(
                        configuration["StorageAccount"],
                        configuration["StorageKey"],
                        configuration["StorageTableName_Catalog"]));
            });

            // Configure Repository that encapsualtes Azure Table Storage


            return services;
        }
    }
}