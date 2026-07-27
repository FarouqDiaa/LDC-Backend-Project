using Microsoft.AspNetCore.JsonPatch;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;

namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(Guid id, Guid? customerId);
        Task<ProductDTO> CreateProductAsync(NewProductDTO newProduct);
        Task<(IEnumerable<ProductDTO> Items, int TotalCount)> GetAllProductsAsync(int pageNumber, int pageSize, Guid? customerId, string? type = null);
        Task<IEnumerable<ProductDTO>> GetBestSellersAsync(int count);
        Task<IEnumerable<ProductDTO>> GetNewArrivalsAsync(int count);
        Task<IEnumerable<ProductDTO>> GetLastPiecesAsync(int count);
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
        Task DeleteProductAsync(Guid id);
        Task<ProductDTO> UpdateProductAsync(UpdateProductDTO updatedProduct, Guid productId);
        Task<ProductDTO> GetProductByNameAsync(string name);
    }
}
