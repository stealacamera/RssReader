using FluentValidation;
using MediatR;
using RssReader.Application.Common;

namespace RssReader.Application.Behaviour.Users.Commands.UpdatePassword;

public record UpdatePasswordCommand(
    int RequesterId, 
    string OldPassword, 
    string NewPassword) 
    : IRequest;

internal class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
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
            .Must(Utils.IsPasswordValid)
            .WithMessage("Password must consists of a combination of letters, numbers, and symbols");

        RuleFor(e => new { e.OldPassword, e.NewPassword })
            .Must(e => !e.NewPassword.Trim().Equals(e.OldPassword.Trim()))
            .WithMessage("The new password should be different from the current password");
    }
}