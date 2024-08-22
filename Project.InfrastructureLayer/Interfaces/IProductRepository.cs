using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> GetByIdAsync(Guid id);
        public Task AddAsync(Product product);
    }
}
