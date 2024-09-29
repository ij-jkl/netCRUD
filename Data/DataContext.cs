using Crud_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}

