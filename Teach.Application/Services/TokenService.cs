using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Teach.Application.Resources;
using Teach.Domain.Dto.TokenDtos;
using Teach.Domain.Entity;
using Teach.Domain.Enum;
using Teach.Domain.Interfaces.Repositories;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;
using Teach.Domain.Settings;

namespace Teach.Application.Services
{
    public class TokenService : ITokenService
    {

        private readonly IBaseRepository<User> _userRepository;

        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IOptions<JwtSettings> options,
                IBaseRepository<User> _userRepository)
        {
            _jwtKey = options.Value.JwtKey;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            this._userRepository = _userRepository;
        }

        public string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];

            var randomNumberGenerated = RandomNumberGenerator.Create();

            randomNumberGenerated.GetBytes(randomNumbers);

            string hash = Convert.ToBase64String(randomNumbers);

            return hash;
        }

        public string GeneratteAccessToken(IEnumerable<Claim> claims)
        {
            var secuitirykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentialns = new SigningCredentials(secuitirykey, SecurityAlgorithms.HmacSha256);

            var securityToken =
                new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.UtcNow.AddMinutes(10), credentialns);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
                ValidateLifetime = true,
                ValidAudience = _audience,
                ValidIssuer = _issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");

            return claimsPrincipal;
        }

        public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
        {
            string accesToken = dto.AccesToken;
            string refreshToken = dto.RefreshToken;

            var claimPrincipal = GetPrincipalFromExpiredToken(accesToken);

            var userName = claimPrincipal.Identity?.Name;

            var user = await _userRepository.GetAll()
                .Include(x => x.UserToken)
                .FirstOrDefaultAsync(x => x.Login == userName);

            if (user == null || user.UserToken.RefreshToken != refreshToken
                || user.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new BaseResult<TokenDto>
                {
                    ErrorMessage = ErrorMessage.InvalidClientRequest,
                    ErrorCode = (int)ErrorCodes.InvalidClientrequest,
                };
            }

            var newAccesToken = GeneratteAccessToken(claimPrincipal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            user.UserToken.RefreshToken = newRefreshToken;
             _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto
                {
                    RefreshToken = newRefreshToken,
                    AccesToken = newAccesToken
                }
            };

        }
    }
}
