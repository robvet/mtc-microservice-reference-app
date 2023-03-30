using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Ordering.Domain.AggregateModels.BuyerAggregate;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.Infrastructure.Relational.DataStore
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        
        //public DataContext(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public DataContext(DbContextOptions options)
            : base(options)
        { }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    throw new NullReferenceException("Data store configuration is missing");

            ////var connectionString = _configuration["orderdbsecret"] ??
            ////                       throw new ArgumentNullException("Connection string for Catalog database is Null");// Test***************************

            ////optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly create FK for 1-1 relationship
            // https://www.learnentityframeworkcore.com/configuration/one-to-one-relationship-configuration
            // https://www.entityframeworktutorial.net/efcore/configure-one-to-one-relationship-using-fluent-api-in-ef-core.aspx
            
            
            modelBuilder.Entity<Order>(order =>
            {
                order.HasKey(e => e.Id);
                ////order.Ignore(x => x.orderid);
                order.Ignore(x => x.CorrelationToken);
                order.Property(x => x.OrderSystemId).IsRequired();
                order.Property(x => x.CustomerSystemId).IsRequired();
                order.Property(x => x.CheckOutSystemId).IsRequired();
                ////order.Property(x => x.orderid);

                // Explicitly specify 1-M with OrderDetails
                order.HasMany(x => x.OrderDetails)
                    .WithOne(y => y.Order);

                // Explicitly specify 1-1 with Buyer
                order.HasOne(x => x.Buyer)
                    .WithOne(y => y.Order)
                    .HasForeignKey<Buyer>(y => y.OrderId);
            });

            modelBuilder.Entity<OrderDetail>(orderDetail =>
            {
                orderDetail.HasKey(e => e.Id);
                //orderDetail.Property(x => x.Id)..ValueGeneratedOnAdd();
                orderDetail.Property(x => x.Title).IsRequired();
                orderDetail.Property(x => x.Artist).IsRequired();

                // Explicitly specify M-1 with Order
                orderDetail.HasOne(x => x.Order)
                    .WithMany(y => y.OrderDetails);
            });

            modelBuilder.Entity<OrderStatus>(orderStatus =>
            {
                orderStatus.HasKey(x => x.Id);
                orderStatus.Property(x => x.StatusDescription).IsRequired();
            });

            modelBuilder.Entity<Buyer>(buyer =>
            {
                buyer.HasKey(e => e.Id);
                buyer.Property(x => x.UserName).IsRequired();
                buyer.Property(x => x.FirstName).IsRequired();
                buyer.Property(x => x.LastName).IsRequired();
                buyer.Property(x => x.Address).IsRequired();
                buyer.Property(x => x.City).IsRequired();
                buyer.Property(x => x.State).IsRequired();
                buyer.Property(x => x.PostalCode).IsRequired();
                buyer.Property(x => x.Phone).IsRequired();
                buyer.Property(x => x.Email).IsRequired();
                buyer.HasOne<PaymentMethod>(x => x.PaymentMethod)
                    .WithOne(y => y.Buyer)
                    .HasForeignKey<PaymentMethod>(y => y.BuyerId);
            });

            modelBuilder.Entity<PaymentMethod>(paymentMethod =>
            {
                paymentMethod.HasKey(x => x.Id);
                paymentMethod.Property(x => x.CardNumber).IsRequired();
                paymentMethod.Property(x => x.SecurityCode).IsRequired();
                paymentMethod.Property(x => x.CardHolderName).IsRequired();
            });
        }
    }
}