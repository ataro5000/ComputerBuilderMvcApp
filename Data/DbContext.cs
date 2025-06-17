using ComputerBuilderMvcApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComputerBuilderMvcApp.Data
{
    // ApplicationDbContext manages the database sets and relationships for the application.
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<Customer>(options)
    {
        public DbSet<Component> Component { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Component has many Reviews, each Review references a Component by ItemId
            modelBuilder.Entity<Component>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Component)
                .HasForeignKey(r => r.ItemId);

            // Order has one Customer, Customer has many Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();

            // Order has many OrderItems, each OrderItem references an Order by OrderId
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            // OrderItem references a Component by ComponentId
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Component)
                .WithMany()
                .HasForeignKey(oi => oi.ComponentId);

            // Set decimal precision for TotalAmount and UnitPrice
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            // Customer has many Orders, each Order references a Customer by CustomerId
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();
        }
    }
}