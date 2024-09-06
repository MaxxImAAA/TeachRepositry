using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Application.Common.CustomExceptions;
using Teach.Application.Resources;
using Teach.Application.Validations;
using Teach.Domain.Dto.Role;
using Teach.Domain.Entity;
using Teach.Domain.Enum;
using Teach.Domain.Interfaces.Repositories;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;

namespace Teach.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;

        public RoleService(IBaseRepository<Role> _roleRepository, IBaseRepository<User> _userRepository,
            IMapper _mapper, IBaseRepository<UserRole> _userRoleRepository)
        {
            this._roleRepository = _roleRepository;
            this._userRepository = _userRepository;
            this._userRoleRepository = _userRoleRepository;
            this._mapper = _mapper;

        }

        public async Task<BaseResult<UserRoleDto>> AddUserRole(UserRoleDto dto)
        {
            var user = await _userRepository.GetAll()
                                             .Include(x=>x.Roles)
                                             .ThenInclude(x=>x.Role)
                                            .FirstOrDefaultAsync(x=>x.Login == dto.UserName);

            var role = await _roleRepository.GetAll()
                                            .FirstOrDefaultAsync(x => x.Name == dto.RoleName);



            if (user == null)
            {
                throw new CustomBaseException(ErrorMessage.UserNotFound, (int)ErrorCodes.UserNotFound);
            }

            if (role == null)
            {
                throw new CustomBaseException(ErrorMessage.RoleNotFound, (int)ErrorCodes.RoleNotFound);
            }

            var result = user.Roles.FirstOrDefault(x => x.Role.Name == dto.RoleName);

            if(result != null)
            {
                throw new CustomBaseException("Такая роль уже есть", 35);
            }

            var user_role = new UserRole
            {
                User = user,
                Role = role
            };

            await _userRoleRepository.CreateAsync(user_role);
            await _userRoleRepository.SaveChangesAsync();

            return new BaseResult<UserRoleDto>
            {
                Data = _mapper.Map<UserRoleDto>(user_role),
            };


        }

        public async Task<BaseResult<RoleDto>> CreateRoleAsync(string RoleName)
        {
            var role = await _roleRepository.GetAll()
                                            .FirstOrDefaultAsync(x => x.Name == RoleName);

            if(role != null)
            {
                throw new CustomBaseException("Такая роль уже имеется", 55);
            }

            role = new Role()
            {
                Name = RoleName,
                Users = new List<UserRole>()
            };

            await _roleRepository.CreateAsync(role);
            await _roleRepository.SaveChangesAsync();

            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role)
            };
        }

        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(int RoleId)
        {
            var role = await _roleRepository.GetAll()
                                           .FirstOrDefaultAsync(x => x.Id == RoleId);

            if (role == null)
            {
                throw new CustomBaseException("Такой роли нет", 56);
            }

             _roleRepository.Delete(role);
            await _roleRepository.SaveChangesAsync();

            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role)
            };


        }

        public async Task<BaseResult> DeleteUserRoleAsync(UserRoleDto dto)
        {
            var user = await _userRepository.GetAll()
                .Include(x=>x.Roles).ThenInclude(x=>x.Role)
                .FirstOrDefaultAsync(x => x.Login == dto.UserName);

            if(user == null)
            {
                throw new CustomBaseException(ErrorMessage.UserNotFound,(int)ErrorCodes.UserNotFound);
            }

            var user_role = user.Roles.FirstOrDefault(x=>x.Role.Name == dto.UserName);

            if( user_role == null)
            {
                throw new CustomBaseException(ErrorMessage.RoleNotFound, (int)ErrorCodes.RoleNotFound);
            }

            _userRoleRepository.Delete(user_role);
            await _userRoleRepository.SaveChangesAsync();

            return new BaseResult
            {

            };


        }

        public async Task<BaseResult<RoleDto>> UpdateRoleAsync(string RoleName, string newRoleName)
        {
            var role = await _roleRepository.GetAll()
                                           .FirstOrDefaultAsync(x => x.Name == RoleName);

            if (role == null)
            {
                throw new CustomBaseException("Такой роли нет", 56);
            }

            role.Name = newRoleName;

             _roleRepository.Update(role);
            await _roleRepository.SaveChangesAsync();

            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role)
            };
                

        }

        public async Task<BaseResult<UserRoleDto>> UpdateUserRoleAsync(UserRoleDto dto, string new_Role)
        {
            var user = await _userRepository.GetAll()
               .Include(x => x.Roles).ThenInclude(x => x.Role)
               .FirstOrDefaultAsync(x => x.Login == dto.UserName);

            if (user == null)
            {
                throw new CustomBaseException(ErrorMessage.UserNotFound, (int)ErrorCodes.UserNotFound);
            }

            var user_role = user.Roles.FirstOrDefault(x => x.Role.Name == dto.RoleName);

            if (user_role == null)
            {
                throw new CustomBaseException(ErrorMessage.RoleNotFound, (int)ErrorCodes.RoleNotFound);
            }
            var new_role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == new_Role);
            user_role.Role = new_role;

             _userRoleRepository.Update(user_role);
            await _userRoleRepository.SaveChangesAsync();

            return new BaseResult<UserRoleDto>
            {
                Data = _mapper.Map<UserRoleDto>(user_role)
            };
        }
    }
}
