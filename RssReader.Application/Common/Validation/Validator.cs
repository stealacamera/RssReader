using FluentValidation;
using FluentValidation.Results;

namespace RssReader.Application.Common.Validation;

internal abstract class Validator<T> : AbstractValidator<T>
{
    protected override void RaiseValidationException(ValidationContext<T> context, ValidationResult result)
        => throw new Exceptions.General.ValidationException(result.ToDictionary());
}
