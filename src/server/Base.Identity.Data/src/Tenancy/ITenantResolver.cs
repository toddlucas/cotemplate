using Microsoft.AspNetCore.Identity;

namespace Corp.Identity.EntityFrameworkCore;

public interface ITenantResolver
{
    Task<IdentityTenant<Guid>?> GetTenantByDomainAsync(string domain);
    Task<IdentityTenant<Guid>?> GetTenantBySubdomainAsync(string subdomain);
}
