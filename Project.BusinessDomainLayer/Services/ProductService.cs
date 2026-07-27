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
            ApplyCoverImage(newProduct.CoverImageUrl, newProduct.ProductImages);

            var product = _mapper.Map<Product>(newProduct);

            product.Cost = Math.Round(product.Cost, 2);
            await _productRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        // The API takes the cover url as its own field; it is stored as an ordinary
        // image row flagged IsCover so the gallery stays a single collection and
        // listings can still pull the cover out cheaply.
        private static void ApplyCoverImage(string? coverImageUrl, List<ProductImageDTO> images)
        {
            foreach (var image in images)
            {
                image.IsCover = false;
            }

            if (string.IsNullOrWhiteSpace(coverImageUrl))
            {
                // No explicit cover — promote the first gallery image so listings
                // still have something to show.
                if (images.Count > 0)
                {
                    images[0].IsCover = true;
                }
                return;
            }

            var existing = images.FirstOrDefault(i => i.Url == coverImageUrl);
            if (existing is not null)
            {
                existing.IsCover = true;
            }
            else
            {
                images.Insert(0, new ProductImageDTO { Url = coverImageUrl, IsCover = true });
            }
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

            // Images are mapped separately: overwriting the tracked collection
            // would orphan the existing rows.
            ApplyCoverImage(updatedProduct.CoverImageUrl, updatedProduct.ProductImages);
            var images = _mapper.Map<List<ProductImage>>(updatedProduct.ProductImages);
            await _productRepository.ReplaceImagesAsync(productId, images);

            await _unitOfWork.CompleteAsync();

            // Re-read so the response carries the images that were just written.
            var saved = await _productRepository.GetByIdAsync(productId);
            return _mapper.Map<ProductDTO>(saved ?? resultProduct);
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

        public async Task<(IEnumerable<ProductDTO> Items, int TotalCount)> GetAllProductsAsync(int pageNumber, int pageSize, Guid? customerId, string? type = null)
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
                products = await _productRepository.GetAllPagedAsAdminAsync(pageNumber, pageSize, type) ?? throw new ProductNotFoundException("No Products Found");
            }
            else
            {
                products = await _productRepository.GetAllPagedAsync(pageNumber, pageSize, type) ?? throw new ProductNotFoundException("No Products Found");
            }

            // Admins see soft-deleted products, so the count must match that scope.
            var totalCount = await _productRepository.GetProductsCountAsync(type, includeDeleted: isAdmin);

            var items = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return (items, totalCount);
        }


        // ── Home-page storefront sections ──────────────────────────────────

        private const int DefaultSectionSize = 10;
        private const int LastPiecesMaxStock = 10;

        private static int NormalizeCount(int count) =>
            count <= 0 || count > MaxPageSize ? DefaultSectionSize : count;

        public async Task<IEnumerable<ProductDTO>> GetBestSellersAsync(int count)
        {
            var products = await _productRepository.GetBestSellersAsync(NormalizeCount(count));
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetNewArrivalsAsync(int count)
        {
            var products = await _productRepository.GetNewArrivalsAsync(NormalizeCount(count));
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetLastPiecesAsync(int count)
        {
            var products = await _productRepository.GetLastPiecesAsync(NormalizeCount(count), LastPiecesMaxStock);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _productRepository.GetCategoriesAsync();
            return categories.Select(c => new CategoryDTO
            {
                Type = c.Type,
                Count = c.Count,
                SampleImageUrl = c.SampleImageUrl
            });
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
