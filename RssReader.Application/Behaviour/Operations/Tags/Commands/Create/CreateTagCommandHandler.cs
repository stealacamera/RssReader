using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

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
        await new CreateTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);
    }
}
