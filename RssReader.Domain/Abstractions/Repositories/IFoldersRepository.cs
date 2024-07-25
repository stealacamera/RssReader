using RssReader.Domain.Entities;

namespace RssReader.Domain.Abstractions.Repositories;

public interface IFoldersRepository : IBaseRepository<Folder>
{
    Task<IEnumerable<Folder>> GetAllChildrenForFolderAsync(int parentFolderId, CancellationToken? cancellationToken = null);
    Task<IEnumerable<Folder>> GetAllForUserAsync(int userId, CancellationToken? cancellationToken = null);
    Task<Folder?> GetByNameAsync(int requesterId, string folderName, CancellationToken? cancellationToken = null);
}
