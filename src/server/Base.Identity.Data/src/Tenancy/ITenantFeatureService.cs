namespace Corp.Data.Identity;

/// <summary>
/// Service that provides tenant-related feature flags and configuration.
/// This abstraction allows for different implementations based on environment,
/// configuration, or application-specific requirements.
/// </summary>
public interface ITenantFeatureService
{
    /// <summary>
    /// Indicates whether tenant context management is enabled.
    /// When enabled, tenant ID will be set on database connections.
    /// </summary>
    bool IsTenantContextEnabled { get; }

    /// <summary>
    /// Indicates whether Row Level Security (RLS) is enabled.
    /// When enabled, PostgreSQL RLS policies will be enforced.
    /// </summary>
    bool IsRowLevelSecurityEnabled { get; }

    /// <summary>
    /// Indicates whether write guard functionality is enabled.
    /// When enabled, write operations will be wrapped in transactions with tenant context.
    /// </summary>
    bool IsWriteGuardEnabled { get; }
}
