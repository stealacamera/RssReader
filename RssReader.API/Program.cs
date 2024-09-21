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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(StartupUtils.RegisterSwagger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(StartupUtils.CorsPolicyName);

app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();