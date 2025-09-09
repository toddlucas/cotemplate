namespace Corp.Data.Mock;

public static class CorpDbContextContainerExtensions
{
    /// <summary>
    /// Create a DI container and add app services, with a test database.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    /// <remarks>
    /// The test DB context will be injected in such a way that a distinct
    /// context will be created within each scope, but it will maintain a
    /// connection to the in-memory Sqlite database so the database is coherent
    /// across scopes.
    /// </remarks>
    public static async Task<ServiceProvider> AddDbServicesAsync(this CorpDbContextContainer container, Action<IServiceCollection>? callback = null)
    {
        await container.CreateAsync(DatabaseNames.Corp, DatabaseNames.MyOther);

        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped(typeof(CorpDbContext), (sp) =>
        {
            // Create a new DbContext for this scope, using the same connection.
            return container.DatabaseSet!.CreateContext<CorpDbContext>(DatabaseNames.Corp);
        });

        serviceCollection.AddScoped(typeof(MyOtherDbContext), (sp) =>
        {
            return container.DatabaseSet!.CreateContext<MyOtherDbContext>(DatabaseNames.MyOther);
        });

#if false
        // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model
        serviceCollection
            .AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<CorpDbContext>();

        serviceCollection.AddTestConfiguration();
        serviceCollection.AddCoreAppServices();

        // AddScoped<UserManager<>, TenantUserManager<ApplicationUser, ApplicationTenant>>();
        var userType = typeof(ApplicationUser);
        var userManagerType = typeof(UserManager<>).MakeGenericType(userType);
        var customType = typeof(TenantUserManager<ApplicationUser, IdentityRole, ApplicationTenant, AppContext, string>);

        serviceCollection.AddScoped(userManagerType, customType);
        serviceCollection.AddScoped<TenantContext<string>>();

        var identityBuilder = serviceCollection
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CorpDbContext>();
#endif

        // Do additional test services need to be added?
        callback?.Invoke(serviceCollection);

        return serviceCollection.BuildServiceProvider();
    }

    private static void AddTestConfiguration(this IServiceCollection services)
    {
        // Load a test version of appsettings.
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        //services.AddOptions<PlatformSettings>()
        //    .Bind(configuration.GetSection(nameof(PlatformSettings)))
        //    .ValidateDataAnnotations();
    }

    public static Task<ServiceProvider> AddTestDbServicesAsync(this CorpDbContextContainer container)
        => container.AddDbServicesAsync(AddTestServices);

    public static Task<ServiceProvider> AddTestDbServicesAsync(this CorpDbContextContainer container, FakeTimeProvider fakeTimeProvider)
        => container.AddDbServicesAsync((services) => AddTestServices(services, fakeTimeProvider));

    private static void AddTestServices(IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
    }

    private static void AddTestServices(IServiceCollection services, FakeTimeProvider fakeTimeProvider)
    {
        services.AddSingleton<TimeProvider>(fakeTimeProvider);
    }
}
