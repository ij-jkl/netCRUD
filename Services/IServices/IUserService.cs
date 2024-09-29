using Crud_API.Dtos.Get;
using Crud_API.Entities;

namespace Crud_API.Services.IServices
{
    public interface IUserService
    {
        Task<List<UserGetDto>> GetAll();
        //Task<User> GetById(int id);
    }
}

//Task<List<PublicacionGetDto>> GetAll();
//Task<Publication> GetById(int id);