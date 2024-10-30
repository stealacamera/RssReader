using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForFeed;

internal class GetAllFeedItemsForFeedQueryHandler : BaseHandler, IRequestHandler<GetAllFeedItemsForFeedQuery, PaginatedResponse<DateTime, IList<FeedItem>>>
{
    public GetAllFeedItemsForFeedQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<PaginatedResponse<DateTime, IList<FeedItem>>> Handle(GetAllFeedItemsForFeedQuery request, CancellationToken cancellationToken)
    {
        var feedSubscription = await ValidateRequestAsync(request, cancellationToken);
        
        var feed = (await _workUnit.FeedsRepository
                                  .GetByIdAsync(feedSubscription.FeedId, cancellationToken))!;

        var feedItems = await _workUnit.FeedItemsRepository
                                       .GetAllForFeedAsync(
                                            feed.Id, 
                                            request.PageSize, 
                                            request.Cursor, 
                                            cancellationToken);

        return new PaginatedResponse<DateTime, IList<FeedItem>>(
            feedItems.NextCursor,
            feedItems.Values
                     .Select(e => new FeedItem(
                                        feedSubscription.Name, feed.IconUrl, e.Title, e.Author,
                                        e.Link, e.Description, e.Content, e.PublishedAt))
                     .ToList()
        ); 
    }

    private async Task<Domain.Entities.FeedSubscription> ValidateRequestAsync(GetAllFeedItemsForFeedQuery request, CancellationToken cancellationToken)
    {
        await new GetAllFeedItemsForFeedQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Validate feed subscription
        var feedSubscription = await _workUnit.FeedSubscriptionsRepository
                                              .GetByIdAsync(request.FeedSubscriptionId, cancellationToken);

        if (feedSubscription == null)
            throw new EntityNotFoundException(nameof(Feed));

        // Validate subscription ownership
        var subscriptionFolder = (await _workUnit.FoldersRepository
                                                .GetByIdAsync(feedSubscription.FolderId, cancellationToken))!;

        if (subscriptionFolder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        return feedSubscription;
    }
}
