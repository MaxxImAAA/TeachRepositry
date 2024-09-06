using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Dto.User;
using Teach.Domain.Result;

namespace Teach.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResult> AddUser(CreateUserDto createUserDto);
    }
}
