using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Feeds.Commands.PullAll;

internal class PullFeedsCommandHandler : BaseHandler, IRequestHandler<PullFeedsCommand>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PullFeedsCommandHandler(IHttpClientFactory httpClientFactory, IWorkUnit workUnit) : base(workUnit)
        => _httpClientFactory = httpClientFactory;

    public async Task Handle(PullFeedsCommand request, CancellationToken cancellationToken)
    {
        var feedIds = await _workUnit.FeedsRepository
                                     .GetAllIdsAsync(cancellationToken);

        foreach (var feedId in feedIds)
            await PullFeedAsync(feedId, cancellationToken);
    }

    private async ValueTask PullFeedAsync(int feedId, CancellationToken cancellationToken)
    {
        var feed = await _workUnit.FeedsRepository
                                  .GetByIdAsync(feedId, cancellationToken);

        if (feed == null)
            throw new EntityNotFoundException(nameof(Feed));

        if (await IsFeedUpdatedAsync(feed, cancellationToken))
            await PullFeedItemsAsync(feed, cancellationToken);
    }

    /// <summary>
    /// Checks if the feed has been updated through the ETag & LastModified headers.
    /// If yes, the feed's ETag/LastModified values are updated
    /// </summary>
    private async Task<bool> IsFeedUpdatedAsync(Domain.Entities.Feed feed, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(feed.Url, cancellationToken);

        if (response.Headers.TryGetValues("ETag", out var etagValues))
        {
            if (feed.Update_ETag != null && feed.Update_ETag == etagValues.First())
                return false;

            feed.Update_ETag = etagValues.First();
        }
        else if (response.Headers.TryGetValues("Last-modified", out var lastModifiedValues))
        {
            if (feed.Update_LastModified != null && feed.Update_LastModified == lastModifiedValues.First())
                return false;

            feed.Update_LastModified = lastModifiedValues.First();
        }

        feed.UpdatedAt = DateTime.UtcNow;
        await _workUnit.SaveChangesAsync();

        return true;
    }

    private async Task PullFeedItemsAsync(Domain.Entities.Feed feed, CancellationToken cancellationToken)
    {
        var feedContent = await CodeHollow.FeedReader
                                          .FeedReader
                                          .ReadAsync(feed.Url, cancellationToken);

        await WrapInTransactionAsync(async () =>
        {
            foreach (var item in feedContent.Items)
            {
                // Check if the item is new
                // If not, then the rest of the feed items aren't new either
                // So stop the pull
                if (await _workUnit.FeedItemsRepository
                                   .DoesInstanceExistForFeedAsync(
                                        feed.Id, item.Id, 
                                        item.Link, item.Link, 
                                        cancellationToken))
                    return;

                var entity = new Domain.Entities.FeedItem
                {
                    FeedId = feed.Id,
                    ItemId = item.Id,

                    Title = item.Title,
                    Description = item.Description,
                    Author = item.Author,
                    Link = item.Link,
                    Content = item.Content,

                    CreatedAt = DateTime.UtcNow,
                    PublishedAt = item.PublishingDate
                };

                await _workUnit.FeedItemsRepository
                               .AddAsync(entity, cancellationToken);

                await _workUnit.SaveChangesAsync();
            }
        });
    }
}
