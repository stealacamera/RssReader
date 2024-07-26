using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Users.Commands.Edit;

internal class EditUserCommandHandler : BaseCommandHandler, IRequestHandler<EditUserCommand>
{
    public EditUserCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new EditUserCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        var user = await _workUnit.UsersRepository
                                  .GetByIdAsync(request.RequesterId, cancellationToken);

        // Validate user
        if (user == null)
            throw new UnauthorizedException();

        user.Username = request.Username.Trim();
        await _workUnit.SaveChangesAsync();
    }
}
