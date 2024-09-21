using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.LogIn;

public record LoginCommand(string Email, string Password) : IRequest<LoggedInUser>;

internal class LoginCommandValidator : Validator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.Password)
            .NotEmpty();
    }
}