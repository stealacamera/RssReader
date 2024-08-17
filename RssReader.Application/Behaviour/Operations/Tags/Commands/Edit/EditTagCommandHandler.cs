using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Behaviour.Operations.Tags.Commands.Edit;

internal class EditTagCommandHandler : BaseHandler, IRequestHandler<EditTagCommand, Tag>
{
    public EditTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Tag> Handle(EditTagCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        tag!.Name = request.NewTagName.Trim();
        await _workUnit.SaveChangesAsync();

        return new Tag(tag.Id, tag.Name);
    }

    private async Task ValidateRequestAsync(EditTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        var validationDetails = await new EditTagCommandValidator().ValidateAsync(request, cancellationToken);

        if (!validationDetails.IsValid)
            throw new ValidationException(validationDetails.ToDictionary());

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();

        // Validate tag ownership & name uniqeness
        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
