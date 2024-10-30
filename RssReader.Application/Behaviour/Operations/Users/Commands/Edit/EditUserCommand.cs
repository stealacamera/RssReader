using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.Edit;

public record EditUserCommand : IRequest
{
    public int RequesterId { get; }
    public string Username { get; }

    public EditUserCommand(int requesterId, string username)
    {
        RequesterId = requesterId; 
        Username = username.Trim();
    }
}

internal class EditUserCommandValidator : Validator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.Username)
            .MaximumLength(ValidationUtils.UsernameMaxLength);
    }
}