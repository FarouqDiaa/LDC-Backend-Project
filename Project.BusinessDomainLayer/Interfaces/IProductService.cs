using Project.BusinessDomainLayer.DTOs;

namespace Project.BusinessDomainLayer.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(Guid id);
        Task CreateProductAsync(NewProductDTO newProductDTO);
    }
}
