using Microsoft.AspNetCore.Mvc;
using RssReader.Application.Common.Exceptions;
using RssReader.Application.Common.Exceptions.General;
using Serilog;

namespace RssReader.API.Common;

internal class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly Dictionary<Type, (int, string)> _exceptionCodes;

    public ExceptionHandlingMiddleware()
    {
        _exceptionCodes = new()
        {
            { typeof(EntityNotFoundException), (StatusCodes.Status404NotFound, "Entity not found") },
            { typeof(UnauthorizedException), (StatusCodes.Status401Unauthorized, "Unauthorized access") },
            { typeof(InvalidFeedUrlException), (StatusCodes.Status400BadRequest, "Feed URL is invalid")}
        };
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BaseException ex) when (_exceptionCodes.ContainsKey(ex.GetType()))
        {
            await HandleApiErrors(ex, context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationErrors(ex, context);
        }
        catch (Exception ex)
        {
            await HandleErrors(ex, context);
        }
    }

    private async Task HandleApiErrors(BaseException ex, HttpContext context)
    {
        ProblemDetails details = new()
        {
            Title = _exceptionCodes[ex.GetType()].Item2,
            Detail = ex.Message
        };

        context.Response.StatusCode = _exceptionCodes[ex.GetType()].Item1;
        await context.Response.WriteAsJsonAsync(details);
    }

    private async Task HandleValidationErrors(ValidationException ex, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsJsonAsync(
            new ValidationProblemDetails { Errors = ex.ErrorDetails });
    }

    private async Task HandleErrors(Exception ex, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(
            new ProblemDetails 
            { 
                Title = "Server error",
                Detail = "Something went wrong in the server" 
            });

        Log.Error(ex, ex is BaseException ? "MISSING_ERROR_CODE_FOR_APP_EXCEPTION" : "API_ERROR");
    }
}