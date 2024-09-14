using Quartz;
using RssReader.API.Common.Quartz.Jobs;

namespace RssReader.API.Common;

internal static class Startup
{
    public static string CorsPolicyName = "RssReaderCors";

    public static void RegisterCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicyName,
                              policy => policy.WithOrigins("http://localhost:4200")
                                              .AllowAnyHeader()
                                              .AllowAnyMethod());
        });
    }

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
