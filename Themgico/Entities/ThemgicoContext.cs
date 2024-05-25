using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Themgico.Entities
{
    public partial class ThemgicoContext : DbContext
    {
        public ThemgicoContext()
        {
        }

        public ThemgicoContext(DbContextOptions<ThemgicoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Table> Tables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build().GetConnectionString("DefaultConnection");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength();

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CategoryDescription)
                    .HasMaxLength(500)
                    .HasColumnName("categoryDescription");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Phone)
                    .HasName("PK_Customers_1");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Point)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("point");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Author)
                    .HasMaxLength(50)
                    .HasColumnName("author");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mode)
                    .HasMaxLength(50)
                    .HasColumnName("mode");

                entity.Property(e => e.Numberguest).HasColumnName("numberguest");

                entity.Property(e => e.Orderdate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("orderdate");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Tablenumber).HasColumnName("tablenumber");

                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.TablenumberNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Tablenumber)
                    .HasConstraintName("FK_Orders_Tables");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.OrderTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("orderTime");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Products");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("paymentDate");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasColumnName("paymentMethod");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Payment_Orders");

                entity.HasOne(d => d.PhoneNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Phone)
                    .HasConstraintName("FK_Payment_Customers1");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Products_Categories");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RefreshToken");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.ExpiredAt).HasColumnType("date");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IssuedAt).HasColumnType("date");

                entity.Property(e => e.JwtId).HasColumnName("jwtID");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.HasOne(d => d.Account)
                    .WithMany()
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_RefreshToken_Accounts");
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Tablenumber).HasColumnName("tablenumber");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
