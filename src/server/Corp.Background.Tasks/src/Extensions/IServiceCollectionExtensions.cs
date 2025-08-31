#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Services used by both foreground and background, these services are
    /// provided by the background task runner, but required by lower level
    /// services, so they're injected by interface.
    /// </summary>
    public static IServiceCollection AddTaskServices(this IServiceCollection serviceCollection)
    {
        // Integrations
        //serviceCollection.AddTransient<IGoogleAnalyticsIntegration, GoogleAnalyticsIntegration>();

        return serviceCollection;
    }
}
