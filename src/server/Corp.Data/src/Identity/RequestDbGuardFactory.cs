namespace Corp.Data.Identity;

/// <summary>
/// Factory implementation that creates the appropriate IRequestDbGuard based on
/// the database provider and configuration.
/// </summary>
/// <remarks>
/// This factory is only used to break circular dependencies, within AddDbContext.
/// </remarks>
public sealed class RequestDbGuardFactory : IRequestDbGuardFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public RequestDbGuardFactory(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public IRequestDbGuard CreateGuard()
    {
        bool useInterceptor = _configuration.UseTenantInterceptor();
        string provider = _configuration.GetDatabaseProvider("CorpDb", "Npgsql");

        if (useInterceptor && provider == "Npgsql")
        {
            // PostgreSQL: Full tenant context with RLS support
            var dbContext = _serviceProvider.GetRequiredService<CorpDbContext>();
            var tenantContext = _serviceProvider.GetRequiredService<TenantContext<string>>();
            return new RequestDbGuard<CorpDbContext>(dbContext, tenantContext);
        }
        else
        {
            // SQLite or other providers: Passthrough implementation
            return new PassthroughRequestDbGuard();
        }
    }
}
