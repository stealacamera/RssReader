using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.FeedSubscriptions.Create;

internal class SubscribeToFeedCommandHandler : BaseHandler, IRequestHandler<SubscribeToFeedCommand, FeedSubscription>
{
    private readonly IPublisher _publisher;

    public SubscribeToFeedCommandHandler(IWorkUnit workUnit, IPublisher publisher) : base(workUnit)
        => _publisher = publisher;

    public async Task<FeedSubscription> Handle(SubscribeToFeedCommand request, CancellationToken cancellationToken)
    {
        //Validate request
        await ValidateRequestAsync(request, cancellationToken);

        var feed = await _workUnit.FeedsRepository
                                  .GetByUrlAsync(request.Url, cancellationToken);

        // Create feed if it doesn't exist
        if (feed == null)
        {
            feed = await Utils.CreateFeedEntity(request.Url, cancellationToken: cancellationToken);

            await _workUnit.FeedsRepository.AddAsync(feed, cancellationToken);
            await _workUnit.SaveChangesAsync();
        }

        if (await _workUnit.FeedSubscriptionsRepository
                           .DoesInstanceExistAsync(feed.Id, request.FolderId))
            throw new ExistingEntityException("Subscription to this feed");

        // Add subscription
        var subscription = new Domain.Entities.FeedSubscription
        {
            CreatedAt = DateTime.UtcNow,
            FeedId = feed.Id,
            FolderId = request.FolderId,
            Name = request.Name ?? feed.Name ?? request.Url
        };

        await _workUnit.FeedSubscriptionsRepository.AddAsync(subscription, cancellationToken);
        await _workUnit.SaveChangesAsync();

        return new FeedSubscription(subscription.Id, subscription.Name);
    }

    private async Task ValidateRequestAsync(SubscribeToFeedCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new SubscribeToFeedCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Validate folder & ownership
        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(request.FolderId, cancellationToken);

        if (folder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (folder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}