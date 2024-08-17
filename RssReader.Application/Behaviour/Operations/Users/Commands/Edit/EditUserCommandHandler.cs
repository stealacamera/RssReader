using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.Edit;

internal class EditUserCommandHandler : BaseHandler, IRequestHandler<EditUserCommand>
{
    public EditUserCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        var validationDetails = await new EditUserCommandValidator().ValidateAsync(request, cancellationToken);

        if (!validationDetails.IsValid)
            throw new ValidationException(validationDetails.ToDictionary());

        // Validate user
        var user = await _workUnit.UsersRepository
                                  .GetByIdAsync(request.RequesterId, cancellationToken);

        if (user == null)
            throw new UnauthorizedException();

        user.Username = request.Username.Trim();
        await _workUnit.SaveChangesAsync();
    }
}
