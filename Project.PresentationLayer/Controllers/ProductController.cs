using AutoMapper;
using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Project.BusinessDomainLayer.VMs;
using Project.BusinessDomainLayer.VMs.ProductVMs;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.BusinessDomainLayer.Responses;
using Project.PresentationLayer.Extensions;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IMapper mapper)
        {
            _productService = productService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody][Required] ProductVM productVM)
        {
            try
            {
                var newProductDTO = _mapper.Map<NewProductDTO>(productVM);
                var productDTO = await _productService.CreateProductAsync(newProductDTO);
                var ProductResVM = _mapper.Map<ProductResVM>(productDTO);
                var successResponse = new SuccessResponse<ProductResVM>
                {
                    StatusCode = 200,
                    Message = "Product Added Successfully",
                    Data = ProductResVM
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Add Product"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Add Product"
                };
                return BadRequest(errorResponse);
            }
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody][Required] ProductVM productVM)
        {
            try
            {
                var updateProductDTO = _mapper.Map<UpdateProductDTO>(productVM);
                var productDTO = await _productService.UpdateProductAsync(updateProductDTO, id);
                var newProductVM = _mapper.Map<ProductResVM>(productDTO);
                var successResponse = new SuccessResponse<ProductResVM>
                {
                    StatusCode = 200,
                    Message = "Product Updated Successfully",
                    Data = newProductVM
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Update Product"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Update Product"
                };
                return BadRequest(errorResponse);
            }
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                var successResponse = new BaseResponse
                {
                    StatusCode = 200,
                    Message = "Product Deleted Successfully"
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Delete Product"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Delete Product"
                };
                return BadRequest(errorResponse);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? type = null)
        {
            var customerId = User.GetCustomerId();

            try
            {
                var (products, totalCount) = await _productService.GetAllProductsAsync(pageNumber, pageSize, customerId, type);
                var productsRes = _mapper.Map<IEnumerable<ProductResVM>>(products);
                var successResponse = new SuccessResponse<PagedResultVM<ProductResVM>>
                {
                    StatusCode = 200,
                    Message = "Products Retrieved Successfully",
                    Data = new PagedResultVM<ProductResVM>
                    {
                        Items = productsRes,
                        TotalCount = totalCount,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Retrieve Products"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Retrieve Products"
                };
                return BadRequest(errorResponse);
            }
        }

        // ── Home-page storefront sections (public) ─────────────────────────

        [AllowAnonymous]
        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetBestSellers([FromQuery] int count = 10)
        {
            var products = await _productService.GetBestSellersAsync(count);
            var productsRes = _mapper.Map<IEnumerable<ProductResVM>>(products);
            return Ok(new SuccessResponse<IEnumerable<ProductResVM>>
            {
                StatusCode = 200,
                Message = "Best Sellers Retrieved Successfully",
                Data = productsRes
            });
        }

        [AllowAnonymous]
        [HttpGet("new-arrivals")]
        public async Task<IActionResult> GetNewArrivals([FromQuery] int count = 10)
        {
            var products = await _productService.GetNewArrivalsAsync(count);
            var productsRes = _mapper.Map<IEnumerable<ProductResVM>>(products);
            return Ok(new SuccessResponse<IEnumerable<ProductResVM>>
            {
                StatusCode = 200,
                Message = "New Arrivals Retrieved Successfully",
                Data = productsRes
            });
        }

        [AllowAnonymous]
        [HttpGet("last-pieces")]
        public async Task<IActionResult> GetLastPieces([FromQuery] int count = 10)
        {
            var products = await _productService.GetLastPiecesAsync(count);
            var productsRes = _mapper.Map<IEnumerable<ProductResVM>>(products);
            return Ok(new SuccessResponse<IEnumerable<ProductResVM>>
            {
                StatusCode = 200,
                Message = "Last Pieces Retrieved Successfully",
                Data = productsRes
            });
        }

        [AllowAnonymous]
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            var categoriesRes = _mapper.Map<IEnumerable<CategoryResVM>>(categories);
            return Ok(new SuccessResponse<IEnumerable<CategoryResVM>>
            {
                StatusCode = 200,
                Message = "Categories Retrieved Successfully",
                Data = categoriesRes
            });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var customerId = User.GetCustomerId();

            try
            {
                var product = await _productService.GetProductByIdAsync(id, customerId);
                var newProductVM = _mapper.Map<ProductResVM>(product);
                var successResponse = new SuccessResponse<ProductResVM>
                {
                    StatusCode = 200,
                    Message = "Product Retrieved Successfully",
                    Data = newProductVM
                };
                return Ok(successResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Retrieve Product"
                };
                return BadRequest(errorResponse);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL exception caught in controller");

                var errorResponse = new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Can’t Retrieve Product"
                };
                return BadRequest(errorResponse);
            }
        }


    }
}