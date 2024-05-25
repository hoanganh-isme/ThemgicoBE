using Themgico.DTO.Authorization;
using Themgico.DTO;
using Themgico.DTO.Account;
using Cursus.Models.DTO;

namespace Themgico.Service.Interface
{
    public interface IAuthService
    {
        public Task<ResultDTO<string>> UserRegistration(AccountDTO model);
        public Task<ResultDTO<LoginResponseDTO>> Login(LoginDTO model);
        public Task<ResultDTO<ChangePasswordDTO>> ChangePassword(ChangePasswordDTO model);
    }
}
