using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs.Notifications;

namespace RssReader.Application.Behaviour.Events;

internal class SendEmailVerificationNotificationHandler : BaseHandler, INotificationHandler<UserCreatedNotification>
{
    private readonly IEmailService _emailService;

    public SendEmailVerificationNotificationHandler(
        IWorkUnit workUnit,
        IEmailService emailService)
        : base(workUnit)
    {
        _emailService = emailService;
    }

    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        await new UserCreatedNotificationValidator().ValidateAndThrowAsync(notification, cancellationToken);

        // Verify user
        var user = await _workUnit.UsersRepository
                                  .GetByIdAsync(notification.UserId);

        if (user == null || user.IsEmailConfirmed)
            return;

        var otp = await UpsertOTPAsync(user.Id, cancellationToken);
        await _emailService.SendEmailVerificationEmailAsync(user.Email, otp.Password);
    }

    private async Task<Domain.Entities.Identity.OTP> UpsertOTPAsync(int userId, CancellationToken cancellationToken)
    {
        var otp = await _workUnit.OTPsRepository
                                 .GetByUserIdAsync(userId, cancellationToken) ??
                                 new Domain.Entities.Identity.OTP();

        otp.Password = _workUnit.OTPsRepository.GenerateOTP();
        otp.ExpiryDate = DateTime.UtcNow.AddMinutes(5);
        otp.RetryAttempts = 0;

        if (otp.UserId <= 0)
        {
            otp.UserId = userId;
            await _workUnit.OTPsRepository.AddAsync(otp, cancellationToken);
        }
                
        await _workUnit.SaveChangesAsync();
        return otp;
    }
}
