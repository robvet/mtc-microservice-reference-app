using System;
using Basket.Service.Contracts;
using Basket.Service.Domain.Entities;
using Basket.Service.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Service.Extensions
{
    //public static class StorageExtension
    //{
        //public static IServiceCollection RegisterStorageAccount(
        //    this IServiceCollection services,
        //    IConfiguration configuration)
        //{
        //    services.AddScoped<IAzureTableStorageRespository<ProductTableEntity>>(factory =>
        //    {
        //        return new AzureTableStorageRespository<ProductTableEntity>(
        //            new AzureTableSettings(
        //                configuration["storageaccount"] ??
        //                throw new ArgumentNullException("Storage Account for ProductEntity is Null"),
        //                // 1/16/2020 - lw - storage key now comes from key vault
        //                configuration["storagekeySecret"] ??
        //                throw new ArgumentNullException("StorageKey for ProductEntity is Null"),
        //                configuration["StorageTableName_Catalog"] ??
        //                throw new ArgumentNullException("Storage Table Name for ProductEntity is Null")));
        //    });
     
        //    return services;
        //}
    //}
}