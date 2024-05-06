﻿using Entities;

namespace Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
    }
}