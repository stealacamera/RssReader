using MediatR;
using Quartz;
using RssReader.Application.Common.DTOs.Notifications;

namespace RssReader.API.Common.QuartzJobs;

[DisallowConcurrentExecution]
internal class RemoveExpiredOTPsJob : IJob
{
    private readonly IPublisher _publisher;

    public RemoveExpiredOTPsJob(IPublisher publisher)
        => _publisher = publisher;

    public async Task Execute(IJobExecutionContext context)
        => await _publisher.Publish(new RemoveExpiredOTPsNotification());
}
