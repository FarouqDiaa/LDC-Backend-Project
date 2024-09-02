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
                .HasIndex(c => c.Email)
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
            modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = Guid.Parse("f005e3b3-9385-4a29-9d1f-3b5371d23d99"),
                Name = "First User",
                Email = "firstuser@example.com",
                Address = "I live here",
                Phone = "+201111111111",
                Status = "InActive",
                PasswordHash = "D5QKIZDafAB4nsYxXEqcmxELEFM5EGliFOAwIuZ/B6o=",
                PasswordSalt = "U5KJucTPYfhhl9avWxEBcdV0z9ZimJ2jj19swLCUpIw=",                
                //Password is Aa@12345678
                IsAdmin = false,
                CreatedOn = DateTime.Parse("2024-09-01 21:55:55.4066667"),
                UpdatedOn = DateTime.Parse("2024-09-01 21:55:55.4066667")
            },
            new Customer
            {
                Id = Guid.Parse("d08e62fd-4ecb-4c04-b8ae-ab50094eeb91"),
                Name = "SuperAdmin",
                Email = "farouq@admin.com",
                Address = "LDC",
                Phone = "01111111111",
                Status = "InActive",
                PasswordHash = "D5QKIZDafAB4nsYxXEqcmxELEFM5EGliFOAwIuZ/B6o=",
                PasswordSalt = "U5KJucTPYfhhl9avWxEBcdV0z9ZimJ2jj19swLCUpIw=",
                //Password is Aa@12345678
                IsAdmin = true,
                CreatedOn = DateTime.Parse("2024-08-26 11:17:03.9300000"),
                UpdatedOn = DateTime.Parse("2024-08-26 11:17:03.9300000")
            }
        );
        }
    }
}
