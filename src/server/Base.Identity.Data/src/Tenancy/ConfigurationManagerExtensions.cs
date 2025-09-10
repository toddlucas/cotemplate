#pragma warning disable IDE0130 // Namespace does not match folder structure
using Microsoft.Extensions.Configuration;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class ConfigurationManagerExtensions
{
    public static bool UseTenantInterceptor(
        this IConfiguration configuration)
        => configuration.GetValue("UseTenantInterceptor", false);
}
