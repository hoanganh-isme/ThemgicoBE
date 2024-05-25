using Cursus.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Themgico.DTO;
using Themgico.DTO.Account;
using Themgico.Entities;
using Themgico.Service.Interface;

namespace Themgico.Service
{
    public class UserService : IUserService
    {
        private readonly ThemgicoContext _context;
        private readonly ClaimsPrincipal _claimsPrincipal;
        public UserService(IHttpContextAccessor httpContextAccessor, ThemgicoContext context) 
        {
            _context = context;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
        }
        Task<ResultDTO<List<AccountDTO>>> IUserService.GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Account> GetCurrentUser()
        {
            // Lấy userId từ claims
            var userIdClaim = _claimsPrincipal.Claims
                .FirstOrDefault(c => c.Type == "Id");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return null;

            try
            {
                // Truy xuất dữ liệu người dùng trực tiếp từ database
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Id == userId);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<bool> CheckPasswordAsync(Account user, string password)
        {
            //var passwordHasher = new PasswordHasher<Account>();
            //var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);
            //return result == PasswordVerificationResult.Success;
            var userFromDb = await _context.Accounts.FindAsync(user.Id);

            // Kiểm tra xem người dùng tồn tại và mật khẩu khớp
            if (userFromDb != null && userFromDb.Password == password)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdatePassword(Account user, string newPassword)
        {
            //try
            //{
            //    // Sử dụng PasswordHasher để băm mật khẩu mới
            //    var passwordHasher = new PasswordHasher<Account>();
            //    user.Password = passwordHasher.HashPassword(user, newPassword);

            //    // Cập nhật mật khẩu trong cơ sở dữ liệu
            //    _context.Accounts.Update(user);
            //    await _context.SaveChangesAsync();

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return false;
            //}
            try
            {
                // Lấy người dùng từ cơ sở dữ liệu bằng ID hoặc bất kỳ thuộc tính nào khác đại diện cho người dùng
                var userFromDb = await _context.Accounts.FindAsync(user.Id);

                if (userFromDb != null)
                {
                    // Cập nhật mật khẩu mới
                    userFromDb.Password = newPassword;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    // Người dùng không tồn tại trong cơ sở dữ liệu
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
