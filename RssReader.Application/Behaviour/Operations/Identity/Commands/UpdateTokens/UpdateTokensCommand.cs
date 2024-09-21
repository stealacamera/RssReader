using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.UpdateTokens;

public record UpdateTokensCommand(string JwtToken, string RefreshToken) : IRequest<Tokens>;

internal class UpdateTokensCommandValidator : Validator<UpdateTokensCommand>
{
    public UpdateTokensCommandValidator()
    {
        RuleFor(e => e.JwtToken)
            .NotEmpty();

        RuleFor(e => e.RefreshToken)
            .NotEmpty();
    }
}