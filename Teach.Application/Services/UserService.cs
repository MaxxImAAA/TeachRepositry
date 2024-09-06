using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Application.Resources;
using Teach.Domain.Dto.User;
using Teach.Domain.Entity;
using Teach.Domain.Enum;
using Teach.Domain.Interfaces.Repositories;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;

namespace Teach.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly ILogger _logger;
        public UserService(IBaseRepository<User> _userRepository, ILogger _logger)
        {
            this._userRepository = _userRepository;
            this._logger = _logger;
        }

        public async Task<BaseResult> AddUser(CreateUserDto createUserDto)
        {
            try
            {
                var user = new User()
                {
                    Login = createUserDto.Login,
                    Password = createUserDto.Password,
                    Reports = new List<Report>()
                };

                await _userRepository.CreateAsync(user);

                return new BaseResult
                {

                };

            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };

            }
        }
    }
}
