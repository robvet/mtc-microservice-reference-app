using System;
using Catalog.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.DataStore
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

                entity.Property(e => e.Cutout).HasDefaultValueSql("((0))");

                entity.Property(e => e.ParentalCaution).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnType("uniqueidentifier");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(160);

                entity.Property(e => e.Upc)
                    .IsRequired()
                    .HasColumnName("UPC")
                    .HasColumnType("nchar(10)");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Artists");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Genres");

                entity.HasOne(s => s.Status)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Status");

                entity.HasOne(s => s.Medium)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.MediumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Medium");

                entity.HasOne(c => c.Condition)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.ConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Condition");

                entity.HasOne(d => d.Description)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.DescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Albums_Description");

                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Artist>(entity => 
            { 
                entity.HasKey(e => e.ArtistId);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Status>(entity => 
            {
                entity.HasKey(e => e.StatusId); 
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Medium>(entity =>
            {
                entity.HasKey(e => e.MediumId);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Condition>(entity =>
            {
                entity.HasKey(e => e.ConditionId);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Description>(entity =>
            {
                entity.HasKey(e => e.DescriptionId);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Genre>(entity => 
            { 
                entity.HasKey(e => e.GenreId);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
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