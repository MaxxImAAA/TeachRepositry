using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teach.Domain.Dto.TokenDtos;
using Teach.Domain.Dto.User;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;

namespace Teach.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService _authService)
        {
            this._authService = _authService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<BaseResult<UserDto>>> Register(RegisterUserDto dto)
        {
            var response = await _authService.Register(dto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<BaseResult<TokenDto>>> Login(LoginUserDto dto)
        {
            var response = await _authService.Login(dto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
