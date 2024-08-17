using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IFeedsRepository : IBaseSimpleRepository<Feed>
{
    Task<IList<Feed>> GetAllForFolderAsync(int folderId, CancellationToken cancellationToken = default);
}
