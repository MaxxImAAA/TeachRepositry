using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Dto.TokenDtos;
using Teach.Domain.Result;

namespace Teach.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GeneratteAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
