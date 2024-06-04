using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Themgico.DTO.Category;
using Themgico.Service.Interface;

namespace Themgico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategory();
            return StatusCode(result._statusCode, result);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryById(id);
            return StatusCode(result._statusCode, result);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            var result = await _categoryService.CreateCategory(categoryDTO);
            return StatusCode(result._statusCode, result);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            return StatusCode(result._statusCode, result);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDTO categoryDTO)
        {
            var result = await _categoryService.UpdateCategory(categoryDTO);
            return StatusCode(result._statusCode, result);
        }
    }
}
