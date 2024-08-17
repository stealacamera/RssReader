using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Common;

namespace RssReader.Infrastructure.Repositories;

internal abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected DbSet<TEntity> _set;

    protected BaseRepository(AppDbContext dbContext)
    {
        _set = dbContext.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _set.AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity)
        => _set.Remove(entity);
}
