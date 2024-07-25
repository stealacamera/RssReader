using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using System.Text.RegularExpressions;

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

        RuleFor(e => e.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .Must(password =>
            {
                Regex allowedCharacters = new(@"^(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).*$");
                return allowedCharacters.IsMatch(password);
            })
            .WithMessage("Password requires a letter a-z, at least one number and one symbol");

        RuleFor(e => e.Username)
            .MaximumLength(100);
    }
}