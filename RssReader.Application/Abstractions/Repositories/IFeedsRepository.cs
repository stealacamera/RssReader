using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IFeedsRepository : IBaseSimpleRepository<int, Feed>
{
    Task<Feed?> GetByUrlAsync(string url, CancellationToken cancellationToken = default);
    Task<bool> IsUrlRegisteredAsync(string url, CancellationToken cancellationToken = default);

    Task<int[]> GetAllIdsAsync(CancellationToken cancellationToken = default);
}
