using RssReader.Domain.Entities;

namespace RssReader.Domain.Abstractions.Repositories;

public interface IFeedsRepository : IBaseSimpleRepository<Feed>
{
    Task<IList<Feed>> GetAllForFolderAsync(int folderId, CancellationToken? cancellationToken = null);
}
