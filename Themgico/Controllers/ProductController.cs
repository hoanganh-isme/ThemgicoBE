using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return StatusCode(result._statusCode, result);
        }
    }
}
