using FluentValidation;
using MediatR;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.VerifyOTP;

public record VerifyOTPCommand(int RequesterId, string Password) : IRequest<bool>;

internal class ValidateOTPCommandValidator : AbstractValidator<VerifyOTPCommand>
{
    public ValidateOTPCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.Password)
            .NotEmpty()
            .MaximumLength(6);
    }
}