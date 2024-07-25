using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.FeedTags.Commands.AddTagToFeed;

internal class AddTagToFeedCommandHandler : BaseCommandHandler, IRequestHandler<AddTagToFeedCommand>
{
    public AddTagToFeedCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(AddTagToFeedCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        await _workUnit.FeedTagsRepository
                       .CreateAsync(new Domain.Entities.FeedTag
                       {
                           FeedId = request.FeedId,
                           TagId = request.TagId
                       }, cancellationToken);

        await _workUnit.SaveChangesAsync();
    }

    private async Task ValidateRequestAsync(AddTagToFeedCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new AddTagToFeedCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();

        // Validate tag ownership
        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        // Validate feed
        var feed = await _workUnit.FeedsRepository
                                  .GetByIdAsync(request.FeedId, cancellationToken);

        if (feed == null)
            throw new EntityNotFoundException(nameof(Feed));

        // Validate feed ownership
        var feedFolder = await _workUnit.FoldersRepository
                                        .GetByIdAsync(feed.FolderId, cancellationToken);

        if (feedFolder!.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
