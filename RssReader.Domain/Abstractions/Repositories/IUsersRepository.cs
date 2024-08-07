﻿using RssReader.Domain.Entities;

namespace RssReader.Domain.Abstractions.Repositories;

public interface IUsersRepository : IBaseSimpleRepository<User>
{
    Task<User> GetByEmailAsync(string email, CancellationToken? cancellationToken = null);
}
