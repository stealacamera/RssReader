using Carter;
using RssReader.API.Common;
using RssReader.Application;
using RssReader.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterCors();
builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastructureServices(builder.Configuration);

builder.Services.AddCarter();
builder.Services.RegisterQuartzServices();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

Log.Logger = new LoggerConfiguration().ReadFrom
                                      .Configuration(builder.Configuration)
                                      .CreateLogger();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(RssReader.API.Common.Startup.CorsPolicyName);

app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();


// TODO fix emailing