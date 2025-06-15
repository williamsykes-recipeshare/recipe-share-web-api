using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareWebApi.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;
using ForbiddenException = RecipeShareWebApi.CustomExceptions.ForbiddenException;

namespace RecipeShareWebApi.Middleware;

public class ExceptionMiddleware(RequestDelegate _)
{
    public async Task Invoke(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();

        if (exceptionDetails == null)
            return;

        var ex = exceptionDetails.Error;

        switch (ex)
        {
            case BadRequestException:
                logger.LogDebug(ex, "{Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
                break;
            case NotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
                await context.Response.CompleteAsync();
                break;
            case UnauthorisedException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
                await context.Response.CompleteAsync();
                break;
            case ForbiddenException:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
                break;
            case OperationCanceledException:
                context.Response.StatusCode = 499;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
                break;
            default:
                logger.LogError(ex, "{Path}", context.Request.Path);
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal Server Error");
                break;
        }
    }
}