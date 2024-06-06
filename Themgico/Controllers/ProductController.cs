using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Themgico.DTO.Product;
using Themgico.Entities;
using Themgico.Service;
using Themgico.Service.Interface;

namespace Themgico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ThemgicoContext _context;
        private readonly IProductService _productService;
        public ProductController(ThemgicoContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return StatusCode(result._statusCode, result);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductById(id);
            return StatusCode(result._statusCode, result);
        }
        [Authorize(Roles = "admin")]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDTO)
        {
            var result = await _productService.CreateProduct(productDTO);
            return StatusCode(result._statusCode, result);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO productDTO)
        {
            var result = await _productService.UpdateProduct(productDTO);
            return StatusCode(result._statusCode, result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            return StatusCode(result._statusCode, result);
        }
        [HttpPut("updatestatus/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUserStatus(int id)
        {
            var result = await _productService.UpdateProductStatus(id);
            return StatusCode(result._statusCode, result);
        }
    }
}
