﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Quartz;
using RssReader.API.Common.Quartz.Jobs;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace RssReader.API.Common;

internal static class StartupUtils
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

    public static void RegisterSwagger(SwaggerGenOptions options)
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
