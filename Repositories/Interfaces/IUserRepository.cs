using Crud_API.Entities;

namespace Crud_API.Repositories.Interfaces
{
    public interface IUserRepository 
    {
        Task<List<User>> GetAll();
    }
}

//Task<List<Publication>> GetAllPublication();