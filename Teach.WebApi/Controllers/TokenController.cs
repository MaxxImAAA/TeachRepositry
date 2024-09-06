using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teach.Domain.Dto.TokenDtos;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;

namespace Teach.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService _tokenService)
        {
            this._tokenService = _tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken(TokenDto tokenDto)
        {
            var response = await _tokenService.RefreshToken(tokenDto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
