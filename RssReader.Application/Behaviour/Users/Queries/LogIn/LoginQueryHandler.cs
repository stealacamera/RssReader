using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Users.Queries.LogIn;

internal class LoginQueryHandler : BaseCommandHandler, IRequestHandler<LoginQuery>
{
    public LoginQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new LoginQueryValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        var user = await _workUnit.UsersRepository
                                  .GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
            throw new EntityNotFoundException(nameof(User));

        // Verify password
        PasswordHasher<Domain.Entities.User> passwordHasher = new();
        var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);

        if (passwordVerification == PasswordVerificationResult.Failed)
            throw new FailedPasswordVerification();
    }
}
