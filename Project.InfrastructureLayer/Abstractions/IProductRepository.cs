using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Abstractions
{
    public interface IProductRepository
    {
        public Task<Product> GetByIdAsync(Guid id);
        public Task<Product> GetByNameAsync(string name);
        public Task AddAsync(Product product);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize);
        public Task<IEnumerable<Product>> GetAllPagedAsAdminAsync(int pageNumber, int pageSize);
        public Task RemoveByIdAsync(Guid id);
        public void Update(Product product);

        public Task<bool> IsProductExistsByIdAsync(Guid id);
        public Task<bool> IsProductExistsAsync(string name);

    }
}
