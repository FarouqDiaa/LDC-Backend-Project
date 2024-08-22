using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;
using Project.BusinessDomainLayer.Interfaces;

namespace Project.BusinessDomainLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return null;

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task CreateProductAsync(NewProductDTO newProductDto)
        {
            var product = _mapper.Map<Product>(newProductDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
        }
    }
}
