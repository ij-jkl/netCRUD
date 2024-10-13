using Crud_API.Commons;
using Crud_API.Dtos.Get;
using Crud_API.Dtos.Post;
using Crud_API.Dtos.Put;
using Crud_API.Entities;

namespace Crud_API.Services.IServices
{
    public interface IUserService
    {
        Task<ResponseObjectJsonDto> GetAll();
        Task<ResponseObjectJsonDto> GetById(int id);
        Task<ResponseObjectJsonDto> CreateUser(UserPostDto user);
        Task<ResponseObjectJsonDto> UpdateUser(int id,UserPutDto user);
        Task<ResponseObjectJsonDto> DeleteUser(int id);
    }
}

