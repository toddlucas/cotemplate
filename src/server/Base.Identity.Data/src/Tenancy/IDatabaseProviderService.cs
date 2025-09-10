namespace Corp.Data.Identity;

/// <summary>
/// Service that provides database provider information and capabilities.
/// This abstraction allows for different implementations based on environment,
/// configuration, or runtime detection.
/// </summary>
public interface IDatabaseProviderService
{
    /// <summary>
    /// The name of the current database provider (e.g., "Npgsql", "Sqlite").
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Indicates whether the current provider is PostgreSQL.
    /// PostgreSQL supports advanced features like Row Level Security and tenant context.
    /// </summary>
    bool IsPostgreSQL { get; }

    /// <summary>
    /// Indicates whether the current provider is SQLite.
    /// SQLite has limited support for advanced tenant features.
    /// </summary>
    bool IsSQLite { get; }

    /// <summary>
    /// Indicates whether the current provider supports Row Level Security.
    /// Currently only PostgreSQL supports RLS.
    /// </summary>
    bool SupportsRowLevelSecurity { get; }

    /// <summary>
    /// Indicates whether the current provider supports tenant context configuration.
    /// Currently only PostgreSQL supports setting tenant context on connections.
    /// </summary>
    bool SupportsTenantContext { get; }
}
