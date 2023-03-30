using System;
using EventBus.Events;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ordering.API.Commands;
using Ordering.API.Events;
using Ordering.API.Extensions;
using Ordering.API.Filters;
using Ordering.API.Queries;
using Ordering.Domain.AggregateModels.OrderAggregate;
using Ordering.Infrastructure.NoSql.Repositories;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register backing services
            services.RegisterTelemetryCollector(Configuration);
            services.RegisterEventBusPublisher(Configuration);
            services.RegisterEventBusSubscriber(Configuration);
            services.RegisterNoSqlStore(Configuration);

            //// Register telemetry initializer
            //services.AddSingleton<ITelemetryInitializer, ServiceNameTelemetryInitializer>();

            // Register concrete dependencies
            services.AddTransient<IMessageEventHandler, CheckOutEventHandler>();
            services.AddTransient<CheckOutEventHandler>();
            services.AddTransient<IOrderQueries, OrderQueries>();
            services.AddTransient<CreateOrderCommandHandler>();

            // Capture SQL query text in App Insights
            // https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-dependencies
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });

            services.AddMvc(config => { config.Filters.Add(typeof(OrderingCustomExceptionFilter)); });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ordering Services API",
                    Version = "v1",
                    Description =
                        "Ordering API service for the Microsoft ActivateAzure with Microservices and Containers Workshop."
                    //Contact = new Contact { Title = "Rob Vettor", Email = "robvet@microsoft.com", Url = "www.thinkinginpaas.com" },
                });

                c.OperationFilter<SwaggerCustomerFilter>();
            });

            services.AddApiVersioning(x =>
            {
                // Allows for API to return a version in the response header
                x.ReportApiVersions = true;
                // Default version for clients not specifying a version number
                x.AssumeDefaultVersionWhenUnspecified = true;
                // Specifies version to which to default. This is the version
                // to which you are routed if no version is specified
                x.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // ServiceLocator-based call that instantiates the RegisterMesageHandler class and invokes its
            // Register() method which sets a callback for underlying ServiceBus Subscription.
            ////serviceProvider.GetService<IRegisterEventHandler>().Register();

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering Services API V1"); });

            app.ConfigureEventBusListeners(serviceProvider);
        }
    }
}

//************************************************
//* Save for CQRS
//************************************************
//////private void RegisterCommandBus(IServiceCollection services)
//////{
//////    var connectionString = Configuration["sbconnstrsecret"] ??
//////                           throw new ArgumentNullException("MessageBroker ConnectionString is Null");

//////    var queueName = Configuration["QueueName"] ??
//////                    throw new ArgumentNullException("MessageBroker Queue Title is Null");

//////    // This code block registers the CommandBusProvider class with the DI container
//////    services.AddSingleton<ICommandBus, ServiceBusCommandProvider>(x =>
//////    {
//////        var logger = x.GetRequiredService<ILogger<ServiceBusCommandProvider>>();
//////        var boundService = Assembly.GetExecutingAssembly().GetName().ToString();

//////        // Register MessageBroker for publishing and receiving
//////        return new ServiceBusCommandProvider(connectionString, boundService, queueName, logger, true);
//////    });
//////}

//////private void ConfigureCommandBus(IApplicationBuilder app, IServiceProvider serviceProvider)
//////{
//////    var commandBus = app.ApplicationServices.GetRequiredService<ICommandBus>();

//////    // Get references to command handler
//////    var productCommandHandler = serviceProvider.GetRequiredService<AddProductCommandHandler>();

//////    // Register command handler
//////    commandBus.Subscribe<AddProductCommand>(productCommandHandler);
//////}
///
///
/// private void RegisterCommandBus(IServiceCollection services)
////{
////    var connectionString = Configuration["sbconnstrsecret"] ??
////                           throw new ArgumentNullException("MessageBroker ConnectionString is Null");

////    var queueName = Configuration["QueueName"] ??
////                    throw new ArgumentNullException("MessageBroker Queue Title is Null");

////    // This code block registers the CommandBusProvider class with the DI container
////    services.AddSingleton<ICommandBus, ServiceBusCommandProvider>(x =>
////    {
////        var logger = x.GetRequiredService<ILogger<ServiceBusCommandProvider>>();
////        var boundService = Assembly.GetExecutingAssembly().GetName().ToString();

////        // Register MessageBroker for publishing only
////        return new ServiceBusCommandProvider(connectionString, boundService, queueName, logger, false);
////    });
////}