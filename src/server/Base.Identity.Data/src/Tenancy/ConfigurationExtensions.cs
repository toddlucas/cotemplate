using Microsoft.Extensions.Configuration;

namespace Corp.Data.Identity;

/// <summary>
/// Extension methods for IConfiguration to support database provider detection.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets the database provider from configuration.
    /// </summary>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="configurationPrefix">The configuration prefix (e.g., "CorpDb").</param>
    /// <param name="defaultProvider">The default provider if not specified.</param>
    /// <returns>The database provider name.</returns>
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
