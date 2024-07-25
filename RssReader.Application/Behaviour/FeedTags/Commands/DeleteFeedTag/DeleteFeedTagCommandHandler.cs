using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.FeedTags.Commands.DeleteFeedTag;

internal class DeleteFeedTagCommandHandler : BaseCommandHandler, IRequestHandler<DeleteFeedTagCommand>
{
    public DeleteFeedTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(DeleteFeedTagCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var feedTag = await _workUnit.FeedTagsRepository
                                     .GetByIdsAsync(request.FeedId, request.TagId, cancellationToken);

        _workUnit.FeedTagsRepository.Delete(feedTag!, cancellationToken);
        await _workUnit.SaveChangesAsync();
    }

    private async Task ValidateRequestAsync(DeleteFeedTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new DeleteFeedTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();

        // Validate feed tag
        if (!await _workUnit.FeedTagsRepository
                            .DoesInstanceExistAsync(request.FeedId, request.TagId, cancellationToken))
            throw new EntityNotFoundException("Feed's tag");

        // Validate tag ownership
        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        // Validate feed ownership
        var feed = await _workUnit.FeedsRepository
                                  .GetByIdAsync(request.FeedId, cancellationToken);

        if (feed == null)
            throw new UnauthorizedException();

        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(feed.FolderId, cancellationToken);

        if (folder!.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
