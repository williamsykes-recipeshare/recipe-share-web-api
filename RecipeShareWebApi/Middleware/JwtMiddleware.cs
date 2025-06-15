using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareWebApi.CustomExceptions;
using RecipeShareWebApi.Services.Rights;

namespace RecipeShareWebApi.Middleware;

public class JwtMiddleware(RequestDelegate next, CancellationToken cancellationToken = default)
{
    public async Task InvokeAsync(
        HttpContext context,
        IAuthorisationService authorisationService,
        ILogger<JwtMiddleware> loggingManager)
    {
        var authType = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").FirstOrDefault();
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();

        if (authType != "Bearer" || token == null)
        {
            await next(context);
            return;
        }

        try
        {
            if (context.User.Claims == null) throw new UnauthorisedException("Invalid Token.");

            var userSession = await authorisationService.GetSessionAsync(cancellationToken);

            // Validate token
            if (userSession == null || !userSession.Token.Equals(token) || userSession.User == null || userSession.IsActive != true)
            {
                throw new UnauthorisedException("Invalid Token.");
            }

            // Add user to context for later use.
            context.Items["User"] = userSession.User;
        }
        catch (UnauthorisedException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized", cancellationToken: cancellationToken);
            return;
        }
        catch (InvalidTokenException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized", cancellationToken: cancellationToken);
            return;
        }
        catch (Exception ex)
        {
            loggingManager.LogTrace(ex, "JWT Middleware - {RequestPath}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Internal Server Error", cancellationToken: cancellationToken);
            return;
        }

        await next(context);
    }
}