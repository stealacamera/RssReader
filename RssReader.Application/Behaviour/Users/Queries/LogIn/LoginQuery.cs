using FluentValidation;
using MediatR;

namespace RssReader.Application.Behaviour.Users.Queries.LogIn;
 
public record LoginQuery(string Email, string Password) : IRequest;

internal class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.Password)
            .NotEmpty();
    }
}