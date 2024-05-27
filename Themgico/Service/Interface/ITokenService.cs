using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Themgico.Repositories
{
    public interface ITokenService
    {
        public JwtSecurityToken GetToken(IEnumerable<Claim> claims, int expireTime);
    }
}
