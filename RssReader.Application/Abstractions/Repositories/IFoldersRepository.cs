using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IFoldersRepository : IBaseSimpleRepository<int, Folder>
{
    Task<IEnumerable<Folder>> GetAllChildrenForFolderAsync(int parentFolderId, CancellationToken cancellationToken = default);
    Task<int[]> GetAllChildrenIdsForFolderAsync(int parentFolderId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Folder>> GetAllForUserAsync(int userId, CancellationToken cancellationToken = default);
}
