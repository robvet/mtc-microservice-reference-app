using System;
using System.Security.Authentication;
using Basket.API.Contracts;
using Basket.API.Domain.BusinessServices;
using Basket.API.Events;
using Basket.API.Extensions;
using Basket.API.Filters;
using Basket.API.Infrastructure.Repository;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestCommunication;
using ServiceDiscovery;

namespace Basket.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //var builder = new ConfigurationBuilder();
            //var azureServiceTokenProvider = new AzureServiceTokenProvider();

            //var keyVaultClient = new KeyVaultClient(
            //    new KeyVaultClient.AuthenticationCallback(
            //        azureServiceTokenProvider.KeyVaultTokenCallback));

            //builder.AddAzureKeyVault(
            //    $"https://{Configuration["VaultName"]}.vault.azure.net/",
            //    keyVaultClient,
            //    new DefaultKeyVaultSecretManager());

            //Configuration = builder.Build();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //LW-1/16/20 - add in config builder for key vault

            // Register backing services
            services.RegisterTelemetryCollector(Configuration);
            services.RegisterDistrbutedCache(Configuration);
            services.RegisterStorageAccount(Configuration);
            services.RegisterEventBusPublisher(Configuration);
            services.RegisterEventBusSubscriber(Configuration);

            // Register concrete dependencies
            services.AddTransient<EmptyBasketEventHandler>();
            services.AddTransient<ProductChangedEventHandler>();
            services.AddTransient<IBasketBusinessServices, BasketBusinessServices>();
            services.AddSingleton<IServiceLocator, ServiceLocator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //// Register telemetry initializer
            //services.AddSingleton<ITelemetryInitializer, ServiceNameTelemetryInitializer>();

            services.Configure<BasketSettings>(Configuration);

            services.AddMvc(config =>
                {
                    // Attach Customer Exception Filter
                    config.Filters.Add(typeof(BasketCustomExceptionFilter));
                }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddControllers()
                .AddNewtonsoftJson(x => { x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shopping BasketEntity API",
                    Version = "v1",
                    Description =
                        "Exposed as microservice for the Microsoft ActivateAzure with Microservices and Containers Workshop. Manages Shopping BasketEntity."
                });

                c.OperationFilter<SwaggerCustomerFilter>();
            });

            // Enable HTTPClient and resiliency support using 
            //services.AddHttpServices(Configuration);

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1
            // Add instance of LoggerFactory for HttpRestClientFactory to consume
            services.AddSingleton<LoggerFactory>();
            services.AddHttpServices<RestClient>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            //loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
            //loggingBuilder.AddConsole();
            //loggingBuilder.AddDebug();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping BasketEntity API V1"); });

            app.ConfigureEventBusListeners(serviceProvider);
        }
    }
}