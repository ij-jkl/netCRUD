using Crud_API.Data;
using Crud_API.Dtos.Get;
using Crud_API.Repositories;
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

        public async Task<List<UserGetDto>> GetAll()
        {
            var userDtos = new List<UserGetDto>();  

            return userDtos;
        }
    }
}
