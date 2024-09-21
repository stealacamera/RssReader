using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.ResendEmailVerification;

public record ResendEmailVerificationCommand(int UserId) : IRequest;

internal class ResendEmailVerificationCommandValidator : Validator<ResendEmailVerificationCommand>
{
    public ResendEmailVerificationCommandValidator()
    {
        RuleFor(e => e.UserId)
            .GreaterThan(0);
    }
}