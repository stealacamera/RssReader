using RssReader.Domain.Common;

namespace RssReader.Application.Abstractions.Repositories;

public interface IBaseSimpleRepository<TKey, TEntity> : IBaseRepository<TEntity> 
    where TEntity : BaseSimpleEntity<TKey>
    where TKey : struct, IComparable<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<bool> DoesInstanceExistAsync(TKey id, CancellationToken cancellationToken = default);
}
