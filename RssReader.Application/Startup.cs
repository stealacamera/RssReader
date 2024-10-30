using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RssReader.Application;

public static class Startup
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddMediatR(
            configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
