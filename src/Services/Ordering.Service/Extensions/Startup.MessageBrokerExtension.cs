using System;
using System.Reflection;
using EventBus.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.API.Events;

namespace Ordering.API.Extensions
{
    public static class MessageBrokerExtension
    {
        // Register TopicClient
        public static IServiceCollection RegisterEventBusPublisher(
            this IServiceCollection services,
            IConfiguration configuration)
        { 
            var connectionString = configuration["sbconnstrsecret"] ??
                                   throw new ArgumentNullException("sbconnstrsecret", "MessageBroker ConnectionString is Null");

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

        // Register SubscriptionClient
        public static IServiceCollection RegisterEventBusSubscriber(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["sbconnstrsecret"] ??
                                   throw new ArgumentNullException("sbconnstrsecret", "MessageBroker ConnectionString is Null");

            var subscriptionName = configuration["SubscriptionName"] ??
                                   throw new ArgumentNullException("SubscriptionName", "MessageBroker SubscriptionName is Null");

            // This code block registers the ServiceBusEventProvider class with the DI container
            services.AddSingleton<IEventBusSubscriber, ServiceBusEventSubscriber>(x =>
            {
                var logger = x.GetRequiredService<ILogger<ServiceBusEventSubscriber>>();
                var boundService = Assembly.GetExecutingAssembly().GetName().Name;

                // Register MessageBroker for publishing only
                return new ServiceBusEventSubscriber(connectionString,
                    boundService,
                    logger,
                    subscriptionName);
            });

            return services;
        }

        public static IApplicationBuilder ConfigureEventBusListeners(this IApplicationBuilder app,
            IServiceProvider serviceProvider)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBusSubscriber>();
            // Get references to eventHandlers
            var checkoutEventHandler = serviceProvider.GetRequiredService<CheckOutEventHandler>();

            // Register events
            // Pass EventType as generic and EventHandler as parameter
            eventBus.Subscribe<CheckOutEvent>(checkoutEventHandler);

            return app;
        }




        //var connectionString = configuration["ServiceBusPublisherConnectionString"];
        //var topicName = configuration["ServiceBusTopicName"];
        //var subscriptionName = configuration["ServiceBusSubscriptionName"];

        //Guard.ForNullOrEmpty(connectionString, "ConnectionString from Basket is Null");
        //Guard.ForNullOrEmpty(topicName, "TopicName from Basket is Null");
        //Guard.ForNullOrEmpty(subscriptionName, "SubscriptionName from Basket is Null");

        //// Add EventHandler to DI Container
        //services.AddTransient<IIntegratedEventHandler, EmptyBasketEventHandler>();
        //services.AddTransient<IIntegratedEventHandler, ProductChangedEventHandler>();

        //// Add Service Bus Connection Object to DI Container
        //services.AddSingleton<IServiceBusPersisterConnection>(sp =>
        //{
        //    var serviceBusConnectionString = connectionString;
        //    var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

        //    return new DefaultServiceBusPersisterConnection(serviceBusConnection);
        //});

        //// Add EventBus to DI Container
        //services.AddSingleton<IEventBus, EventBusServiceBus>(x =>
        //{
        //    var serviceBusPersisterConnection = x.GetRequiredService<IServiceBusPersisterConnection>();
        //    return new EventBusServiceBus(serviceBusPersisterConnection, subscriptionName);
        //});


    }
}