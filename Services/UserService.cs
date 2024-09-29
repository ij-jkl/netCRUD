using Crud_API.Data;
using Crud_API.Dtos.Get;
using Crud_API.Dtos.Post;
using Crud_API.Dtos.Put;
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

        public async Task<UserEntity> GetById(int id)
        {         
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return null; 
            }

            var userById = new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Password = user.Password, 
                Email = user.Email
            };

            return userById; 
        }

        public async Task<UserPostDto> CreateUser(UserPostDto userPostDto)
        {
            var userEntity = new UserEntity
            {
                Id = userPostDto.Id,
                Name = userPostDto.Name,
                Email = userPostDto.Email,
                Password = userPostDto.Password,
                UserName = userPostDto.UserName
            };

            await _userRepository.CreateUser(userEntity);

            return userPostDto;
        }

        public async Task<UserPutDto> UpdateUser(UserPostDto userPostDto)
        {
            
            var existingUser = await _userRepository.GetById(userPostDto.Id);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.Name = userPostDto.Name;
            existingUser.Email = userPostDto.Email;
            existingUser.Password = userPostDto.Password;
            existingUser.UserName = userPostDto.UserName;

            await _userRepository.UpdateUser(existingUser);

            var userPutDto = new UserPutDto
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                UserName = existingUser.UserName,
                Email = existingUser.Email
            };

            return userPutDto;
        }
        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            await _userRepository.DeleteUser(id);
        }

    }
}