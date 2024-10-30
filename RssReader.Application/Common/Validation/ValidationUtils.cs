using FluentValidation;
using MediatR;
using System.Text.RegularExpressions;

namespace RssReader.Application.Common.Validation;

internal static class ValidationUtils
{
    public static int UsernameMaxLength = 100;
    public static int PasswordMaxLength = 30, PasswordMinLength = 8;

    public static IRuleBuilder<T, string> ValidPasswordCharacters<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(str =>
                            {
                                Regex allowedCharacters = new(@"^(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).*$");
                                return allowedCharacters.IsMatch(str);
                            })
                          .WithMessage("Passwords should consist of letters, numbers, and symbols");
    }

    public static IRuleBuilder<T, string> ValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(url =>
                            {
                                // Validate url format
                                Uri uri;

                                if (!Uri.TryCreate(url, UriKind.Absolute, out uri) || 
                                    !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                                    return false;

                                return true;
                            })
                          .WithMessage("URL must be of a valid form");
    }
}