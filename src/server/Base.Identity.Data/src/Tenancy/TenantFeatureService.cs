using Microsoft.Extensions.Configuration;

namespace Corp.Data.Identity;

/// <summary>
/// Configuration-based implementation of ITenantFeatureService.
/// Reads feature flags from application configuration.
/// </summary>
public sealed class TenantFeatureService : ITenantFeatureService
{
    private readonly IConfiguration _configuration;

    public TenantFeatureService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Indicates whether tenant context management is enabled.
    /// Reads from "UseTenantInterceptor" configuration value, defaults to false.
    /// </summary>
    public bool IsTenantContextEnabled => _configuration.GetValue("UseTenantInterceptor", false);

    /// <summary>
    /// Indicates whether Row Level Security (RLS) is enabled.
    /// Currently uses the same configuration as tenant context.
    /// </summary>
    public bool IsRowLevelSecurityEnabled => _configuration.GetValue("UseTenantInterceptor", false);

    /// <summary>
    /// Indicates whether write guard functionality is enabled.
    /// Currently uses the same configuration as tenant context.
    /// </summary>
    public bool IsWriteGuardEnabled => _configuration.GetValue("UseTenantInterceptor", false);
}
