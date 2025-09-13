﻿using Microsoft.AspNetCore.Identity;

using Base.Data.Identity;

namespace Corp.Data.Identity;

public class TenantResolver(CorpDbContext dbContext) : ITenantResolver
{
    private readonly CorpDbContext _dbContext = dbContext;

    public async Task<IdentityTenant<Guid>?> GetTenantByDomainAsync(string domain)
    {
        IdentityTenant<Guid>? tenant = await _dbContext.Tenants
            .Where(t => t.Domain == domain)
            .SingleOrDefaultAsync();

        return tenant;
    }

    public async Task<IdentityTenant<Guid>?> GetTenantBySubdomainAsync(string subdomain)
    {
        IdentityTenant<Guid>? tenant = await _dbContext.Tenants
            .Where(t => t.Subdomain == subdomain)
            .SingleOrDefaultAsync();

        return tenant;
    }
}
