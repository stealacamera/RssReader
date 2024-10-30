using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForTag;

internal class GetAllFeedItemsForTagQueryHandler : BaseHandler, IRequestHandler<GetAllFeedItemsForTagQuery, PaginatedResponse<DateTime, IList<FeedItem>>>
{
    public GetAllFeedItemsForTagQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<PaginatedResponse<DateTime, IList<FeedItem>>> Handle(GetAllFeedItemsForTagQuery request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var subscriptionIds = (await _workUnit.FeedSubscriptionTagsRepository
                                           .GetAllForTagAsync(request.TagId, cancellationToken))
                                           .Select(e => e.FeedSubscriptionId)
                                           .ToArray();

        var subscriptionDetails = subscriptionIds.Select(async e => await GetSubscriptionFeedDetailsAsync(e, cancellationToken))
                                                 .Select(e => e.Result)
                                                 .ToDictionary(e => e.Id, e => (e.Name, e.IconUrl));
        
        var feedItems = await _workUnit.FeedItemsRepository
                                       .GetAllForFeedsAsync(
                                            subscriptionDetails.Keys.ToArray(), request.PageSize, 
                                            cursor: request.Cursor, cancellationToken: cancellationToken);
                
        return new PaginatedResponse<DateTime, IList<FeedItem>>(
            feedItems.NextCursor,
            feedItems.Values
                     .Select(e => new FeedItem(
                         subscriptionDetails[e.FeedId].Name, 
                         subscriptionDetails[e.FeedId].IconUrl, 
                         e.Title, e.Author, e.Link, 
                         e.Description, e.Content, e.PublishedAt))
                     .ToList()
        );
    }

    private async Task ValidateRequestAsync(GetAllFeedItemsForTagQuery request, CancellationToken cancellationToken)
    {
        await new GetAllFeedItemsForTagQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }

    private async Task<(int Id, string Name, string? IconUrl)> GetSubscriptionFeedDetailsAsync(int subscriptionId, CancellationToken cancellationToken)
    {
        var subscription = (await _workUnit.FeedSubscriptionsRepository
                                           .GetByIdAsync(subscriptionId, cancellationToken))!;

        var feed = (await _workUnit.FeedsRepository
                                   .GetByIdAsync(subscription.FeedId, cancellationToken))!;

        return (feed.Id, subscription.Name, feed.IconUrl);
    }
}
