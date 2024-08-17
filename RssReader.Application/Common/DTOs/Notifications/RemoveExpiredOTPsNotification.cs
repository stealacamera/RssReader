using MediatR;

namespace RssReader.Application.Common.DTOs.Notifications;

public record RemoveExpiredOTPsNotification : INotification;