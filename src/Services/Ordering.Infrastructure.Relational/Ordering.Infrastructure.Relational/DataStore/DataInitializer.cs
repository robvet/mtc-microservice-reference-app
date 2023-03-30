using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.AggregateModels.BuyerAggregate;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.Infrastructure.Relational.DataStore
{
    public static class DataInitializer
    {
        private static int _counter;

        public static async Task InitializeDatabaseAsync(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>();

            try
            {
                if (context != null)
                {
                    var databaseCreated = context.Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                // Todo: Add Logging
                var errorMessage = $"Error creating tables in Ordering database: {ex.Message}";
                throw;
            }

            // Determine if database has been seeded
            if (!context.OrderStatus.Any()) await Seed(context);
        }

        private static async Task Seed(DataContext context)
        {
            var orderStatuses = new List<OrderStatus>
            {
                new OrderStatus(1, "Pending"),
                new OrderStatus(2, "AwaitingValidation"),
                new OrderStatus(3, "StockConfirmed"),
                new OrderStatus(4, "Paid"),
                new OrderStatus(5, "Shipped"),
                new OrderStatus(6, "Complete"),
                new OrderStatus(7, "Cancelled"),
            };

            foreach (var item in orderStatuses)
            {
                context.Add(item);
            }
            
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Todo: Add Logging
                var errorMessage = $"Error seeding Ordering data {ex.Message}";
                throw;
            }
        }
    }
}
