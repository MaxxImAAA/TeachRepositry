using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Teach.Application.Resources;
using Teach.Domain.Dto.TokenDtos;
using Teach.Domain.Dto.User;
using Teach.Domain.Entity;
using Teach.Domain.Enum;
using Teach.Domain.Interfaces.Repositories;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;
using BCrypt;
using BCrypt.Net;
using Teach.Application.Common.CustomExceptions;

namespace Teach.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserToken> _userTokenRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly ILogger _loger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        

        public AuthService(IBaseRepository<User> _userRepository, ILogger _loger, IMapper _mapper,
            IBaseRepository<UserToken> _userTokenRepository, ITokenService _tokenService,
            IBaseRepository<Role> _roleRepository, IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
            this._userRepository = _userRepository;
            this._userTokenRepository = _userTokenRepository;
            this._loger = _loger;
            this._mapper = _mapper;
            this._tokenService = _tokenService;
            this._roleRepository = _roleRepository;
        }

        public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
        {
           
                var user = await _userRepository.GetAll()
                                            .Include(x=>x.Roles).ThenInclude(x=>x.Role)
                                           .FirstOrDefaultAsync(x => x.Login == dto.Login);

                if (user == null)
                {
                    return new BaseResult<TokenDto>
                    {
                        ErrorMessage = ErrorMessage.UserNotFound,
                        ErrorCode = (int)ErrorCodes.UserNotFound
                    };
                }

                var isVerifyPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

                if(!isVerifyPassword)
                {
                    throw new CustomBaseException(ErrorMessage.PasswordIsWrong, (int)ErrorCodes.PasswordIsWrong);
                    /*return new BaseResult<TokenDto>
                    {
                        ErrorMessage = ErrorMessage.PasswordIsWrong,
                        ErrorCode = (int)ErrorCodes.PasswordIsWrong
                    };*/
                }

                

                var userToken = await _userTokenRepository.GetAll()
                                                    .FirstOrDefaultAsync(x => x.UserId == user.Id);

                var userRoles = user.Roles.Select(x => x.Role.Name).ToList();
                var claims = userRoles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
                claims.Add(new Claim(ClaimTypes.Name, user.Login));
                /*var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, "User")
                };*/

                var accesToken = _tokenService.GeneratteAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                if(userToken == null)
                {
                    userToken = new UserToken
                    {
                        UserId = user.Id,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)

                    };

                    await _userTokenRepository.CreateAsync(userToken);
                }
                else
                {
                    userToken.RefreshToken = refreshToken;
                    userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                }

                return new BaseResult<TokenDto>()
                {
                    Data = new TokenDto
                    {
                        AccesToken = accesToken,
                        RefreshToken = refreshToken,
                    }
                };


            
           


        }

        public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
        {

                

                if(dto.Password != dto.PasswordConfirm)
                {
                    throw new CustomBaseException(ErrorMessage.PasswordNotEqualsPasswordConfirm,
                        (int)ErrorCodes.userPasswordIncorected);
                    
                }

                var user = await _userRepository.GetAll()
                                            .FirstOrDefaultAsync(x => x.Login == dto.Login);

                if(user != null)
                {
                    throw new CustomBaseException(ErrorMessage.UserIsNotNull, (int)ErrorCodes.UserIsRegistered);
                  
                }

                

            var hash_Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == "User");
                    user = new User
                    {
                        Login = dto.Login,
                        Password = hash_Password,
                        Reports = new List<Report>(),
                        Roles = new List<UserRole>()

                    };

                    await _unitOfWork.Users.CreateAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    var user_role = new UserRole
                    {
                        Role = role,
                        User = user
                    };

                    await _unitOfWork.UserRoles.CreateAsync(user_role);

                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                }

            }

           

               

                return new BaseResult<UserDto>
                {
                    Data = _mapper.Map<UserDto>(user)
                };
            
          
        }

        private string HashPassword(string Password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(Password));

            return BitConverter.ToString(bytes).ToLower();
        }

        private bool VerifyPassword(string UserPasswordHash, string UserPassword)
        {
            var hash = HashPassword(UserPassword);

            return UserPasswordHash == hash ? true : false;
        }
    }
}
