using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForUser;

internal class GetAllFeedItemsForUserQueryHandler : BaseHandler, IRequestHandler<GetAllFeedItemsForUserQuery, PaginatedResponse<DateTime, IList<FeedItem>>>
{
    public GetAllFeedItemsForUserQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<PaginatedResponse<DateTime, IList<FeedItem>>> Handle(GetAllFeedItemsForUserQuery request, CancellationToken cancellationToken)
    {
        await new GetAllFeedItemsForUserQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // For each feed user is subscribed to
        // Get its subscription name (as given by the user) & its icon url
        var subscriptions = (await _workUnit.FeedSubscriptionsRepository
                                            .GetAllForUserAsync(request.RequesterId, cancellationToken))
                                            .Select(async e => (e.Name, await _workUnit.FeedsRepository
                                                                                       .GetByIdAsync(e.FeedId, cancellationToken)))
                                            .Select(e => e.Result)
                                            .ToDictionary(e => e.Item2.Id, e => (e.Item1, e.Item2.IconUrl));

        var feedItems = await _workUnit.FeedItemsRepository
                                       .GetAllForFeedsAsync(
                                            subscriptions.Keys.ToArray(), 
                                            request.PageSize, 
                                            request.Cursor, 
                                            cancellationToken);

        return new PaginatedResponse<DateTime, IList<FeedItem>>(
            feedItems.NextCursor,
            feedItems.Values
                     .Select(e => new FeedItem(
                                        subscriptions[e.FeedId].Item1, subscriptions[e.FeedId].IconUrl, e.Title, 
                                        e.Author, e.Link, e.Description, e.Content, e.PublishedAt))
                     .ToList()
        );
    }
}
