using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.UpdateTokens;

public record UpdateTokensCommand(int RequesterId, string JwtToken, string RefreshToken) : IRequest<Tokens>;

internal class UpdateTokensCommandValidator : AbstractValidator<UpdateTokensCommand>
{
    public UpdateTokensCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.JwtToken)
            .NotEmpty();

        RuleFor(e => e.RefreshToken)
            .NotEmpty();
    }
}