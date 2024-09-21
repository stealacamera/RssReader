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
        
        _workUnit.FeedTagsRepository.Delete(feedTag);
        await _workUnit.SaveChangesAsync();
    }

    private async Task<Domain.Entities.FeedTag> ValidateRequestAsync(DeleteFeedTagCommand request, CancellationToken cancellationToken)
    {
        await new DeleteFeedTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Validate feed tag
        var feedTag = await _workUnit.FeedTagsRepository
                                     .GetByIdsAsync(request.FeedId, request.TagId, cancellationToken);

        if (feedTag == null)
            throw new EntityNotFoundException("Feed's tag");

        await ValidateTagAsync(request.TagId, request.RequesterId, cancellationToken);
        await ValidateFeedAsync(request.FeedId, request.RequesterId, cancellationToken);

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

    private async Task ValidateFeedAsync(int feedId, int requesterId, CancellationToken cancellationToken)
    {
        var feed = await _workUnit.FeedsRepository
                                  .GetByIdAsync(feedId, cancellationToken);

        if (feed == null)
            throw new EntityNotFoundException(nameof(Feed));

        // Validate ownership
        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(feed.FolderId, cancellationToken);

        if (folder!.OwnerId != requesterId)
            throw new UnauthorizedException();
    }
}
