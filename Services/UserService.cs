using Crud_API.Data;
using Crud_API.Dtos.Get;
using Crud_API.Entities;
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
            var userDtos = (await _userRepository.GetAll()).Select(user => new UserGetDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName
            }).ToList();

            return userDtos;
        }

        public async Task<User> GetById(int id)
        {         
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return null; 
            }

            var userById = new User
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Password = user.Password, 
                Email = user.Email
            };

            return userById; 
        }

    }
}
