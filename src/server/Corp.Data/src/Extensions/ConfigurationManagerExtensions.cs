#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class ConfigurationManagerExtensions
{
    public static string GetDatabaseProvider(
        this IConfiguration configuration,
        string configurationPrefix,
        string defaultProvider)
    {
        var provider = configuration.GetValue(configurationPrefix + "Provider", defaultProvider);
        if (provider == null)
            throw new Exception("Provider entry not found");

        return provider;
    }
}
