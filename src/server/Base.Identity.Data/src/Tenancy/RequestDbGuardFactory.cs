using Microsoft.Extensions.DependencyInjection;

using Corp.Identity;

namespace Corp.Data.Identity;

/// <summary>
/// Factory implementation that creates the appropriate IRequestDbGuard based on
/// feature flags and database provider capabilities.
/// </summary>
/// <remarks>
/// This factory uses feature services to determine the appropriate guard implementation,
/// removing direct dependency on configuration and making it more testable and flexible.
/// </remarks>
public sealed class RequestDbGuardFactory : IRequestDbGuardFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITenantFeatureService _featureService;
    private readonly IDatabaseProviderService _providerService;

    public RequestDbGuardFactory(
        IServiceProvider serviceProvider,
        ITenantFeatureService featureService,
        IDatabaseProviderService providerService)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
        _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
    }

    public IRequestDbGuard CreateGuard()
    {
        if (_featureService.IsTenantContextEnabled && _providerService.SupportsTenantContext)
        {
            // PostgreSQL with tenant context: Full tenant context with RLS support
            // Note: The actual DbContext type will be resolved by the consuming application
            // This factory creates a generic guard that can work with any DbContext
            var tenantContext = _serviceProvider.GetRequiredService<TenantContext<string>>();

            // We need to get the DbContext from the service provider
            // The consuming application will register the specific DbContext type
            var dbContext = _serviceProvider.GetService<Microsoft.EntityFrameworkCore.DbContext>();
            if (dbContext == null)
            {
                throw new InvalidOperationException("No DbContext registered in the service provider. Please ensure a DbContext is registered before using the RequestDbGuardFactory.");
            }

            // Create a generic RequestDbGuard using reflection or a factory method
            return CreateRequestDbGuard(dbContext, tenantContext);
        }
        else
        {
            // SQLite or other providers: Passthrough implementation
            return new PassthroughRequestDbGuard();
        }
    }

    private static IRequestDbGuard CreateRequestDbGuard(Microsoft.EntityFrameworkCore.DbContext dbContext, TenantContext<string> tenantContext)
    {
        // Use reflection to create the generic RequestDbGuard<T> type
        var dbContextType = dbContext.GetType();
        var requestDbGuardType = typeof(RequestDbGuard<>).MakeGenericType(dbContextType);
        return (IRequestDbGuard)Activator.CreateInstance(requestDbGuardType, dbContext, tenantContext)!;
    }
}
