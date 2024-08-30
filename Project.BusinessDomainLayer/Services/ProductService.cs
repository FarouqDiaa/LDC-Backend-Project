using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Abstractions;
using Project.BusinessDomainLayer.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using OpenQA.Selenium;
using Project.BusinessDomainLayer.VMs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Project.BusinessDomainLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache, IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id, Guid customerId)
        {
            bool isAdmin = await _customerRepository.IsAdmin(customerId);
            var product = await _productRepository.GetByIdAsync(id);
            if (product.IsDeleted == true && isAdmin != true) {
                product = null;
            }
            return product == null ? throw new NotFoundException("Product not found") : _mapper.Map<ProductDTO>(product);
        }

        public async Task CreateProductAsync(NewProductDTO newProduct, Guid customerId)
        {
            bool isAdmin = await _customerRepository.IsAdmin(customerId);
            if (!isAdmin) {
                throw new AccessViolationException("UnAuthorized");
            }

            var existingProduct = await _productRepository.GetByNameAsync(newProduct.Name);
            if (existingProduct != null)
            {
                throw new ArgumentException("Product name used");
            }
            var product = _mapper.Map<Product>(newProduct);
            if (product.StockQuantity == 0) {
                product.Status = "OutOfStock";
            }
            product.Status = "InStock";
            await _productRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ProductDTO> GetProductByNameAsync(string name) {
            var product = await _productRepository.GetByNameAsync(name);
            return product == null ? throw new NotFoundException("Product not found") : _mapper.Map<ProductDTO>(product);
        }


        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int pageNumber, Guid customerId)
        {
            bool isAdmin = await _customerRepository.IsAdmin(customerId);
            int pageCount = 25;
            if (pageNumber <= 0) {
                throw new ArgumentException("Page number is invalid");
            }
            IEnumerable<Product> products;
            if (isAdmin)
            {
                products = await _productRepository.GetAllPagedAsAdminAsync(pageNumber, pageCount) ?? throw new NotFoundException("No Products Found");
            }
            else
            {
                products = await _productRepository.GetAllPagedAsync(pageNumber, pageCount) ?? throw new NotFoundException("No Products Found");
            }
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task UpdateProductAsync(UpdateProductDTO updatedProduct, Guid customerId, Guid productId)
        {

            bool isAdmin = await _customerRepository.IsAdmin(customerId);
            if (!isAdmin)
            {
                throw new AccessViolationException("Unauthorized");
            }

            var existingProduct = await _productRepository.GetByIdAsync(productId)
                                  ?? throw new KeyNotFoundException("Product not found");

            if (updatedProduct.Name != null && updatedProduct.Name != existingProduct.Name)
            {
                var oldProduct = await _productRepository.GetByNameAsync(updatedProduct.Name);
                if (oldProduct != null)
                {
                    throw new InvalidOperationException($"The product name '{updatedProduct.Name}' is already used");
                }
            }

            string newStatus = existingProduct.Status;
            if (updatedProduct.StockQuantity != existingProduct.StockQuantity)
            {
                if (updatedProduct.StockQuantity > 0)
                {
                    newStatus = "InStock";
                }
                else
                {
                    newStatus = "OutOfStock";
                }
            }

            var resultProduct = _mapper.Map(updatedProduct, existingProduct);
            resultProduct.Status = newStatus;

            _productRepository.Update(resultProduct);
            await _unitOfWork.CompleteAsync();
        }


        public async Task DeleteProductAsync(Guid id,Guid customerId)
        {
            bool isAdmin = await _customerRepository.IsAdmin(customerId);
            if (!isAdmin)
            {
                throw new AccessViolationException("UnAuthorized");
            }
            var product = await _productRepository.IsProductExistsAsync(id);
            if (product)
            {
                await _productRepository.RemoveByIdAsync(id);
                await _unitOfWork.CompleteAsync();
            }
            else
            { 
             throw new KeyNotFoundException("Product not found");
            }
        }


    }
}
