using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class FoldersRepository : BaseSimpleRepository<Folder>, IFoldersRepository
{
    public FoldersRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Folder>> GetAllChildrenForFolderAsync(int parentFolderId, CancellationToken cancellationToken = default)
    {
        var query = _set.Where(e => e.ParentId == parentFolderId);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Folder>> GetAllForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var query = _set.Where(e => e.OwnerId == userId && e.ParentId == null);
        return await query.ToListAsync(cancellationToken);
    }
}
