using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.BusinessDomainLayer.Abstractions;
using Project.InfrastructureLayer;
using Project.InfrastructureLayer.Entities;

namespace Project.RuntimeLayer.Seeding
{
    public static class DataSeeder
    {
        private const int CustomersToSeed = 10;
        private const int OrdersToSeed = 10;

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var encryption = scope.ServiceProvider.GetRequiredService<IEncryption>();

            var customers = await SeedCustomersAsync(context, encryption);
            // Products are no longer seeded — order seeding uses whatever
            // products already exist in the database.
            var products = await context.Products
                                        .Where(p => !p.IsDeleted)
                                        .ToListAsync();
            await SeedOrdersAsync(context, customers, products);
        }

        private static async Task<List<Customer>> SeedCustomersAsync(ApplicationDbContext context, IEncryption encryption)
        {
            var existingCustomers = await context.Customers.Where(c => !c.IsAdmin).ToListAsync();
            if (existingCustomers.Count >= CustomersToSeed) return existingCustomers;

            var salt = encryption.GenerateSaltedPassword();
            var hash = encryption.GenerateEncryptedPassword(salt, "Aa@12345678");

            var customerFaker = new Faker<Customer>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Name.FirstName() + f.Name.LastName())
                .RuleFor(c => c.Email, (f, c) => $"{f.Internet.UserName(c.Name)}{f.IndexFaker}@example.com")
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.Phone, f => "+2011" + f.Random.Number(10000000, 99999999))
                .RuleFor(c => c.Status, f => f.PickRandom("Active", "InActive"))
                .RuleFor(c => c.PasswordSalt, f => salt)
                .RuleFor(c => c.PasswordHash, f => hash)
                .RuleFor(c => c.IsAdmin, f => false)
                .RuleFor(c => c.CreatedOn, f => DateTime.UtcNow)
                .RuleFor(c => c.UpdatedOn, f => DateTime.UtcNow);

            var newCustomers = customerFaker.Generate(CustomersToSeed - existingCustomers.Count);
            await context.Customers.AddRangeAsync(newCustomers);
            await context.SaveChangesAsync();

            existingCustomers.AddRange(newCustomers);
            return existingCustomers;
        }

        private static async Task SeedOrdersAsync(ApplicationDbContext context, List<Customer> customers, List<Product> products)
        {
            if (customers.Count == 0 || products.Count == 0) return;
            if (await context.Orders.CountAsync() >= OrdersToSeed) return;

            var random = new Random();
            var orders = new List<Order>();
            var orderItems = new List<OrderItem>();

            foreach (var customer in customers.Take(OrdersToSeed))
            {
                var itemCount = random.Next(1, 4);
                var chosenProducts = products.OrderBy(_ => random.Next()).Take(itemCount).ToList();

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };

                double amount = 0;
                foreach (var product in chosenProducts)
                {
                    var quantity = random.Next(1, 5);
                    var cost = Math.Round(product.Cost * quantity, 2);
                    amount += cost;

                    orderItems.Add(new OrderItem
                    {
                        OrderitemId = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = quantity,
                        Cost = cost
                    });
                }

                var tax = (float)Math.Round(amount * 0.14, 2);
                order.Amount = amount;
                order.Tax = tax;
                order.TotalAmount = amount + tax;

                orders.Add(order);
            }

            await context.Orders.AddRangeAsync(orders);
            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();
        }
    }
}
