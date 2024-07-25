using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Tags.Commands.Edit;

internal class EditTagCommandHandler : BaseCommandHandler, IRequestHandler<EditTagCommand, Tag>
{
    public EditTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Tag> Handle(EditTagCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId);

        tag!.Name = request.NewTagName.Trim();
        await _workUnit.SaveChangesAsync();

        return new Tag(tag.Id, tag.Name);
    }

    private async Task ValidateRequestAsync(EditTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new EditTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();
        
        // Validate tag ownership & name uniqeness
        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
        else if (await _workUnit.TagsRepository
                                .GetByNameForUserAsync(request.NewTagName, request.RequesterId, cancellationToken) != null)
            throw new ExistingTagException(request.NewTagName);
    }
}
