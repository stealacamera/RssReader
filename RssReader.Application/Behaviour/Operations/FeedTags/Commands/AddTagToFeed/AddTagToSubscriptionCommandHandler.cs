using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;

internal class AddTagToSubscriptionCommandHandler : BaseHandler, IRequestHandler<AddTagToSubscriptionCommand, FeedSubscriptionTag>
{
    public AddTagToSubscriptionCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<FeedSubscriptionTag> Handle(AddTagToSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var feedSubscription = await ValidateRequestAsync(request, cancellationToken);

        await _workUnit.FeedSubscriptionTagsRepository
                       .AddAsync(new Domain.Entities.FeedSubscriptionTag
                       {
                           FeedSubscriptionId = feedSubscription.Id,
                           TagId = request.TagId,
                           CreatedAt = DateTime.UtcNow
                       }, cancellationToken);

        await _workUnit.SaveChangesAsync();
        return new FeedSubscriptionTag(feedSubscription.Id, request.TagId);
    }

    private async Task<Domain.Entities.FeedSubscription> ValidateRequestAsync(AddTagToSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await new AddTagToSubscriptionCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Validate tag & ownership
        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        // Validate feed subscription & onwership
        var subscription = await _workUnit.FeedSubscriptionsRepository
                                          .GetByIdAsync(request.FeedSubscriptionId, cancellationToken);

        if (subscription == null)
            throw new EntityNotFoundException(nameof(Feed));

        var subscriptionFolder = (await _workUnit.FoldersRepository
                                                .GetByIdAsync(subscription.FolderId, cancellationToken))!;

        if (subscriptionFolder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        // Ensure tag isn't already attached to subscription
        if (await _workUnit.FeedSubscriptionTagsRepository
                           .DoesInstanceExistAsync(tag.Id, subscription.Id, cancellationToken))
            throw new ExistingEntityException("Feed's tag");

        return subscription;
    }
}
