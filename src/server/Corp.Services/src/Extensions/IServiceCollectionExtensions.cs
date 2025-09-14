#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        // Identity
        //serviceCollection.AddScoped<ProfileService>();

        // Access
        serviceCollection.AddScoped<Corp.Access.OrganizationService>();

        return serviceCollection;
    }
}
