using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.UpdateTokens;

internal class UpdateTokensCommandHandler : BaseHandler, IRequestHandler<UpdateTokensCommand, Tokens>
{
    private readonly IJwtProvider _jwtProvider;

    public UpdateTokensCommandHandler(
        IWorkUnit workUnit,
        IJwtProvider jwtProvider) 
        : base(workUnit)
    {
        _jwtProvider = jwtProvider;
    }

    public async Task<Tokens> Handle(UpdateTokensCommand request, CancellationToken cancellationToken)
    {
        var user = await ValidateRequestAsync(request, cancellationToken);

        // Update tokens
        _jwtProvider.UpdateRefreshTokens(user);
        await _workUnit.SaveChangesAsync();

        return new Tokens(
            _jwtProvider.GenerateToken(user.Id, user.Email),
            user.RefreshToken!);
    }

    private async Task<Domain.Entities.Identity.User> ValidateRequestAsync(UpdateTokensCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new UpdateTokensCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user & refresh tokens
        var userId = _jwtProvider.ExtractIdFromToken(request.JwtToken);

        var user = await _workUnit.UsersRepository
                                  .GetByIdAsync(userId, cancellationToken);

        if (user == null || user.RefreshToken != request.RefreshToken)
            throw new UnauthorizedException();
        else if (user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new ExpiredRefreshTokenException();

        return user;
    }
}