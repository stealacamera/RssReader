using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.UpdatePassword;

public record UpdatePasswordCommand(
    int RequesterId,
    string OldPassword,
    string NewPassword)
    : IRequest;

internal class UpdatePasswordCommandValidator : Validator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.OldPassword.Trim())
            .NotEmpty();

        RuleFor(e => e.NewPassword.Trim())
            .NotEmpty()
            .MinimumLength(ValidationUtils.PasswordMinLength)
            .MaximumLength(ValidationUtils.PasswordMaxLength)
            .ValidPasswordCharacters();

        RuleFor(e => new { e.OldPassword, e.NewPassword })
            .Must(e => !e.NewPassword.Equals(e.OldPassword))
            .WithMessage("The new password should be different from the current password");
    }
}