using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Abstractions;
using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.Exceptions.ProductExceptions;
using Project.BusinessDomainLayer.Exceptions.CustomerExceptions;
using Project.BusinessDomainLayer.VMs;

namespace Project.BusinessDomainLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }


        public async Task <ProductDTO> CreateProductAsync(NewProductDTO newProduct)
        {
            var existingProduct = await _productRepository.IsProductExistsAsync(newProduct.Name);
            if (existingProduct)
            {
                throw new ProductNameUsedException("Product name used");
            }
            var product = _mapper.Map<Product>(newProduct);

            product.Cost = Math.Round(product.Cost, 2);
            await _productRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> GetProductByNameAsync(string name) {
            var product = await _productRepository.GetByNameAsync(name);
            return product == null ? throw new ProductNotFoundException("Product not found") : _mapper.Map<ProductDTO>(product);
        }


        public async Task<ProductDTO> UpdateProductAsync(UpdateProductDTO updatedProduct, Guid productId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId)
                                  ?? throw new ProductNotFoundException("Product not found");

            if (updatedProduct.Name != existingProduct.Name)
            {
                var oldProduct = await _productRepository.IsProductExistsAsync(updatedProduct.Name);
                if (oldProduct)
                {
                    throw new ProductNameUsedException($"The product name '{updatedProduct.Name}' is already used");
                }
            }

            var resultProduct = _mapper.Map(updatedProduct, existingProduct);

            resultProduct.Cost = Math.Round(resultProduct.Cost, 2);
            await _productRepository.Update(resultProduct);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDTO>(resultProduct);
        }


        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.IsProductExistsByIdAsync(id);
            if (product)
            {
                await _productRepository.RemoveByIdAsync(id);
                await _unitOfWork.CompleteAsync();
            }
            else
            { 
             throw new InvalidProductIdException("InValid Product Id");
            }
        }


        private const int DefaultPageSize = 25;
        private const int MaxPageSize = 100;

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber, int pageSize, Guid? customerId)
        {
            bool isAdmin = false;
            if (customerId is not null)
            {
                bool exists = await _customerRepository.IsCustomerExistsByIdAsync(customerId.Value);
                if (!exists)
                {
                    throw new InvalidCustomerIdException("InValid Customer Id");
                }
                isAdmin = await _customerRepository.IsAdmin(customerId.Value);
            }

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            if (pageSize <= 0 || pageSize > MaxPageSize)
            {
                pageSize = DefaultPageSize;
            }

            IEnumerable<Product> products;
            if (isAdmin)
            {
                products = await _productRepository.GetAllPagedAsAdminAsync(pageNumber, pageSize) ?? throw new ProductNotFoundException("No Products Found");
            }
            else
            {
                products = await _productRepository.GetAllPagedAsync(pageNumber, pageSize) ?? throw new ProductNotFoundException("No Products Found");
            }
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }


        public async Task<ProductDTO> GetProductByIdAsync(Guid id, Guid? customerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) throw new ProductNotFoundException("Product not found");

            if (product.IsDeleted)
            {
                bool isAdmin = customerId is not null && await _customerRepository.IsAdmin(customerId.Value);
                if (!isAdmin) throw new ProductNotFoundException("Product not found");
            }

            return _mapper.Map<ProductDTO>(product);
        }


    }
}
