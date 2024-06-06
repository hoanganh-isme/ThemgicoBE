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
        
        public Task<bool> CheckPasswordAsync(Account user, string password);
        public Task<bool> UpdatePassword(Account user, string newPassword);
        //Task<ResultDTO<string>> UpdateUserProfile(UserProfileUpdateDTO updateUser);
        public Task<ResultDTO<string>> UpdateUserStatus(int id);
        public Task<ResultDTO<AccountDTO>> CreateAccountStaff(AccountDTO user);
        public Task<ResultDTO<List<AccountDTO>>> GetAll();

    }
}
