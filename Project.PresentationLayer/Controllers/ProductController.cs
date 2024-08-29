using AutoMapper;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using OpenQA.Selenium;
using Project.BusinessDomainLayer.VMs;
using System.ComponentModel.DataAnnotations;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly JwtService _jwtService;
        private readonly ILogger<CustomerController> _logger;

        public ProductController(IProductService productService, JwtService jwtService, ILogger<CustomerController> logger)
        {
            _productService = productService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpGet("getallproducts/{id}")]
        public async Task<IActionResult> GetAllProducts(Guid id, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync(pageNumber, id);
                return Ok(products);
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(new { Message = e.Message });
            }
            catch (ArgumentException e) {
                _logger.LogError(e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("getproduct/{id}/{customerId}")]
        public async Task<IActionResult> GetProductById(Guid id, Guid customerId)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id, customerId);
                return Ok(product);
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(new { Message = e.Message });
            }
        }

        [HttpPost("addproduct/{id}")]
        public async Task<IActionResult> AddProduct(Guid id,[FromBody][Required] NewProductVM newProduct)
        {
            try
            {
                await _productService.CreateProductAsync(newProduct, id);
                return Ok(new { Message = "Product Added Successfully" });
            }
            catch (AccessViolationException e)
            {
                _logger.LogWarning(e.Message);
                return Unauthorized(new { Message = e.Message });
            }
            catch (ArgumentException e)
            {
                _logger.LogWarning(e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }


        [HttpPut("updateproduct/{id}/{customerId}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Guid customerId, [FromBody][Required] EditProductVM updatedProduct)
        {
            try
            {
                await _productService.UpdateProductAsync(updatedProduct, customerId, id);
                return Ok(new { Message = "Updated Successfully" });
            }
            catch (AccessViolationException e)
            {
                _logger.LogWarning(e.Message);
                return Unauthorized(new { Message = e.Message });
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(new { Message = e.Message });
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning(e.Message);
                return BadRequest(new { Message = e.Message });
            }
        }


        [HttpDelete("deleteproduct/{id}/{customerId}")]
        public async Task<IActionResult> DeleteProduct(Guid id, Guid customerId)
        {
            try
            {
                await _productService.DeleteProductAsync(id, customerId);
                return Ok("Product Deleted Successfully");
            }
            catch (AccessViolationException e)
            {
                _logger.LogWarning(e.Message);
                return Unauthorized(new { Message = e.Message });
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(new { Message = e.Message });
            }
        }

    }
}



//    var isAdminClaim = User.FindFirst("IsAdmin")?.Value;
//    if (isAdminClaim != null && bool.TryParse(isAdminClaim, out bool isAdmin) && isAdmin)
//    {
//    }
//    else
//    {
//        return Forbid("You do not have permission to add products");
//    }


//[HttpPatch("updateproduct/{id}")]
//[Authorize]
//public async Task<IActionResult> PatchProduct(Guid id, [FromBody] JsonPatchDocument<ProductDTO> patchDoc)
//{
//    if (patchDoc == null)
//    {
//        return BadRequest("Patch document is null");
//    }

//    var isAdminClaim = User.FindFirst("IsAdmin")?.Value;

//    if (isAdminClaim != null && bool.TryParse(isAdminClaim, out bool isAdmin) && isAdmin)
//    {
//        var existingProduct = await _productService.GetProductByIdAsync(id);
//        if (existingProduct == null)
//        {
//            return NotFound("Product not found");
//        }

//        var productToPatch = _mapper.Map<ProductDTO>(existingProduct);
//        patchDoc.ApplyTo(productToPatch);

//        if (!TryValidateModel(productToPatch))
//        {
//            return ValidationProblem(ModelState);
//        }

//        _mapper.Map(productToPatch, existingProduct);
//        existingProduct.UpdatedOn = DateTime.UtcNow;

//        await _productService.UpdateProductAsync(existingProduct);

//        return Ok("Product updated successfully");
//    }
//    else
//    {
//        return Forbid("You do not have permission to update products");
//    }
//}