using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Common;

namespace RssReader.Infrastructure.Repositories;

internal abstract class BaseSimpleRepository<TKey, TSimpleEntity> : 
    BaseRepository<TSimpleEntity>, IBaseSimpleRepository<TKey, TSimpleEntity> 
    where TSimpleEntity : BaseSimpleEntity<TKey>
    where TKey : struct, IComparable<TKey>
{
    protected BaseSimpleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> DoesInstanceExistAsync(TKey id, CancellationToken cancellationToken = default)
        => await _set.FindAsync(id) != null;

    public async Task<TSimpleEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await _set.FindAsync(id);
}
