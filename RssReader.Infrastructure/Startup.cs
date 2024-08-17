using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RssReader.Application.Abstractions;
using RssReader.Infrastructure.Authentication;
using RssReader.Infrastructure.Email;
using RssReader.Infrastructure.Options;
using RssReader.Infrastructure.Options.Setups;

namespace RssReader.Infrastructure;

public static class Startup
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ApiDbConnection")));

        services.AddAuthentication(e =>
                {
                    e.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer();

        services.AddAuthorization();

        RegisterOptions(services);
        RegisterEmail(services, configuration);

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IWorkUnit, WorkUnit>();
        services.AddTransient<IEmailService, EmailService>();
    }

    private static void RegisterEmail(IServiceCollection services, IConfiguration configuration)
    {
        EmailOptions emailOptions = new();
        configuration.GetSection(EmailOptions.SectionName).Bind(emailOptions);

        services.AddFluentEmail(emailOptions.Email)
                .AddRazorRenderer()
                .AddSmtpSender(emailOptions.Host, emailOptions.Port, emailOptions.Name, emailOptions.Password);
    }

    private static void RegisterOptions(IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<EmailOptionsSetup>();
    }
}
