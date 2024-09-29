using Crud_API.Data;
using Crud_API.Entities;
using Crud_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crud_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext; 

        public UserRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<UserEntity>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<UserEntity> GetById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(usr => usr.Id == id);
        }

        public async Task CreateUser(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateUser(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(usr => usr.Id == user.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {user.Id} not found");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.UserName = user.UserName;

            _dbContext.Users.Update(existingUser);  
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}