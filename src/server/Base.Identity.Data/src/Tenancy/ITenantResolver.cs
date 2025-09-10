using Microsoft.AspNetCore.Identity;

namespace Corp.Identity.EntityFrameworkCore;

public interface ITenantResolver
{
    Task<IdentityTenant?> GetTenantByDomainAsync(string domain);
    Task<IdentityTenant?> GetTenantBySubdomainAsync(string subdomain);
}
