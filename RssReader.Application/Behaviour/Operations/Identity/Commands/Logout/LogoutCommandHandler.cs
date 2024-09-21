using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.Logout;

internal class LogoutCommandHandler : BaseHandler, IRequestHandler<LogoutCommand>
{
    public LogoutCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await new LogoutCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        var user = (await _workUnit.UsersRepository
                                   .GetByIdAsync(request.RequesterId))!;

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        await _workUnit.SaveChangesAsync();
    }
}
