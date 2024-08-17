using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Common;

namespace RssReader.Infrastructure.Repositories;

internal abstract class BaseSimpleRepository<TSimpleEntity> : 
    BaseRepository<TSimpleEntity>, IBaseSimpleRepository<TSimpleEntity> 
    where TSimpleEntity : BaseSimpleEntity
{
    protected BaseSimpleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> DoesInstanceExistAsync(int id, CancellationToken cancellationToken = default)
        => await _set.FindAsync(id) != null;

    public async Task<TSimpleEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _set.FindAsync(id);
}
