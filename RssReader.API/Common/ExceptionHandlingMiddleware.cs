using Microsoft.AspNetCore.Mvc;
using RssReader.Application.Common.Exceptions;
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
            { typeof(ExistingEmailException),  (StatusCodes.Status400BadRequest, "Existing email") },
            { typeof(ExpiredRefreshTokenException), (StatusCodes.Status401Unauthorized, "Expired refresh token") },
            { typeof(FailedPasswordVerification), (StatusCodes.Status400BadRequest, "Failed password verification") },
            { typeof(InvalidOTPException), (StatusCodes.Status400BadRequest, "Invalid OTP") },
            { typeof(UnauthorizedException), (StatusCodes.Status403Forbidden, "Unauthorized access") },
            { typeof(UnconfirmedEmailException), (StatusCodes.Status403Forbidden, "Unconfirmed email") }
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
            ProblemDetails details = new() { 
                Title = _exceptionCodes[ex.GetType()].Item2,
                Detail = ex.Message 
            };

            context.Response.StatusCode = _exceptionCodes[ex.GetType()].Item1;
            await context.Response.WriteAsJsonAsync(details);
        }
        catch (ValidationException ex)
        {
            ValidationProblemDetails details = new() { Errors = ex.ErrorDetails };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(details);
        }

        catch (Exception ex)
        {
            ProblemDetails details = new() { Detail = "Something went wrong in the server" };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(details);

            Log.Error(
                ex,
                ex is BaseException ? "MISSING_ERROR_CODE_FOR_APP_EXCEPTION" : "API_ERROR");
        }
    }
}
