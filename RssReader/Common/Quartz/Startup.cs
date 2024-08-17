using Quartz;
using RssReader.API.Common.Quartz.Jobs;

namespace RssReader.API.Common.Quartz;

public static class Startup
{
    public static void RegisterQuartzServices(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var expiredOTPsKey = JobKey.Create(nameof(RemoveExpiredOTPsJob));
            
            options.AddJob<RemoveExpiredOTPsJob>(expiredOTPsKey)
                   .AddTrigger(trigger =>
                        trigger.ForJob(expiredOTPsKey)
                               .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Wednesday, 1, 0)));
        });

        services.AddQuartzHostedService();
    }
}
