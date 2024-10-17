using Crud_API.Commons;
using Crud_API.Commons.Enum;
using Crud_API.Data;
using Crud_API.Dtos.Get;
using Crud_API.Dtos.Post;
using Crud_API.Dtos.Put;
using Crud_API.Entities;
using Crud_API.Repositories.Interfaces;
using Crud_API.Services.IServices;
using BCrypt.Net;
using Crud_API.Dtos.Login;
using FluentValidation;
using FluentValidation.Results;

namespace Crud_API.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserPostDto> _userPostValidator;
        private readonly IValidator<UserPutDto> _userPutValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator;

        public UserService(
            DataContext dbContext,
            IUserRepository userRepository,
            IValidator<UserPostDto> userPostValidator,
            IValidator<UserPutDto> userPutValidator,
            IValidator<LoginDto> loginDtoValidator)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userPostValidator = userPostValidator;
            _userPutValidator = userPutValidator;
            _loginDtoValidator = loginDtoValidator;
        }

        public async Task<ResponseObjectJsonDto> GetAll()
        {
            try
            {
                var userDtos = (await _userRepository.GetAll()).Select(user => new UserGetDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName
                }).ToList();

                return userDtos.Any()
                    ? new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.OK,
                        Message = "OK",
                        Response = userDtos
                    }
                    : new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.NOTFOUND,
                        Message = "There aren't any users registered.",
                        Response = null
                    };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred ({ex.Message})",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> GetById(int id)
        {
            try
            {
                var user = await _userRepository.GetById(id);

                if (user == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.NOCONTENT,
                        Message = "There aren't any users registered with that ID.",
                        Response = null
                    };
                }

                var userById = new UserGetDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName
                };

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,
                    Message = "The user with that ID is found.",
                    Response = userById
                };

            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred ({ex.Message})",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> CreateUser(UserPostDto userPostDto)
        {
            // Validate the UserPostDto
            ValidationResult validationResult = await _userPostValidator.ValidateAsync(userPostDto);
            if (!validationResult.IsValid)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.BADREQUEST,
                    Message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    Response = null
                };
            }

            try
            {
                if (await _userRepository.UserExists(userPostDto.UserName))
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.CONFLICT,
                        Message = "Username already exists.",
                        Response = null
                    };
                }

                var userEntity = new UserEntity
                {
                    Name = userPostDto.Name,
                    Email = userPostDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userPostDto.Password),
                    UserName = userPostDto.UserName
                };

                await _userRepository.CreateUser(userEntity);

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.CREATED,
                    Message = "User was created successfully.",
                    Response = userEntity
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred ({ex.Message})",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> UpdateUser(int id, UserPutDto userPutDto)
        {
            // Validate the UserPutDto
            ValidationResult validationResult = await _userPutValidator.ValidateAsync(userPutDto);
            if (!validationResult.IsValid)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.BADREQUEST,
                    Message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    Response = null
                };
            }

            try
            {
                var existingUser = await _userRepository.GetById(id);

                if (existingUser == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.BADREQUEST,
                        Message = $"User not found with ID: {id}",
                        Response = null
                    };
                }

                existingUser.Name = userPutDto.Name;
                existingUser.Email = userPutDto.Email;
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userPutDto.Password);
                existingUser.UserName = userPutDto.UserName;

                await _userRepository.UpdateUser(existingUser);

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,
                    Message = "The user was updated successfully.",
                    Response = userPutDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred: {ex.Message}",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetById(id);

                if (user == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.NOTFOUND,
                        Message = "The user you are trying to delete was not found.",
                        Response = null
                    };
                }

                await _userRepository.DeleteUser(id);
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.NOCONTENT,
                    Message = "The user was deleted successfully.",
                    Response = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred ({ex.Message})",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> VerifyUser(LoginDto loginDto)
        {
            // Validate the LoginDto
            ValidationResult validationResult = await _loginDtoValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.BADREQUEST,
                    Message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    Response = null
                };
            }

            try
            {
                var existingUser = await _userRepository.GetByUserName(loginDto.UserName);

                if (existingUser == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.NOTFOUND,
                        Message = "User not found.",
                        Response = null
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.Password))
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.UNAUTHORIZED,
                        Message = "Incorrect password.",
                        Response = null
                    };
                }

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,
                    Message = "User and password are correct.",
                    Response = new { UserName = existingUser.UserName }
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred ({ex.Message})",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> UserExists(string userName)
        {
            try
            {
                var exists = await _userRepository.UserExists(userName);

                if (exists)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.CONFLICT,
                        Message = "Username already exists and cannot be registered. Try a different one.",
                        Response = null
                    };
                }

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,
                    Message = "Username is available for registration!",
                    Response = exists
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred ({ex.Message})",
                    Response = null
                };
            }
        }

    }
}
