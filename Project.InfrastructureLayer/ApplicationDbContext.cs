using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Project.InfrastructureLayer.Entities;


namespace Project.InfrastructureLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(op => op.Product)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<Customer>()
                .Property(c => c.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Customer>()
                .Property(c => c.UpdatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Customer>()
                .Property(c => c.Status)
                .HasDefaultValue("InActive");

            modelBuilder.Entity<Customer>()
                .Property(c => c.Address)
                .HasDefaultValue("");

            modelBuilder.Entity<Customer>()
                .Property(c => c.IsAdmin)
                .HasDefaultValue(false);

            modelBuilder.Entity<Customer>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Phone)
                .HasDefaultValue("");

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Order>()
                .Property(o => o.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
