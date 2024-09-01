using Microsoft.AspNetCore.JsonPatch;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;

namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(Guid id);
        Task<ProductDTO> CreateProductAsync(NewProductDTO newProduct);
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber, Guid customerId);
        Task DeleteProductAsync(Guid id);
        Task<ProductDTO> UpdateProductAsync(UpdateProductDTO updatedProduct, Guid productId);
        Task<ProductDTO> GetProductByNameAsync(string name);
    }
}
