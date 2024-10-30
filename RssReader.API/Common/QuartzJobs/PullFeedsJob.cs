using MediatR;
using Quartz;
using RssReader.Application.Behaviour.Operations.Feeds.Commands.PullAll;

namespace RssReader.API.Common.QuartzJobs;

[DisallowConcurrentExecution]
internal class PullFeedsJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public PullFeedsJob(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var sender = scope.ServiceProvider.GetService<ISender>()!;
            await sender.Send(new PullFeedsCommand());
        }
    }

    // TODO make sure to try refreshing from two different user browsers
}
