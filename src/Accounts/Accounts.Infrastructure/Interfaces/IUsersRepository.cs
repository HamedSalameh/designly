﻿using Accounts.Domain;

namespace Accounts.Infrastructure.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    }
}