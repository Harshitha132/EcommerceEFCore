
using ECommerceEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=.\SQLEXPRESS;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(c => c.CategoryId);

            entity.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Description)
                .HasMaxLength(500);

            entity.HasIndex(c => c.CategoryName)
                .IsUnique();
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products", table =>
            {
                table.HasCheckConstraint(
                    "CK_Products_Price",
                    "[Price] >= 0");

                table.HasCheckConstraint(
                    "CK_Products_StockQuantity",
                    "[StockQuantity] >= 0");
            });

            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Description)
                .HasMaxLength(1000);

            entity.Property(p => p.Price)
                .HasPrecision(18, 2);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(c => c.CustomerId);

            entity.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.HasIndex(c => c.Email)
                .IsUnique();
        });

        // Cart
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Carts");

            entity.HasKey(c => c.CartId);

            entity.HasOne(c => c.Customer)
                .WithOne(cu => cu.Cart)
                .HasForeignKey<Cart>(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(c => c.CustomerId)
                .IsUnique();
        });

        // CartItem
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItems", table =>
            {
                table.HasCheckConstraint(
                    "CK_CartItems_Quantity",
                    "[Quantity] > 0");
            });

            entity.HasKey(ci => ci.CartItemId);

            entity.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(ci => new { ci.CartId, ci.ProductId })
                .IsUnique();
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders", table =>
            {
                table.HasCheckConstraint(
                    "CK_Orders_TotalAmount",
                    "[TotalAmount] >= 0");
            });

            entity.HasKey(o => o.OrderId);

            entity.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            entity.Property(o => o.OrderStatus)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems", table =>
            {
                table.HasCheckConstraint(
                    "CK_OrderItems_Quantity",
                    "[Quantity] > 0");

                table.HasCheckConstraint(
                    "CK_OrderItems_UnitPrice",
                    "[UnitPrice] >= 0");

                table.HasCheckConstraint(
                    "CK_OrderItems_Subtotal",
                    "[Subtotal] >= 0");
            });

            entity.HasKey(oi => oi.OrderItemId);

            entity.Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            entity.Property(oi => oi.Subtotal)
                .HasPrecision(18, 2);

            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments", table =>
            {
                table.HasCheckConstraint(
                    "CK_Payments_Amount",
                    "[Amount] >= 0");
            });

            entity.HasKey(p => p.PaymentId);

            entity.Property(p => p.Amount)
                .HasPrecision(18, 2);

            entity.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(p => p.PaymentStatus)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(p => p.TransactionReference)
                .HasMaxLength(100);

            entity.Property(p => p.PaymentDate)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(p => p.OrderId)
                .IsUnique();
        });
    }
}
