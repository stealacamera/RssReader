using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.Logout;

public record LogoutCommand(int RequesterId) : IRequest;

internal class LogoutCommandValidator : Validator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .GreaterThan(0);
    }
}