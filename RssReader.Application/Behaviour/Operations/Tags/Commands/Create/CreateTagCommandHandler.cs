using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Behaviour.Operations.Tags.Commands.Create;

internal class CreateTagCommandHandler : BaseHandler, IRequestHandler<CreateTagCommand, Tag>
{
    public CreateTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Tag> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request
        await ValidateRequestAsync(request, cancellationToken);

        // Create tag
        var tag = new Domain.Entities.Tag
        {
            Name = request.TagName.Trim(),
            OwnerId = request.RequesterId
        };

        await _workUnit.TagsRepository.AddAsync(tag, cancellationToken);
        await _workUnit.SaveChangesAsync();

        return new Tag(tag.Id, tag.Name);
    }

    private async Task ValidateRequestAsync(CreateTagCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        var validationDetails = await new CreateTagCommandValidator().ValidateAsync(request, cancellationToken);

        if (!validationDetails.IsValid)
            throw new ValidationException(validationDetails.ToDictionary());

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();
    }
}
