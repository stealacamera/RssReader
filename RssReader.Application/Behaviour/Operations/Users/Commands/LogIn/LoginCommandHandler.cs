using MediatR;
using Microsoft.AspNetCore.Identity;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.LogIn;

internal class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommand, LoggedInUser>
{
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IWorkUnit workUnit, IJwtProvider jwtProvider) : base(workUnit)
        => _jwtProvider = jwtProvider;

    public async Task<LoggedInUser> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await ValidateRequestAsync(request, cancellationToken);

        // Update tokens
        _jwtProvider.UpdateRefreshTokens(user);
        await _workUnit.SaveChangesAsync();

        // Return user & tokens
        var tokens = new Tokens(_jwtProvider.GenerateToken(user.Id, user.Email), user.RefreshToken!);

        return new LoggedInUser(
            new User(user.Id, user.Email, user.Username),
            tokens);
    }

    private void VerifyPassword(Domain.Entities.User user, string password)
    {
        PasswordHasher<Domain.Entities.User> passwordHasher = new();
        var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

        if (passwordVerification == PasswordVerificationResult.Failed)
            throw new FailedPasswordVerification();
    }

    private async Task<Domain.Entities.User> ValidateRequestAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        var validationDetails = await new LoginCommandValidator().ValidateAsync(request, cancellationToken);

        if (!validationDetails.IsValid)
            throw new ValidationException(validationDetails.ToDictionary());

        // Validate user
        var user = await _workUnit.UsersRepository
                                  .GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
            throw new EntityNotFoundException(nameof(User));
        
        VerifyPassword(user, request.Password);
        
        if (!user.IsEmailConfirmed)
            throw new UnconfirmedEmailException();

        return user;
    }
}
