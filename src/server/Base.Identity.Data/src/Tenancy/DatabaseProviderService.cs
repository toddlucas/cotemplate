using Microsoft.Extensions.Configuration;

namespace Corp.Data.Identity;

/// <summary>
/// Configuration-based implementation of IDatabaseProviderService.
/// Reads database provider information from application configuration.
/// </summary>
public sealed class DatabaseProviderService : IDatabaseProviderService
{
    private readonly IConfiguration _configuration;
    private readonly string _providerName;

    public DatabaseProviderService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _providerName = _configuration.GetDatabaseProvider("CorpDb", "Npgsql");
    }

    /// <summary>
    /// The name of the current database provider (e.g., "Npgsql", "Sqlite").
    /// </summary>
    public string ProviderName => _providerName;

    /// <summary>
    /// Indicates whether the current provider is PostgreSQL.
    /// </summary>
    public bool IsPostgreSQL => _providerName == "Npgsql";

    /// <summary>
    /// Indicates whether the current provider is SQLite.
    /// </summary>
    public bool IsSQLite => _providerName == "Sqlite";

    /// <summary>
    /// Indicates whether the current provider supports Row Level Security.
    /// Currently only PostgreSQL supports RLS.
    /// </summary>
    public bool SupportsRowLevelSecurity => IsPostgreSQL;

    /// <summary>
    /// Indicates whether the current provider supports tenant context configuration.
    /// Currently only PostgreSQL supports setting tenant context on connections.
    /// </summary>
    public bool SupportsTenantContext => IsPostgreSQL;
}
