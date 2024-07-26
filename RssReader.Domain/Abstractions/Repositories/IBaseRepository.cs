using RssReader.Domain.Common;

namespace RssReader.Domain.Abstractions.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity, CancellationToken? cancellationToken = null);
    void Delete(T entity, CancellationToken? cancellationToken = null);

}

public interface IBaseSimpleRepository<T> : IBaseRepository<T> where T : BaseSimpleEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken? cancellationToken = null);
    Task<bool> DoesInstanceExistAsync(int id, CancellationToken? cancellationToken = null);
}
