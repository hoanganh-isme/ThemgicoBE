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

        public async Task<ResultDTO<string>> UpdateUserStatus(int id)
        {
            try
            {
                var user = await _context.Accounts.FindAsync(id);
                if (user == null)
                    return ResultDTO<string>.Fail("User not found");

                // Đảo ngược trạng thái hiện tại của user
                user.Status = !user.Status;
                _context.Accounts.Update(user);
                await _context.SaveChangesAsync();
                return ResultDTO<string>.Success("User status updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<string>.Fail("Failed to update user status");
            }
        }
            public async Task<ResultDTO<AccountDTO>> CreateAccountStaff(AccountDTO userDTO)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(userDTO.Name))
                {
                    return ResultDTO<AccountDTO>.Fail("Name is required.");
                }

                if (string.IsNullOrEmpty(userDTO.Email))
                {
                    return ResultDTO<AccountDTO>.Fail("Email is required.");
                }

                if (string.IsNullOrEmpty(userDTO.Password))
                {
                    return ResultDTO<AccountDTO>.Fail("Password is required.");
                }

                if (string.IsNullOrEmpty(userDTO.Role))
                {
                    return ResultDTO<AccountDTO>.Fail("Role is required.");
                }

                var user = new Account
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    Password = userDTO.Password,
                    Phone = userDTO.Phone,
                    Role = userDTO.Role,
                    Status = userDTO.Status ?? true // Default to true if null
                };

                _context.Accounts.Add(user);
                await _context.SaveChangesAsync();

                userDTO.Id = user.Id;

                return ResultDTO<AccountDTO>.Success(userDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<AccountDTO>.Fail("Failed to create account staff.");
            }
        }

        public async Task<ResultDTO<List<AccountDTO>>> GetAll()
        {
            try
            {
                var users = await _context.Accounts.ToListAsync();

                var userDTOs = users.Select(user => new AccountDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role,
                    Status = user.Status
                }).ToList();

                return ResultDTO<List<AccountDTO>>.Success(userDTOs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<List<AccountDTO>>.Fail("Failed to fetch all accounts.");
            }
        }

    }
}
