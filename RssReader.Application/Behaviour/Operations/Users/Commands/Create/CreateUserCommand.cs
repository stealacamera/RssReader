using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.Create;

public record CreateUserCommand : IRequest<User>
{
    public string Email { get; }
    public string Password { get; }
    public string? Username { get; }

    public CreateUserCommand(string email, string password, string? username = null)
    {
        Email = email.Trim();
        Password = password.Trim();
        Username = string.IsNullOrEmpty(username?.Trim()) ? null : username.Trim();
    }
}

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
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