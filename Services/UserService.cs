using Crud_API.Commons;
using Crud_API.Commons.Enum;
using Crud_API.Data;
using Crud_API.Dtos.Get;
using Crud_API.Dtos.Post;
using Crud_API.Dtos.Put;
using Crud_API.Entities;
using Crud_API.Repositories.Interfaces;
using Crud_API.Services.IServices;

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
                var userEntity = new UserEntity
                {
                    Name = userPostDto.Name,
                    Email = userPostDto.Email,
                    Password = userPostDto.Password,
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

        public async Task<ResponseObjectJsonDto> UpdateUser(UserPutDto userPutDt)
        {
            try
            {

                var existingUser = await _userRepository.GetById(userPutDt.Id);

                if (existingUser == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.BADREQUEST,
                        Message = $"User data is null : ",
                        Response = null
                    };
                }

                existingUser.Id = userPutDt.Id;
                existingUser.Name = userPutDt.Name;
                existingUser.Email = userPutDt.Email;
                existingUser.Password = userPutDt.Password;
                existingUser.UserName = userPutDt.UserName;

                await _userRepository.UpdateUser(existingUser);


                if (existingUser.Id == null)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodesHttp.NOTFOUND,
                        Message = $"The user that you are trying to Update wasnt Found : ",
                        Response = null
                    };
                }

                var userPutDto = new UserPutDto
                {
                    Id = existingUser.Id,
                    Name = existingUser.Name,
                    UserName = existingUser.UserName,
                    Email = existingUser.Email,
                    Password = existingUser.Password
                };

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodesHttp.CREATED,
                    Message = $"The user was updated succesfully: ",
                    Response = userPutDto
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
    }
}