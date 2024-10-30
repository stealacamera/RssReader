using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;

internal class CreateFeedCommandHandler : BaseHandler, IRequestHandler<CreateFeedCommand, Feed>
{
    public CreateFeedCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Feed> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
    {
        //TODO check requester is admin

        await new CreateFeedCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        if (await _workUnit.FeedsRepository.IsUrlRegisteredAsync(request.Url, cancellationToken))
            throw new ExistingEntityException(nameof(Feed));

        var feed = await Utils.CreateFeedEntity(request.Url, request.Name, cancellationToken);

        await _workUnit.FeedsRepository.AddAsync(feed);
        await _workUnit.SaveChangesAsync();

        return new Feed(feed.Id, feed.Url, feed.Name, feed.IconUrl);
    }
}
