using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;

internal class AddTagToFeedCommandHandler : BaseHandler, IRequestHandler<AddTagToFeedCommand, FeedTag>
{
    public AddTagToFeedCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<FeedTag> Handle(AddTagToFeedCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        await _workUnit.FeedTagsRepository
                       .AddAsync(new Domain.Entities.FeedTag
                       {
                           FeedId = request.FeedId,
                           TagId = request.TagId,
                           CreatedAt = DateTime.UtcNow
                       }, cancellationToken);

        await _workUnit.SaveChangesAsync();
        return new FeedTag(request.FeedId, request.TagId);
    }

    private async Task ValidateRequestAsync(AddTagToFeedCommand request, CancellationToken cancellationToken)
    {
        await new AddTagToFeedCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        await ValidateTagAsync(request.TagId, request.RequesterId, cancellationToken);
        await ValidateFeedAsync(request.FeedId, request.RequesterId, cancellationToken);
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
        // Validate feed
        var feed = await _workUnit.FeedsRepository
                                  .GetByIdAsync(feedId, cancellationToken);

        if (feed == null)
            throw new EntityNotFoundException(nameof(Feed));

        // Validate feed ownership
        var feedFolder = await _workUnit.FoldersRepository
                                        .GetByIdAsync(feed.FolderId, cancellationToken);

        if (feedFolder!.OwnerId != requesterId)
            throw new UnauthorizedException();
    }
}
