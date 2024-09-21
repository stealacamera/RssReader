using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Tags.Commands.Edit;

internal class EditTagCommandHandler : BaseHandler, IRequestHandler<EditTagCommand, Tag>
{
    public EditTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Tag> Handle(EditTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await ValidateRequestAsync(request, cancellationToken);

        tag.Name = request.NewTagName.Trim();
        await _workUnit.SaveChangesAsync();

        return new Tag(tag.Id, tag.Name);
    }

    private async Task<Domain.Entities.Tag> ValidateRequestAsync(EditTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new EditTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Validate tag ownership & name uniqeness
        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        return tag;
    }
}
