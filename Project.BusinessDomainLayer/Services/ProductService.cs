using AutoMapper;
using Project.BusinessDomainLayer.DTOs;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;
using Project.BusinessDomainLayer.Interfaces;
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

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _productRepository = productRepository;
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? throw new NotFoundException("Product not found") : _mapper.Map<ProductDTO>(product);
        }

        public async Task CreateProductAsync(NewProductVM newProduct, Guid customerId)
        {
            bool isAdmin = await _customerRepository.IsAdmin(customerId);
            if (!isAdmin) {
                throw new AccessViolationException("UnAuthorized");
            }

            var existingProduct = _productRepository.GetByNameAsync(newProduct.Name);
            if (existingProduct != null)
            {
                throw new Exception("Product name used");
            }
            var product = _mapper.Map<Product>(newProduct);
            if (product.StockQuantity == 0) {
                product.Status = "OutOfStock";
            }
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
            var products = await _productRepository.GetAllPagedAsync(pageNumber, pageCount, isAdmin) ?? throw new NotFoundException("No Products Found");
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }


        public async Task UpdateProductAsync(JsonPatchDocument<ProductDTO> patchDoc, Guid customerId, Guid productId)
        {
            bool isAdmin = await _customerRepository.IsAdmin(customerId);
            if (!isAdmin)
            {
                throw new AccessViolationException("UnAuthorized");
            }
            var existingProduct = await _productRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Product not found");
            var newProduct = _mapper.Map<ProductDTO>(existingProduct);

            patchDoc.ApplyTo(newProduct);
            var validationContext = new ValidationContext(newProduct);
            Validator.ValidateObject(newProduct, validationContext, validateAllProperties: true);


            if (newProduct.Name != existingProduct.Name)
            {
                var oldProduct = await _productRepository.GetByNameAsync(newProduct.Name);
                if (oldProduct != null)
                {
                    throw new Exception("Name already used");
                }
            }

            if (newProduct.StockQuantity != 0 && existingProduct.StockQuantity == 0)
            {
                newProduct.Status = "InStock";
            }
            if (newProduct.StockQuantity == 0 && existingProduct.StockQuantity != 0)
            {
                newProduct.Status = "OutOfStock";
            }

            var resultProduct = _mapper.Map<Product>(newProduct);

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
            var product = await _productRepository.ProductExistsAsync(id);
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
