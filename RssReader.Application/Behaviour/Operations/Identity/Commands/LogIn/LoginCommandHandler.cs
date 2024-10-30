using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.LogIn;

internal class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommand, LoggedInUser>
{
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IWorkUnit workUnit, IJwtProvider jwtProvider) : base(workUnit)
        => _jwtProvider = jwtProvider;

    public async Task<LoggedInUser> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var user = (await _workUnit.UsersRepository
                                   .GetByEmailAsync(request.Email, cancellationToken))!;

        // Update tokens
        _jwtProvider.UpdateRefreshTokens(user);
        await _workUnit.SaveChangesAsync();

        // Return user & tokens
        return new LoggedInUser(
            new User(user.Id, user.Email, user.Username),
            new Tokens(_jwtProvider.GenerateToken(user.Id, user.Email), user.RefreshToken!));
    }

    private async Task ValidateRequestAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new LoginCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        var user = await _workUnit.UsersRepository
                                  .GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || !IsPasswordCorrect(user, request.Password))
            throw new InvalidLoginCredentialsException();
        else if (!user.IsEmailConfirmed)
            throw new UnauthorizedException();
    }

    private bool IsPasswordCorrect(Domain.Entities.Identity.User user, string password)
    {
        PasswordHasher<Domain.Entities.Identity.User> passwordHasher = new();
        var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

        return passwordVerification != PasswordVerificationResult.Failed;
    }
}