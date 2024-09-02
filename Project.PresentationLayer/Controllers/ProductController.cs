using AutoMapper;
using Project.BusinessDomainLayer.Abstractions;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.BusinessDomainLayer.Responses;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        //private readonly IJwtService _jwtService;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, /*JwtService jwtService,*/ ILogger<ProductController> logger, IMapper mapper)
        {
            _productService = productService;
            //_jwtService = jwtService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("addproduct")]
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


        [HttpPut("updateproduct/{id}")]
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



        [HttpDelete("deleteproduct/{id}")]
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

        [HttpGet("getallproducts/{customerId}")]
        public async Task<IActionResult> GetAllProducts(Guid customerId, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync(pageNumber, customerId);
                var productsRes = _mapper.Map<IEnumerable<ProductResVM>>(products);
                var successResponse = new SuccessResponse<IEnumerable<ProductResVM>>
                {
                    StatusCode = 200,
                    Message = "Products Retrieved Successfully",
                    Data = productsRes
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

        [HttpGet("getproductbyid/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
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