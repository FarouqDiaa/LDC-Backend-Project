using Project.InfrastructureLayer.Entities;

namespace Project.InfrastructureLayer.Abstractions
{
    public interface IProductRepository
    {
        public Task<Product> GetByIdAsync(Guid id);
        public Task<Product> GetByNameAsync(string name);
        public Task AddAsync(Product product);
        public Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize, string? type = null);
        public Task<IEnumerable<Product>> GetAllPagedAsAdminAsync(int pageNumber, int pageSize, string? type = null);
        public Task<int> GetProductsCountAsync(string? type = null, bool includeDeleted = false);
        public Task ReplaceImagesAsync(Guid productId, IEnumerable<ProductImage> images);

        public Task<IEnumerable<Product>> GetBestSellersAsync(int count);
        public Task<IEnumerable<Product>> GetNewArrivalsAsync(int count);
        public Task<IEnumerable<Product>> GetLastPiecesAsync(int count, int maxStock);
        public Task<IEnumerable<(string Type, int Count, string? SampleImageUrl)>> GetCategoriesAsync();
        public Task RemoveByIdAsync(Guid id);
        public Task Update(Product product);

        public Task<bool> IsProductExistsByIdAsync(Guid id);
        public Task<bool> IsProductExistsAsync(string name);

    }
}
