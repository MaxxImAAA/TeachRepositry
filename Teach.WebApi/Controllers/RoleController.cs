using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teach.Application.Services;
using Teach.Domain.Dto.Role;
using Teach.Domain.Entity;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;

namespace Teach.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService _roleService)
        {
            this._roleService = _roleService;
        }

        [HttpPost("AddRole")]
        public async Task<ActionResult<BaseResult<RoleDto>>> CreateRole(string RoleName)
        {
            var response = await _roleService.CreateRoleAsync(RoleName);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("DeleteRole")]
        public async Task<ActionResult<BaseResult<RoleDto>>> DeleteRole(int RoleId)
        {
            var response = await _roleService.DeleteRoleAsync(RoleId);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);

        }

        [HttpPatch("UpdateRole")]
        public async Task<ActionResult<BaseResult<RoleDto>>> UpdateRole(string RoleName, string newRoleName)
        {
            var response = await _roleService.UpdateRoleAsync(RoleName, newRoleName);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);

        }

        [HttpPost("AddUserRole")]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> AddUserRole(UserRoleDto dto)
        {
            var response = await _roleService.AddUserRole(dto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPatch("UpdateRoleForUser")]
        public async Task<ActionResult<BaseResult<RoleDto>>> UpdateUserRoleAsync(UserRoleDto dto, string new_Role)
        {
            var response = await _roleService.UpdateUserRoleAsync(dto, new_Role);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
