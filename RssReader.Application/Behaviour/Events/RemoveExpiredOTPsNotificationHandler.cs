using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs.Notifications;

namespace RssReader.Application.Behaviour.Events;

internal class RemoveExpiredOTPsNotificationHandler : BaseHandler, INotificationHandler<RemoveExpiredOTPsNotification>
{
    public RemoveExpiredOTPsNotificationHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(RemoveExpiredOTPsNotification notification, CancellationToken cancellationToken)
    {
        await _workUnit.OTPsRepository.DeleteAllExpiredAsync(cancellationToken);
        await _workUnit.SaveChangesAsync();
    }
}
