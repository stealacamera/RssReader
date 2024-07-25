using RssReader.Domain.Common;

namespace RssReader.Domain.Abstractions.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity, CancellationToken? cancellationToken = null);
    Task<T?> GetByIdAsync(int id, CancellationToken? cancellationToken = null);
    Task<bool> DoesInstanceExistAsync(int id, CancellationToken? cancellationToken = null);
    Task DeleteAsync(int id, CancellationToken? cancellationToken = null);
}
