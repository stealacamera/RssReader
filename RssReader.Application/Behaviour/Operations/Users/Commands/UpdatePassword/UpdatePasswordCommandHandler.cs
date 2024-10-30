using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.Exceptions;
using RssReader.Application.Common.Exceptions.General;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.UpdatePassword;

internal class UpdatePasswordCommandHandler : BaseHandler, IRequestHandler<UpdatePasswordCommand>
{
    public UpdatePasswordCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await ValidateRequestAsync(request, cancellationToken);

        // Update password
        PasswordHasher<User> passwordHasher = new();
        user.HashedPassword = passwordHasher.HashPassword(user, request.NewPassword);
        
        await _workUnit.SaveChangesAsync();
    }

    private async Task<User> ValidateRequestAsync(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new UpdatePasswordCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        var user = await _workUnit.UsersRepository
                                  .GetByIdAsync(request.RequesterId, cancellationToken);

        if (user == null)
            throw new UnauthorizedException();

        // Verify password
        PasswordHasher<User> passwordHasher = new();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.OldPassword);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new FailedPasswordVerification();

        return user;
    }
}
