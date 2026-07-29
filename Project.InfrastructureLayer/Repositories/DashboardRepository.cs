using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project.InfrastructureLayer.Abstractions;

namespace Project.InfrastructureLayer.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public DashboardRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => !o.IsDeleted)
                .SumAsync(o => (double?)o.TotalAmount) ?? 0d;
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.AsNoTracking().CountAsync(o => !o.IsDeleted);
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _context.Customers.AsNoTracking().CountAsync(c => !c.IsDeleted && !c.IsAdmin);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.AsNoTracking().CountAsync(p => !p.IsDeleted);
        }

        public async Task<IEnumerable<(DateTime Date, double Revenue, int Orders)>> GetSalesOverTimeAsync(int days)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-(days - 1));

            // Aggregate per calendar day in the database; fill missing days below
            // so the chart always has a continuous x-axis.
            var grouped = await _context.Orders
                .AsNoTracking()
                .Where(o => !o.IsDeleted && o.CreatedOn >= startDate)
                .GroupBy(o => o.CreatedOn.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount),
                    Orders = g.Count()
                })
                .ToListAsync();

            var byDate = grouped.ToDictionary(x => x.Date, x => (x.Revenue, x.Orders));

            var result = new List<(DateTime, double, int)>();
            for (var i = 0; i < days; i++)
            {
                var day = startDate.AddDays(i);
                if (byDate.TryGetValue(day, out var v))
                {
                    result.Add((day, v.Revenue, v.Orders));
                }
                else
                {
                    result.Add((day, 0d, 0));
                }
            }

            return result;
        }

        public async Task<IEnumerable<(string Type, double Revenue, int UnitsSold)>> GetSalesByCategoryAsync()
        {
            var rows = await _context.OrderItems
                .AsNoTracking()
                .Where(oi => !oi.Order.IsDeleted)
                .GroupBy(oi => oi.Product.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    Revenue = g.Sum(oi => oi.Cost),
                    UnitsSold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.Revenue)
                .ToListAsync();

            return rows.Select(x => (x.Type, x.Revenue, x.UnitsSold)).ToList();
        }

        public async Task<IEnumerable<(Guid Id, string Name, string? Type, int UnitsSold, double Revenue, string? ImageUrl)>> GetTopProductsAsync(int count)
        {
            var sales = await _context.OrderItems
                .AsNoTracking()
                .Where(oi => !oi.Order.IsDeleted)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    UnitsSold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Cost)
                })
                .OrderByDescending(x => x.UnitsSold)
                .Take(count)
                .ToListAsync();

            var ids = sales.Select(s => s.ProductId).ToList();

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Type,
                    ImageUrl = p.ProductImages
                        .OrderByDescending(pi => pi.IsCover)
                        .Select(pi => pi.Url)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var meta = products.ToDictionary(p => p.Id);

            return sales
                .Where(s => meta.ContainsKey(s.ProductId))
                .Select(s =>
                {
                    var p = meta[s.ProductId];
                    return (p.Id, p.Name, (string?)p.Type, s.UnitsSold, s.Revenue, (string?)p.ImageUrl);
                })
                .ToList();
        }

        public async Task<IEnumerable<(Guid Id, string Name, string? Type, int StockQuantity, string? ImageUrl)>> GetLowStockProductsAsync(int threshold, int count)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.StockQuantity <= threshold)
                .OrderBy(p => p.StockQuantity)
                .Take(count)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Type,
                    p.StockQuantity,
                    ImageUrl = p.ProductImages
                        .OrderByDescending(pi => pi.IsCover)
                        .Select(pi => pi.Url)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return products
                .Select(p => (p.Id, p.Name, (string?)p.Type, p.StockQuantity, (string?)p.ImageUrl))
                .ToList();
        }
    }
}
