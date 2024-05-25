using Cursus.Models.DTO;
using Themgico.DTO;
using Themgico.DTO.Account;
using Themgico.Entities;

namespace Themgico.Service.Interface
{
    public interface IUserService
    {
        public Task<Account> GetCurrentUser();
        //public Task<ResultDTO<UserProfileDTO>> GetUserProfile();
        public Task<ResultDTO<List<AccountDTO>>> GetAll();
        public Task<bool> CheckPasswordAsync(Account user, string password);
        public Task<bool> UpdatePassword(Account user, string newPassword);
        //Task<ResultDTO<string>> UpdateUserProfile(UserProfileUpdateDTO updateUser);
        //Task<ResultDTO<string>> UpdateUserStatus(int id, string status);
    }
}
