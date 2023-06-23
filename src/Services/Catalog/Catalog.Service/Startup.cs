using System;
using catalog.service.Contracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using catalog.service.Domain.DataInitializationServices;
using catalog.service.Extensions;
using catalog.service.Infrastructure.Repository;
using catalog.service.Domain.BusinessServices;
using catalog.service.Filters;
using SharedUtilities.Utilties;

namespace catalog.service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Invoked at startup by the runtime.
        // Use to add services to the Dependency Injection container.
        public void ConfigureServices(IServiceCollection services)
        {
            // validate configuration data exists
            Guard.ForNullOrEmpty(Configuration["catalogdbsecret"], "Catalog Database Endpoint Environment Variable not set");
            Guard.ForNullOrEmpty(Configuration["redisconnstrsecret"], "Redis Cache Environment Variable not set");
            Guard.ForNullOrEmpty(Configuration["sbconnstrsecret"], "Message Broker Endpoint Environment Variable not set");
            Guard.ForNullOrEmpty(Configuration["aiinstrumkeysecret"], "Observability Endpoint Environment Variable not set");

            // Register backing services
            services.RegisterTelemetryCollector(Configuration);
            services.RegisterEventBusPublisher(Configuration);
            services.RegisterRelationalDatabase(Configuration);
            services.RegisterDistrbutedCache(Configuration);

            //// Register telemetry initializer
            //services.AddSingleton<ITelemetryInitializer, ServiceNameTelemetryInitializer>();

            // Register concrete dependencies
            services.AddTransient<ICatalogBusinessServices, CatalogBusinessServices>();
            services.AddTransient<IDataSeedingServices, DataSeedingServices>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IArtistRepository, ArtistRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IMediumRepository, MediumRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICatalogBusinessServices, CatalogBusinessServices>();

            // Capture SQL query text in App Insights
            // https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-dependencies
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });

            // Register MVC Core framework as service
            services.AddMvc(config =>
                {
                    // Attach Customer Exception Filter
                    config.Filters.Add(typeof(CatalogCustomExceptionFilter));
                }
            );

            // Register Swashbuckle as service
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Product Catalog API",
                    Version = "v1",
                    Description =
                        "Exposed as microservice for the Microsoft ActivateAzure with Microservices and Containers Workshop. Manages Catalog items."
                });

                c.OperationFilter<SwaggerCustomerFilter>();
            });
        }

        // Use to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider,
            ILoggerFactory log)
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

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API V1"); });

            // Because we declare within serviceScope, we must wrap the call in a serviceScope
            // The code block creates and populates the Product database
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                //// Get DataContext and Logger explicitly from DI container
                //var context = serviceScope.ServiceProvider.GetService<DataContext>();

                //Guard.ForNullObject(context, "DataContext not found in DI container");

                //// Create database and tables, but not populate data
                //var databaseCreated = context.Database.EnsureCreated();
                //DataInitializer.InitializeDatabaseAsync(serviceScope).Wait();


                //new ProductDatabaseInitializer().InitializeDatabaseAsync(serviceScope).Wait();
            }
        }
    }
}