using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Users.Commands.Create;

public record CreateUserCommand(
    string Email, 
    string Password, 
    string? Username = null) 
    : IRequest<User>;

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(e => e.Password.Trim())
            .NotEmpty()
            .Must(Utils.IsPasswordValid);

        When(
            e => e.Username != null, 
            () => RuleFor(e => e.Username!.Trim())
                    .MaximumLength(Utils.UsernameMaxLength));
    }
}