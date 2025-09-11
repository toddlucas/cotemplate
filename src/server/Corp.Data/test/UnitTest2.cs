using Corp.Access;
using Corp.Data.Mock;
using Corp.Identity;

namespace Corp.Data.Test;

public class UnitTest2
{
    [Fact]
    public async Task Test1()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddDatabases(configuration);
        var sp = services.BuildServiceProvider();
        var corpDb = sp.GetRequiredService<CorpDbContext>();

        // using var scope = services.CreateScope();

        var profile = new Person() { TenantId = IdentitySeedData.TenantId, GivenName = "Jack", FamilyName = "Squat" };
        corpDb.People.Add(profile);
        await corpDb.SaveChangesAsync();

        profile.Id.Should().BeGreaterThan(0);
    }
}
