using RssReader.Domain.Entities.Identity;

namespace RssReader.Application.Abstractions.Repositories.Identity;

public interface IUsersRepository : IBaseSimpleRepository<int, User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
