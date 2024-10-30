using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Quartz;
using RssReader.API.Common.Authorization;
using RssReader.API.Common.QuartzJobs;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace RssReader.API.Common;

internal static class StartupUtils
{
    public static string CorsPolicyName = "RssReaderCors";

    public static void RegisterSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom
                                              .Configuration(configuration)
                                              .CreateLogger();

        builder.Logging.AddSerilog(Log.Logger);
        builder.Host.UseSerilog();
    }

    public static void RegisterPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicyName,
                              policy => policy.WithOrigins("http://localhost:4200")
                                              .AllowAnyHeader()
                                              .AllowAnyMethod());
        });

        services.AddCarter();
        RegisterQuartzServices(services);
        services.AddSwaggerGen(RegisterSwagger);

        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void RegisterQuartzServices(IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();

            var expiredOTPsKey = JobKey.Create(nameof(RemoveExpiredOTPsJob));

            options.AddJob<RemoveExpiredOTPsJob>(expiredOTPsKey)
                   .AddTrigger(trigger =>
                        trigger.ForJob(expiredOTPsKey)
                               .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Wednesday, 1, 0)));

            var repullFeedsKey = JobKey.Create(nameof(PullFeedsJob));

            options.AddJob<PullFeedsJob>(repullFeedsKey)
                   .AddTrigger(trigger => 
                        trigger.ForJob(repullFeedsKey)
                               .WithSimpleSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(30)));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }

    private static void RegisterSwagger(SwaggerGenOptions options)
    {
        // Add JWT authentication
        options.AddSecurityDefinition(
            JwtBearerDefaults.AuthenticationScheme,
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Description = "JWT authentication (no need to add 'Bearer' in the beginning)",
            });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme }},
                new string[] {}
            }
        });

        // Add XML comments to documentation
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
}
