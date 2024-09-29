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
        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(usr => usr.Id == id);
        }
    }
}
