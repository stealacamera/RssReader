using FluentValidation;
using MediatR;

namespace RssReader.Application.Common.DTOs.Notifications;

public record UserCreatedNotification(int UserId) : INotification;

internal class UserCreatedNotificationValidator : AbstractValidator<UserCreatedNotification>
{
    public UserCreatedNotificationValidator()
    {
        RuleFor(e => e.UserId)
            .NotEmpty()
            .GreaterThan(0);
    }
}