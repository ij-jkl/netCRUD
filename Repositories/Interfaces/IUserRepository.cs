﻿using Crud_API.Entities;

namespace Crud_API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAll();
        Task<UserEntity> GetById(int id);

        Task CreateUser(UserEntity user);
        Task UpdateUser(UserEntity user);
        Task DeleteUser(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UserExistsAsync(string userName);
    }
}