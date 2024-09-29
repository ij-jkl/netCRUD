using Crud_API.Dtos.Get;
using Crud_API.Dtos.Post;
using Crud_API.Dtos.Put;
using Crud_API.Entities;

namespace Crud_API.Services.IServices
{
    public interface IUserService
    {
        Task<List<UserGetDto>> GetAll();
        Task<UserEntity> GetById(int id);
        Task<UserPostDto> CreateUser(UserPostDto user);
        Task<UserPutDto> UpdateUser(UserPostDto user); 
    }
}

