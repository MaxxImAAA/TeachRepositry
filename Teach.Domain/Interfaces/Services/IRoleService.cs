using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Dto.Role;
using Teach.Domain.Entity;
using Teach.Domain.Result;

namespace Teach.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для управления ролями :)
    /// </summary>
    public interface IRoleService
    {
        Task<BaseResult<RoleDto>> CreateRoleAsync(string RoleName);

        Task<BaseResult<RoleDto>> UpdateRoleAsync(string RoleName, string newRoleName);

        Task<BaseResult<RoleDto>> DeleteRoleAsync(int RoleId);

        Task<BaseResult<UserRoleDto>> AddUserRole(UserRoleDto dto);

        Task<BaseResult<UserRoleDto>> UpdateUserRoleAsync(UserRoleDto dto, string new_Role);

        Task<BaseResult> DeleteUserRoleAsync(UserRoleDto dto); 
    }
}
