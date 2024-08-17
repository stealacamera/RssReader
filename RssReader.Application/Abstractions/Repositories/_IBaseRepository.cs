using RssReader.Domain.Common;

namespace RssReader.Application.Abstractions.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Delete(T entity);

}