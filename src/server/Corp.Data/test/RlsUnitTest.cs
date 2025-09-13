using Microsoft.EntityFrameworkCore.Storage;

using Corp.Access;
using Corp.Data.Identity;
using Corp.Identity;

namespace Corp.Data.Test;

// RLS Behavior
//
// Read Operations (SELECT)
// * RLS filters rows - returns empty result set if no rows match the policy
// * No exceptions thrown - this is normal PostgreSQL behavior
// * Silent filtering - the query succeeds but returns no data
//
// Write Operations (INSERT/UPDATE/DELETE)
// * RLS filters rows - only affects rows that match the policy
// * INSERT: New rows are inserted normally (RLS doesn't prevent new inserts)
// * UPDATE/DELETE: Only affects rows that match the policy
// * No exceptions thrown - operations succeed but may affect 0 rows

public class UnitTest2
{
    [Fact]
    public async Task Read_WithGuard_ShouldSucceed()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddDatabases(configuration);
        services.AddTenantServices();
        var sp = services.BuildServiceProvider();
        long personId = 1;
        await using (var scope = sp.CreateAsyncScope())
        {
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();
            IRequestDbGuard requestGuard = GetRequestGuard(scope.ServiceProvider);
            await requestGuard.EnsureWriteAsync();

            var person = new Person() { TenantId = IdentitySeedData.TenantId, GivenName = "Jack", FamilyName = "Squat" };
            corpDb.People.Add(person);
            await corpDb.SaveChangesAsync();

            person.Id.Should().BeGreaterThan(0);
            personId = person.Id;
        }

        await using (var scope = sp.CreateAsyncScope())
        {
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();
            IRequestDbGuard requestGuard = GetRequestGuard(scope.ServiceProvider);
            await requestGuard.EnsureReadAsync();

            // Act
            var person = await corpDb.People.Where(p => p.Id == personId).FirstOrDefaultAsync();

            // Assert
            person.Should().NotBeNull();

            // Cleanup
            corpDb.People.Remove(person);
            await corpDb.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task ReadOtherTenant_WithGuard_ShouldFail()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddDatabases(configuration);
        services.AddTenantServices();
        var sp = services.BuildServiceProvider();
        long personId = 1;
        await using (var scope = sp.CreateAsyncScope())
        {
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();
            IRequestDbGuard requestGuard = GetRequestGuard(scope.ServiceProvider);
            await requestGuard.EnsureWriteAsync();

            var person = new Person() { TenantId = IdentitySeedData.TenantId, GivenName = "Jack", FamilyName = "Squat" };
            corpDb.People.Add(person);
            await corpDb.SaveChangesAsync();

            person.Id.Should().BeGreaterThan(0);
            personId = person.Id;
        }

        await using (var scope = sp.CreateAsyncScope())
        {
            // Other tenant
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();
            IRequestDbGuard requestGuard = GetOtherRequestGuard(scope.ServiceProvider);
            await requestGuard.EnsureReadAsync();

            // Act
            var person = await corpDb.People.Where(p => p.Id == personId).FirstOrDefaultAsync();

            // Assert
            person.Should().BeNull();
        }

        await using (var scope = sp.CreateAsyncScope())
        {
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();
            IRequestDbGuard requestGuard = GetRequestGuard(scope.ServiceProvider);
            await requestGuard.EnsureReadAsync();

            var person = await corpDb.People.Where(p => p.Id == personId).FirstOrDefaultAsync();

            // Cleanup
            corpDb.People.Remove(person);
            await corpDb.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task ReadRls_WithMissingTenant_ShouldFail()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddDatabases(configuration);
        services.AddTenantServices();
        var sp = services.BuildServiceProvider();
        long personId = 1;
        await using (var scope = sp.CreateAsyncScope())
        {
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();
            IRequestDbGuard requestGuard = GetRequestGuard(scope.ServiceProvider);
            await requestGuard.EnsureWriteAsync();

            var person = new Person() { TenantId = IdentitySeedData.TenantId, GivenName = "Jack", FamilyName = "Squat" };
            corpDb.People.Add(person);
            await corpDb.SaveChangesAsync();

            person.Id.Should().BeGreaterThan(0);
            personId = person.Id;
        }

        await using (var scope = sp.CreateAsyncScope())
        {
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();

            // Arrange the RLS config using lower level API (bypassing param checks).
            var connection = corpDb.Database.GetDbConnection();
            var cmd = connection.CreateCommand();
            var transaction = await corpDb.Database.BeginTransactionAsync();
            cmd.Transaction = transaction.GetDbTransaction();

            // Only configure group--not tenant.
            RlsConfig.SetGroupConfig(cmd, IdentitySeedData.GroupId);
            await cmd.ExecuteScalarAsync();

            // Act
            var person = await corpDb.People.Where(p => p.Id == personId).FirstOrDefaultAsync();

            // Assert
            person.Should().BeNull();
        }

        await using (var scope = sp.CreateAsyncScope())
        {
            var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();
            IRequestDbGuard requestGuard = GetRequestGuard(scope.ServiceProvider);
            await requestGuard.EnsureReadAsync();

            var person = await corpDb.People.Where(p => p.Id == personId).FirstOrDefaultAsync();

            // Cleanup
            corpDb.People.Remove(person);
            await corpDb.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Tests the degenerate scenario with the current RLS design approach.
    /// </summary>
    /// <remarks>
    /// NOTE: This is a MAJOR antipattern.
    /// </remarks>
    [Fact]
    public async Task ReadAntipattern_WithNoGuard_ShouldSucceed()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddDatabases(configuration);
        services.AddTenantServices();
        var sp = services.BuildServiceProvider();

        var corpDb = sp.GetRequiredService<CorpDbContext>();

        // Act - This query works, but should be blocked with no tenant.
        var person = await corpDb.People.Where(p => p.Id == -1).FirstOrDefaultAsync();
        person.Should().BeNull();
    }

    [Fact]
    public async Task TenantContext_WithoutGroup_ShouldFail()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddDatabases(configuration);
        services.AddTenantServices();
        var sp = services.BuildServiceProvider();

        var tenantContext = sp.GetRequiredService<TenantContext<Guid>>();
        tenantContext.CurrentId = IdentitySeedData.TenantId;

        var requestGuard = sp.GetRequiredService<IRequestDbGuard>();

        // Act
        Func<Task> act = () => requestGuard.EnsureWriteAsync();

        // Assert
        (await act.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("No group ID available for this request.");
    }

    [Fact]
    public async Task TenantContext_WithoutTenant_ShouldFail()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddDatabases(configuration);
        services.AddTenantServices();
        var sp = services.BuildServiceProvider();

        var tenantContext = sp.GetRequiredService<TenantContext<Guid>>();
        tenantContext.CurrentGroupId = IdentitySeedData.GroupId;

        var requestGuard = sp.GetRequiredService<IRequestDbGuard>();

        // Act
        Func<Task> act = () => requestGuard.EnsureWriteAsync();

        // Assert
        (await act.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("No tenant ID available for this request.");
    }

    private IRequestDbGuard GetRequestGuard(IServiceProvider serviceProvider)
    {
        var tenantContext = serviceProvider.GetRequiredService<TenantContext<Guid>>();
        tenantContext.CurrentGroupId = IdentitySeedData.GroupId;
        tenantContext.CurrentId = IdentitySeedData.TenantId;
        return serviceProvider.GetRequiredService<IRequestDbGuard>();
    }

    private IRequestDbGuard GetOtherRequestGuard(IServiceProvider serviceProvider)
    {
        var tenantContext = serviceProvider.GetRequiredService<TenantContext<Guid>>();
        tenantContext.CurrentGroupId = IdentitySeedData.GroupId;
        tenantContext.CurrentId = IdentitySeedData.OtherTenantId;
        return serviceProvider.GetRequiredService<IRequestDbGuard>();
    }
}
