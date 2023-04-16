using System;
using System.Reflection;
using EventBus.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Extensions
{
    public static class MessageBrokerExtension
    {
        // Register TopicClient
        public static IServiceCollection RegisterEventBusPublisher(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //var connectionString = configuration["sbconnstrsecret"] ??
            //                       throw new ArgumentNullException("sbconnstrsecret",
            //                           "MessageBroker ConnectionString is Null");

            var connectionString = configuration["sbconnstrsecret"] ??
                                  throw new ArgumentNullException("sbconnstrsecret",
                                      "MessageBroker ConnectionString is Null");


            // This code block registers the ServiceBusEventProvider class with the DI container
            services.AddSingleton<IEventBusPublisher, ServiceBusEventPublisher>(x =>
            {
                var logger = x.GetRequiredService<ILogger<ServiceBusEventPublisher>>();
                var boundService = Assembly.GetExecutingAssembly().GetName().Name;

                // Register MessageBroker for publishing only
                return new ServiceBusEventPublisher(connectionString,
                    boundService,
                    logger);
            });

            return services;
        }
    }
}