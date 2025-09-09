using Corp.Access;
using Corp.Data.Mock;

namespace Corp.Data.Test;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        using var container = new CorpDbContextContainer();
        var services = await container.AddTestDbServicesAsync();

        using var scope = services.CreateScope();

        var corpDb = scope.ServiceProvider.GetRequiredService<CorpDbContext>();

        var profile = new Person() { TenantId = 1, GivenName = "Jack", FamilyName = "Squat" };
        corpDb.People.Add(profile);
        await corpDb.SaveChangesAsync();

        profile.Id.Should().BeGreaterThan(0);
    }
}
