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

        public UserController(ThemgicoContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        //[HttpPost("Login")]
        //public IActionResult Validate(LoginDTO login)
        //{
        //    var user = _context.Accounts.SingleOrDefault(p => p.Email == login.Email && p.Password == login.Password);
        //    if(user == null)
        //    {
        //        return Ok( new ResponseAPI
        //        {
        //            Success = false,
        //            Message = "Invalid Email/Password"
        //        });
        //    }
        //    return Ok(new ResponseAPI
        //    {
        //        Success = true,
        //        Message = "Authenticate success",
        //        Data = null
        //    });
        //}
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
    }
}
