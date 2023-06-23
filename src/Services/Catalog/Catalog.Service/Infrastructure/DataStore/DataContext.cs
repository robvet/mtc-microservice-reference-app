using System;
using catalog.service.Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace catalog.service.Infrastructure.DataStore
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Description> Descriptions { get; set; }
        public virtual DbSet<Medium> Mediums { get; set; }
        public virtual DbSet<Condition> Conditions { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                throw new NullReferenceException("Data store configuration is missing");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AlbumArtUrl).HasMaxLength(160);

                entity.Property(e => e.ParentalCaution).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnType("uniqueidentifier");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(160);

                entity.Property(e => e.Upc)
                    .IsRequired()
                    .HasColumnName("UPC")
                    .HasColumnType("nchar(20)");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Artists");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Genres");

                //entity.HasOne(s => s.Status)
                //    .WithMany(p => p.Products)
                //    .HasForeignKey(d => d.StatusId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Albums_Status");

                entity.HasOne(s => s.Status)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Status");

                entity.HasOne(s => s.Medium)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.MediumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Medium");

                entity.HasOne(c => c.Condition)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Condition");

                //entity.HasOne(d => d.Description)
                //    .WithOne(p => p.Product)
                //    .HasForeignKey<Product>(d => d.DescriptionId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Albums_Description");

                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => e.ArtistId);
                entity.Property(e => e.GuidId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.StatusId);
                entity.Property(e => e.GuidId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Medium>(entity =>
            {
                entity.HasKey(e => e.MediumId);
                entity.Property(e => e.GuidId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Condition>(entity =>
            {
                entity.HasKey(e => e.ConditionId);
                entity.Property(e => e.GuidId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.GenreId);
                entity.Property(e => e.GuidId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            // Create 1-M relationship with Products as parent table
            modelBuilder.Entity<Description>(entity =>
            {
                entity.HasKey(e => e.DescriptionId);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.HasOne(s => s.Product)
                    .WithMany(p => p.Descriptions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Description_Status");
            });


            //modelBuilder.Entity<Status>(entity =>
            //{
            //    entity.HasKey(e => e.Id);

            //    entity.HasOne(s => s.Product)

            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(50);
            //});
        }
    }
}