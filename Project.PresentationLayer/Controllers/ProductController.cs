using AutoMapper;
using Project.BusinessDomainLayer.Interfaces;
using Project.BusinessDomainLayer.DTOs;
using Project.BusinessDomainLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using OpenQA.Selenium;
using Project.BusinessDomainLayer.VMs;

namespace Project.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly ILogger<CustomerController> _logger;

        public ProductController(IProductService productService, IMapper mapper, JwtService jwtService, ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _productService = productService;
            _mapper = mapper;
            _jwtService = jwtService;
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet("getallproducts/{id}")]
        public async Task<IActionResult> GetAllProducts(Guid id, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync(pageNumber, id);
                return Ok(products);
            }
            catch(NotFoundException e) {
                return NotFound(e);
            }
        }

        [HttpGet("getproduct/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (NotFoundException e) {
                return NotFound(e);
            }
        }

        [HttpPost("addproduct/{id}")]
        public async Task<IActionResult> AddProduct(Guid id, NewProductVM newProduct)
        {
            try
            {
                await _productService.CreateProductAsync(newProduct, id);
                return Ok("Product Added Successfully");
            }
            catch (AccessViolationException e)
            {
                return Unauthorized(e);
            }
            catch (Exception e) { 
            return BadRequest(e);
            }
        }


        [HttpPatch("updateproduct/{id}/{customerId}")]
        public async Task<IActionResult> PatchProduct(Guid id,Guid customerId ,[FromBody] JsonPatchDocument<ProductDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is null");
            }
            try
            {
                await _productService.UpdateProductAsync(patchDoc, customerId, id);
                return Ok("Updated Successfully");
            }
            catch (AccessViolationException e)
            {
                return Unauthorized(e);
            }
            catch (Exception e) 
            {
                return BadRequest(e);
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
                return Unauthorized(e);
            }
            catch (KeyNotFoundException e) {
                return NotFound(e);
            }
        }

    }
}




//[
//{ "op": "replace", "path": "/ProductName", "value": "New Product Name" },
//{ "op": "replace", "path": "/Price", "value": 99.99 }
//]

//[HttpPost("addproduct/{id}")]
//[Authorize]
//public async Task<IActionResult> AddProduct(Guid id, NewProductDTO newProduct)
//{
//    var isAdminClaim = User.FindFirst("IsAdmin")?.Value;

//    if (isAdminClaim != null && bool.TryParse(isAdminClaim, out bool isAdmin) && isAdmin)
//    {
//        await _productService.CreateProductAsync(newProduct);
//        return Ok("Product added successfully");
//    }
//    else
//    {
//        return Forbid("You do not have permission to add products");
//    }
//}


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


//[HttpDelete("deleteproduct/{id}")]
//[Authorize]
//public async Task<IActionResult> DeleteProduct(Guid id)
//{
//    var isAdminClaim = User.FindFirst("IsAdmin")?.Value;

//    if (isAdminClaim != null && bool.TryParse(isAdminClaim, out bool isAdmin) && isAdmin)
//    {
//        var existingProduct = await _productService.GetProductByIdAsync(id);
//        if (existingProduct == null)
//        {
//            return NotFound("Product not found");
//        }

//        await _productService.DeleteProductAsync(id);

//        return Ok("Product deleted successfully");
//    }
//    else
//    {
//        return Forbid("You do not have permission to delete products");
//    }
//}