using Cursus.Models.DTO;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Themgico.DTO;
using Themgico.DTO.Account;
using Themgico.DTO.Authorization;
using Themgico.Entities;
using Themgico.Repositories;
using Themgico.Service.Interface;

namespace Themgico.Service
{
    public class AuthService : IAuthService
    {
        const string PHONE_NUMBER_PATTERN = @"^(\+84|0)(3|5|7|8|9)([0-9]{8})$";

        // Expire time in minute
        private const int TOKEN_EXPIRE_TIME = 60 * 24 * 30; // 1 months
        private const int ADMIN_TOKEN_EXPIRE_TIME = 60; // 1 hours
        private readonly ThemgicoContext _context;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public AuthService(
            ThemgicoContext context,
            ITokenService tokenService,
            IUserService userService
        )
        {
            _context = context;
            _tokenService = tokenService;
            _userService = userService;
        }
        public async Task<ResultDTO<LoginResponseDTO>> Login(LoginDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                    return ResultDTO<LoginResponseDTO>.Fail("Email is required", 400);

                if (string.IsNullOrEmpty(model.Password))
                    return ResultDTO<LoginResponseDTO>.Fail("Password is required", 400);

                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || user.Password != model.Password)
                    return ResultDTO<LoginResponseDTO>.Fail("Email or password is wrong");

                // Create user claims
                var claims = new List<Claim>
        {
            new Claim("Name", user.Name),
            new Claim("Email", user.Email),
            new Claim("Id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                // Assuming you have a method to get user roles from the database
                var userRoles = await _context.Accounts
                                              .Where(ur => ur.Id == user.Id)
                                              .Select(ur => ur.Role)
                                              .ToListAsync();

                claims.AddRange(userRoles.Select(userRole => new Claim("Role", userRole)));

                var expireTime = userRoles.Contains("Admin") ? ADMIN_TOKEN_EXPIRE_TIME : TOKEN_EXPIRE_TIME;
                var token = _tokenService.GetToken(claims, expireTime);

                var loginRes = new LoginResponseDTO
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expire = token.ValidTo
                };

                return ResultDTO<LoginResponseDTO>.Success(loginRes);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return ResultDTO<LoginResponseDTO>.Fail("Service is not available");
            }
        }


        public Task<ResultDTO<string>> UserRegistration(AccountDTO model)
        {
            throw new NotImplementedException();
        }
        public async Task<ResultDTO<ChangePasswordDTO>> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                    return ResultDTO<ChangePasswordDTO>.Fail("Current password is required", 400);

                if (string.IsNullOrEmpty(model.NewPassword))
                    return ResultDTO<ChangePasswordDTO>.Fail("New password is required", 400);

                if (string.IsNullOrEmpty(model.ConfirmNewPassword))
                    return ResultDTO<ChangePasswordDTO>.Fail("Confirm new password is required", 400);

                // Find the current user
                var user = await _userService.GetCurrentUser();
                if (user is null)
                    throw new Exception("Get current user result is null");

                // Check current password
                if (!await _userService.CheckPasswordAsync(user, model.CurrentPassword))
                {
                    return ResultDTO<ChangePasswordDTO>.Fail("Current password is incorrect", 400);
                }

                // Check if current password and new password are the same
                if (model.CurrentPassword.Equals(model.NewPassword))
                {
                    return ResultDTO<ChangePasswordDTO>.Fail("New password cannot be the same as the current password", 400);
                }

                // Check if new password and confirm new password match
                if (!model.ConfirmNewPassword.Equals(model.NewPassword))
                    return ResultDTO<ChangePasswordDTO>.Fail("Confirm new password does not match new password", 400);

                // Change password
                var result = await _userService.UpdatePassword(user, model.NewPassword);
                if (!result)
                {
                    return ResultDTO<ChangePasswordDTO>.Fail("Failed to change password");
                }

                return ResultDTO<ChangePasswordDTO>.Success(null, "Password has been changed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<ChangePasswordDTO>.Fail("Service is not available");
            }
        }
    }
}
