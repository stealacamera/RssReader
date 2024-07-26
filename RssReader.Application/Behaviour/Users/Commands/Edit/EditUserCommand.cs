using FluentValidation;
using MediatR;
using RssReader.Application.Common;

namespace RssReader.Application.Behaviour.Users.Commands.Edit;

public record EditUserCommand(int RequesterId, string Username) : IRequest;

internal class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.Username.Trim())
            .MaximumLength(Utils.UsernameMaxLength);
    }
}