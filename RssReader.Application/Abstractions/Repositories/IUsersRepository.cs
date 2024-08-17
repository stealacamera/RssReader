using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IUsersRepository : IBaseSimpleRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
