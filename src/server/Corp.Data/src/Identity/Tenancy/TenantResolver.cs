using Microsoft.AspNetCore.Identity;

using Corp.Identity.EntityFrameworkCore;

namespace Corp.Data.Identity;

public class TenantResolver(CorpDbContext dbContext) : ITenantResolver
{
    private readonly CorpDbContext _dbContext = dbContext;

    public async Task<IdentityTenant?> GetTenantByDomainAsync(string domain)
    {
        IdentityTenant? tenant = await _dbContext.Tenants
            .Where(t => t.Domain == domain)
            .SingleOrDefaultAsync();

        return tenant;
    }

    public async Task<IdentityTenant?> GetTenantBySubdomainAsync(string subdomain)
    {
        IdentityTenant? tenant = await _dbContext.Tenants
            .Where(t => t.Subdomain == subdomain)
            .SingleOrDefaultAsync();

        return tenant;
    }
}
