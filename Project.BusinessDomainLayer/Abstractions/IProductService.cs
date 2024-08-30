using Microsoft.AspNetCore.JsonPatch;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.VMs;

namespace Project.BusinessDomainLayer.Abstractions
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(Guid id, Guid customerId);
        Task CreateProductAsync(NewProductDTO newProduct, Guid CustomerId);
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber, Guid customerId);
        Task DeleteProductAsync(Guid id, Guid customerId);
        Task UpdateProductAsync(UpdateProductDTO updatedProduct, Guid customerId, Guid productId);
        Task<ProductDTO> GetProductByNameAsync(string name);
    }
}
