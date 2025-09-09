using Corp.Data.Identity;
using Corp.Identity;
using Corp.Identity.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ITenantServiceCollectionExtensions
{
    public static IServiceCollection AddTenantServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(TimeProvider.System);
        //serviceCollection.AddCoreVacayServices();

        // Identity
        //serviceCollection.AddTransient<TenantManager>();
        serviceCollection.AddTransient<ITenantResolver, TenantResolver>();
        serviceCollection.AddScoped<TenantContext<string>>();

        return serviceCollection;
    }
}
