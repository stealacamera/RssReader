using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RssReader.Application.Common;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Users.Commands.UpdatePassword;

internal class UpdatePasswordCommandHandler : BaseCommandHandler, IRequestHandler<UpdatePasswordCommand>
{
    public UpdatePasswordCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new UpdatePasswordCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        var user = await _workUnit.UsersRepository
                                  .GetByIdAsync(request.RequesterId, cancellationToken);

        // Validate user
        if (user == null)
            throw new UnauthorizedException();

        // Verify password
        PasswordHasher<Domain.Entities.User> passwordHasher = new();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.OldPassword);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new FailedPasswordVerification();

        // Update password
        user.HashedPassword = passwordHasher.HashPassword(user, request.NewPassword);
        await _workUnit.SaveChangesAsync();
    }
}
