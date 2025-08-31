using Corp.Data;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class IServiceCollectionExtensions
{
    private const string _npgsqlAssembly = "Corp.Data.Npgsql";
    private const string _sqliteAssembly = "Corp.Data.Sqlite";

    public static IServiceCollection AddDatabases(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddCorpDbConfiguration(configuration);
        serviceCollection.AddMyOtherDbConfiguration(configuration);

        return serviceCollection;
    }

    /// <summary>
    /// Add the app DbContext to the container.
    /// </summary>
    public static IServiceCollection AddCorpDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services.AddProviderDbContext<CorpDbContext>(configuration, "CorpDb", "Npgsql");

    /// <summary>
    /// Add the data DbContext to the container.
    /// </summary>
    public static IServiceCollection AddMyOtherDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services.AddProviderDbContext<MyOtherDbContext>(configuration, "MyOtherDb", "Npgsql");

    public static IServiceCollection AddProviderDbContext<TContext>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        string configurationPrefix,
        string defaultProvider,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext
    {
        string provider = configuration.GetDatabaseProvider(configurationPrefix, defaultProvider);
        string connectionString = GetConnectionString(configuration, configurationPrefix, provider);

        serviceCollection.AddDbContext<TContext>(options =>
        {
            if (provider == "Sqlite")
            {
                options = options.UseSqlite(
                    connectionString,
                    o =>
                    {
                        o.MigrationsAssembly(_sqliteAssembly);
                        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
            }
            else if (provider == "Npgsql")
            {
                options = options.UseNpgsql(
                    connectionString,
                    o =>
                    {
                        o.MigrationsAssembly(_npgsqlAssembly);
                        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
            }
            else
            {
                throw new Exception($"Configuration not found for {provider} provider.");
            }

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        },
        contextLifetime,
        optionsLifetime);

        return serviceCollection;
    }

    private static string GetConnectionString(IConfiguration configurationManager, string configurationPrefix, string provider)
    {
        var connectionString = configurationManager.GetConnectionString(
            provider + configurationPrefix + "ContextConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception($"Connection string not found for {provider}.");
        }

        return connectionString;
    }
}
