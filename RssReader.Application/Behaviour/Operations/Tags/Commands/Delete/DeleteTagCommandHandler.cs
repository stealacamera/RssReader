using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Tags.Commands.Delete;

internal class DeleteTagCommandHandler : BaseHandler, IRequestHandler<DeleteTagCommand>
{
    public DeleteTagCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        await new DeleteTagCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        var tag = await _workUnit.TagsRepository
                                 .GetByIdAsync(request.TagId, cancellationToken);

        if (tag == null)
            throw new EntityNotFoundException(nameof(Tag));
        else if (tag.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        _workUnit.TagsRepository.Delete(tag);
        await _workUnit.SaveChangesAsync();
    }
}
