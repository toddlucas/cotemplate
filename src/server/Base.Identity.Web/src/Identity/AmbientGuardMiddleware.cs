using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Base.Data.Identity;

namespace Base.Web.Identity;

/// <summary>
/// ASP.NET Core middleware that sets up the ambient database guard for each request.
/// This enables Metalama aspects to access the guard without dependency injection.
/// </summary>
public sealed class AmbientGuardMiddleware
{
    private readonly RequestDelegate _next;

    public AmbientGuardMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware to set up the ambient guard for the current request.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        // Get the database guard from the request services
        var guard = context.RequestServices.GetRequiredService<IRequestDbGuard>();

        // Set the ambient guard for this request
        using (AmbientRequestGuard.Use(guard))
        {
            // Continue to the next middleware/controller
            await _next(context);
        }

        // The guard will be disposed automatically when the request scope ends
    }
}
