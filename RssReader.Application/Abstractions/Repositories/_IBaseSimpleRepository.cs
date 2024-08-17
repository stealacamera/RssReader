using RssReader.Domain.Common;

namespace RssReader.Application.Abstractions.Repositories;

public interface IBaseSimpleRepository<T> : IBaseRepository<T> where T : BaseSimpleEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DoesInstanceExistAsync(int id, CancellationToken cancellationToken = default);
}
