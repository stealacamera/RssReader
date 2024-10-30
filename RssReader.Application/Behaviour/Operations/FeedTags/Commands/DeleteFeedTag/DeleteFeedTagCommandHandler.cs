using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.DeleteFeedTag;

internal class DeleteFeedTagCommandHandler : BaseHandler, IRequestHandler<DeleteFeedTagCommand>
{
    public DeleteFeedTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(DeleteFeedTagCommand request, CancellationToken cancellationToken)
    {
        var feedTag = await ValidateRequestAsync(request, cancellationToken);
        
        _workUnit.FeedSubscriptionTagsRepository.Delete(feedTag);
        await _workUnit.SaveChangesAsync();
    }

    private async Task<Domain.Entities.FeedSubscriptionTag> ValidateRequestAsync(DeleteFeedTagCommand request, CancellationToken cancellationToken)
    {
        await new DeleteFeedTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        await ValidateFeedSubscriptionAsync(request.FeedSubscriptionId, request.RequesterId, cancellationToken);
        await ValidateTagAsync(request.TagId, request.RequesterId, cancellationToken);

        // Validate feed tag
        var feedTag = await _workUnit.FeedSubscriptionTagsRepository
                                     .GetByIdsAsync(request.TagId, request.FeedSubscriptionId, cancellationToken);

        if (feedTag == null)
            throw new EntityNotFoundException("Feed's tag");

        return feedTag;
    }

    private async Task ValidateTagAsync(int tagId, int requesterId, CancellationToken cancellationToken)
    {
        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(tagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != requesterId)
            throw new UnauthorizedException();
    }

    private async Task ValidateFeedSubscriptionAsync(int feedSubscriptionId, int requesterId, CancellationToken cancellationToken)
    {
        var subscription = await _workUnit.FeedSubscriptionsRepository
                                          .GetByIdAsync(feedSubscriptionId, cancellationToken);

        if (subscription == null)
            throw new EntityNotFoundException(nameof(Feed));

        var subscriptionFolder = (await _workUnit.FoldersRepository
                                                .GetByIdAsync(subscription.FolderId, cancellationToken))!;

        if (subscriptionFolder.OwnerId != requesterId)
            throw new UnauthorizedException();
    }
}
