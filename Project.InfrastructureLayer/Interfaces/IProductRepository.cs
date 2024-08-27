using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> GetByIdAsync(Guid id);
        public Task<Product> GetByNameAsync(string name);
        public Task AddAsync(Product product);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize, bool isAdmin);
        public Task RemoveByIdAsync(Guid id);
        public void Update(Product product);

        public Task<bool> ProductExistsAsync(Guid id);

    }
}
