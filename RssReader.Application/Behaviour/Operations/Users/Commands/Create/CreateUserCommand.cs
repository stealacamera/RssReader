using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Enums;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.Create;

public record CreateUserCommand : IRequest<User>
{
    public string Email { get; }
    public string Password { get; }
    public string? Username { get; }

    public Roles Role { get; }

    public CreateUserCommand(
        string email, 
        string password, 
        Roles role, 
        string? username = null)
    {
        Email = email.Trim();
        Password = password.Trim();
        Username = string.IsNullOrWhiteSpace(username) ? null : username.Trim();
        
        Role = role;

    }
}

internal class CreateUserCommandValidator : Validator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(e => e.Password)
            .NotEmpty()
            .MinimumLength(ValidationUtils.PasswordMinLength)
            .MaximumLength(ValidationUtils.PasswordMaxLength)
            .ValidPasswordCharacters();

        When(
            e => e.Username != null, 
            () => RuleFor(e => e.Username)
                    .MaximumLength(ValidationUtils.UsernameMaxLength));
    }
}