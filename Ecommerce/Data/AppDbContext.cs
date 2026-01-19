using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<ShippingAddress> ShippingAddresses => Set<ShippingAddress>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.IsAdmin).HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

            });


            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");


                entity.HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Sku).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Sku).IsUnique();
                entity.Property(e => e.Color).HasMaxLength(30);
                entity.Property(e => e.Size).HasMaxLength(20);
                entity.Property(e => e.PhoneModel).HasMaxLength(50);
                entity.Property(e => e.Material).HasMaxLength(50);
                entity.Property(e => e.Stock).HasDefaultValue(0);
                entity.Property(e => e.Stock).HasColumnType("decimal(10,2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);


                entity.HasOne(e => e.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            });


            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Order).HasDefaultValue(0);
                entity.Property(e => e.IsPrimary).HasDefaultValue(false);

                entity.HasOne(e => e.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.ShippingCost).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Total).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Total).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ShippingAddress)
                .WithOne()
                .HasForeignKey<ShippingAddress>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Payment)
                .WithOne(e => e.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            });


            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.VariantDetails).HasMaxLength(200);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)").IsRequired();

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ProductVariant)
                      .WithMany()
                      .HasForeignKey(e => e.ProductVariantId)
                      .OnDelete(DeleteBehavior.Restrict);


                modelBuilder.Entity<ShippingAddress>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                    entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                    entity.Property(e => e.Street).IsRequired().HasMaxLength(100);
                    entity.Property(e => e.Number).IsRequired().HasMaxLength(10);
                    entity.Property(e => e.AdditionalInfo).HasMaxLength(200);
                    entity.Property(e => e.City).IsRequired().HasMaxLength(50);
                    entity.Property(e => e.State).IsRequired().HasMaxLength(50);
                    entity.Property(e => e.PostalCode).IsRequired().HasMaxLength(10);
                    entity.Property(e => e.Country).IsRequired().HasMaxLength(50);
                    entity.Property(e => e.IsDefault).HasDefaultValue(false);

                    entity.HasOne(e => e.User)
                          .WithMany(e => e.ShippingAddresses)
                          .HasForeignKey(e => e.UserId)
                          .OnDelete(DeleteBehavior.Cascade);
                });

                modelBuilder.Entity<Payment>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                    entity.Property(e => e.TransactionId).HasMaxLength(100);
                    entity.Property(e => e.Amount).HasColumnType("decimal(10,2)").IsRequired();
                    entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                    entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                });
            });
        }
    }
}
