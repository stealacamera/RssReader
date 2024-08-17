using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;

internal class CreateFeedCommandHandler : BaseHandler, IRequestHandler<CreateFeedCommand, Feed>
{
    public CreateFeedCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Feed> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        //Validate request
        await ValidateRequestAsync(request, cancellationToken);

        // Create feed
        var feed = new Domain.Entities.Feed
        {
            CreatedAt = DateTime.UtcNow,
            FolderId = request.FolderId,
            Name = request.Name != null ? request.Name.Trim() : request.Url.Remove(80),
            Url = request.Url.Trim()
        };

        await _workUnit.FeedsRepository.AddAsync(feed, cancellationToken);
        await _workUnit.SaveChangesAsync();

        return new Feed(feed.Id, feed.Url, feed.Name);
    }

    private async Task ValidateRequestAsync(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        var validationDetails = await new CreateFeedCommandValidator().ValidateAsync(request, cancellationToken);

        if (!validationDetails.IsValid)
            throw new ValidationException(validationDetails.ToDictionary());

        // Validate user existence
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();

        // Validate folder & ownership
        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(request.FolderId, cancellationToken);

        if (folder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (folder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
