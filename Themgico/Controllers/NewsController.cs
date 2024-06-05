using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Themgico.DTO.News;
using Themgico.Entities;
using Themgico.Service.Interface;

namespace Themgico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ThemgicoContext _context;
        private readonly INewsService _newsService;
        public NewsController(ThemgicoContext context, INewsService newsService)
        {
            _context = context;
            _newsService = newsService;
        }
        [HttpGet("GetAllNews")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _newsService.GetAllNews();
            return StatusCode(result._statusCode, result);
        }
        [HttpGet("GetNewsById/{id}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var result = await _newsService.GetNewsById(id);
            return StatusCode(result._statusCode, result);
        }

        [HttpPost("CreateNews")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateNews(NewsDTO newsDTO)
        {
            var result = await _newsService.CreateNews(newsDTO);
            return StatusCode(result._statusCode, result);
        }

        [HttpDelete("DeleteNews/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var result = await _newsService.DeleteNews(id);
            return StatusCode(result._statusCode, result);
        }

        [HttpPut("UpdateNews")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateNews(NewsDTO newsDTO)
        {
            var result = await _newsService.UpdateNews(newsDTO);
            return StatusCode(result._statusCode, result);
        }
    }
}
