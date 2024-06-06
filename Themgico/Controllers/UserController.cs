using Cursus.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Themgico.DTO;
using Themgico.DTO.Account;
using Themgico.DTO.Authorization;
using Themgico.Entities;
using Themgico.Properties;
using Themgico.Service.Interface;

namespace Themgico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ThemgicoContext _context;
        private readonly AppSetting _appsetting;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(ThemgicoContext context, IAuthService authService, IUserService userService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await _authService.Login(model);
            return StatusCode(result._statusCode, result);
        }
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var result = await _authService.ChangePassword(model);
            return StatusCode(result._statusCode, result);
        }
        [HttpPut("update-status/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUserStatus(int id)
        {
            var result = await _userService.UpdateUserStatus(id);
            return StatusCode(result._statusCode, result);
        }

        [HttpPost("create-account-staff")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAccountStaff([FromBody] AccountDTO user)
        {
            user.Role = "staff";
            var result = await _userService.CreateAccountStaff(user);
            return StatusCode(result._statusCode, result);
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAll();
            return StatusCode(result._statusCode, result);
        }
    }
}
