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

namespace Crud_API.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dbContext;
        private readonly IUserRepository _userRepository;

        public UserService(DataContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
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

                if (userDtos == null || userDtos.Count == 0)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.NOTFOUND,
                        Message = "There arent any users registred.",
                        Response = null
                    };
                }
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,
                    Message = "OK",
                    Response = userDtos.ToList()
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception has ocurred ({ex.Message})",
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
                        Message = "There arent any users registred with that ID.",
                        Response = null
                    };
                }

                var userById = new UserEntity
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Password = user.Password,
                    Email = user.Email
                };

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,
                    Message = "The user with that ID is : .",
                    Response = userById
                };

            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception has ocurred ({ex.Message})",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> CreateUser(UserPostDto userPostDto)
        {

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

                if (userEntity == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.BADREQUEST,
                        Message = $"User Entity is null : ",
                        Response = null
                    };
                }

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.CREATED,
                    Message = $"User was created : ",
                    Response = userEntity
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception has ocurred ({ex.Message})",
                    Response = null
                };
            }

        }

        public async Task<ResponseObjectJsonDto> UpdateUser(int id, UserPutDto userPutDto)
        {
            try
            {
                var existingUser = await _userRepository.GetById(id);

                if (existingUser == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.BADREQUEST,
                        Message = $"User not found with id : {id}",
                        Response = null
                    };
                }

                existingUser.Name = userPutDto.Name;
                existingUser.Email = userPutDto.Email;
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userPutDto.Password);
                existingUser.UserName = userPutDto.UserName;

                await _userRepository.UpdateUser(existingUser);

                var updatedUserDto = new UserPutDto
                {
                    Name = existingUser.Name,
                    UserName = existingUser.UserName,
                    Email = existingUser.Email,
                    Password = existingUser.Password
                };

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,  
                    Message = $"The user was updated successfully :",
                    Response = updatedUserDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception occurred : {ex.Message}",
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
                        Message = $"The user that you are trying to Delete wasnt Found : ",
                        Response = null
                    };
                }

                _userRepository.DeleteUser(id);
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.NOCONTENT,
                    Message = $"The user was Deleted : ",
                    Response = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception has ocurred ({ex.Message})",
                    Response = null
                };
            }
        }

        public async Task<ResponseObjectJsonDto> VerifyUser(LoginDto loginDto)
        {
            try
            {
                var existingUser = await _userRepository.GetByUserName(loginDto.UserName);

                if (existingUser == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.NOTFOUND,
                        Message = "User not found : ",
                        Response = null
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.Password))
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.UNAUTHORIZED,
                        Message = "Incorrect password : ",
                        Response = null
                    };
                }

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.OK,
                    Message = "User and password are correct : ",
                    Response = new { UserName = existingUser.UserName }
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception has occurred ({ex.Message})",
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
                        Message = "Username already exists and cannot be registered, try with a different one : ",
                        Response = null
                    };
                }
                else
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.OK, 
                        Message = "Username is available for registration!",
                        Response = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.INTERNALSERVER,
                    Message = $"Exception has occurred ({ex.Message})",
                    Response = null
                };
            }
        }
    }
}