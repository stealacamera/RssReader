using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Common.DTOs.Notifications;

public record UserCreatedNotification(int UserId) : INotification;

internal class UserCreatedNotificationValidator : Validator<UserCreatedNotification>
{
    public UserCreatedNotificationValidator()
    {
        RuleFor(e => e.UserId)
            .NotEmpty()
            .GreaterThan(0);
    }
}