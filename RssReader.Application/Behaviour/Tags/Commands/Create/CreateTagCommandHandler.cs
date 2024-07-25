using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Tags.Commands.Create;

internal class CreateTagCommandHandler : BaseCommandHandler, IRequestHandler<CreateTagCommand, Tag>
{
    public CreateTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Tag> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request
        await ValidateRequestAsync(request, cancellationToken);

        // Create tag
        var tag = await _workUnit.TagsRepository
                                 .AddAsync(new Domain.Entities.Tag
                                 {
                                     Name = request.TagName.Trim(),
                                     OwnerId = request.RequesterId
                                 }, cancellationToken);

        await _workUnit.SaveChangesAsync();
        return new Tag(tag.Id, tag.Name);
    }

    private async Task ValidateRequestAsync(CreateTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new CreateTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();

        // Validate tag uniqueness
        if (await _workUnit.TagsRepository
                           .GetByNameForUserAsync(request.TagName, request.RequesterId, cancellationToken) != null)
            throw new ExistingTagException(request.TagName);
    }
}
