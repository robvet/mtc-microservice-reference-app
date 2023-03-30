using System;
using Catalog.API.Contracts;
using Catalog.API.Domain.BusinessServices;
using Catalog.API.Extensions;
using Catalog.API.Filters;
using Catalog.API.Infrastructure.DataStore;
using Catalog.API.Infrastructure.Repository;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Catalog.API
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
            // Register backing services
            services.RegisterTelemetryCollector(Configuration);
            services.RegisterEventBusPublisher(Configuration);
            services.RegisterRelationalDatabase(Configuration);

            //// Register telemetry initializer
            //services.AddSingleton<ITelemetryInitializer, ServiceNameTelemetryInitializer>();

            // Register concrete dependencies
            services.AddTransient<ICatalogBusinessServices, CatalogBusinessServices>();
            services.AddTransient<IMusicRepository, MusicRepository>();
            services.AddTransient<IArtistRepository, ArtistRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
                DataInitializer.InitializeDatabaseAsync(serviceScope).Wait();
            }
        }
    }
}