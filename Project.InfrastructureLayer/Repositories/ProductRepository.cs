using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;


namespace Project.InfrastructureLayer.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Set<Product>().FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Set<Product>().AddAsync(product);
            await _context.SaveChangesAsync();
        }
    }
}
