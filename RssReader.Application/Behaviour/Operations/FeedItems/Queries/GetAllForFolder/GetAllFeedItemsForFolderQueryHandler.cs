using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;
using System.Collections.Concurrent;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForFolder;

internal class GetAllFeedItemsForFolderQueryHandler : BaseHandler, IRequestHandler<GetAllFeedItemsForFolderQuery, PaginatedResponse<DateTime, IList<FeedItem>>>
{
    public GetAllFeedItemsForFolderQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<PaginatedResponse<DateTime, IList<FeedItem>>> Handle(GetAllFeedItemsForFolderQuery request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);
        var subscriptions = await GetSubscriptionTree(request.FolderId, cancellationToken);

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
                                        subscriptions[e.FeedId].Item1, subscriptions[e.FeedId].Item2, e.Title, e.Author,
                                        e.Link, e.Description, e.Content, e.PublishedAt))
                     .ToList()
        );
    }

    private async Task ValidateRequestAsync(GetAllFeedItemsForFolderQuery request, CancellationToken cancellationToken)
    {
        await new GetAllFeedItemsForFolderQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(request.FolderId, cancellationToken);

        // Validate folder & ownership
        if (folder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (folder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }

    private async Task<ConcurrentDictionary<int, (string, string?)>> GetSubscriptionTree(int folderId, CancellationToken cancellationToken)
    {
        var subscriptionNames = new ConcurrentDictionary<int, (string, string?)>();

        // Get folder's subscriptions
        var subscriptions = await _workUnit.FeedSubscriptionsRepository
                                           .GetAllForFolderAsync(folderId, cancellationToken);


        foreach (var subscription in subscriptions)
        {
            var feed = (await _workUnit.FeedsRepository
                                      .GetByIdAsync(subscription.FeedId, cancellationToken))!;

            subscriptionNames.TryAdd(subscription.FeedId, (subscription.Name, feed.IconUrl));
        }

        // Get subfolders' subscriptions
        var subfolderIds = await _workUnit.FoldersRepository
                                          .GetAllChildrenIdsForFolderAsync(folderId, cancellationToken);

        await Parallel.ForEachAsync(
            subfolderIds,
            async (subfolderId, cancellationToken) => subscriptionNames.Concat(await GetSubscriptionTree(subfolderId, cancellationToken)));

        return subscriptionNames;
    }
}
