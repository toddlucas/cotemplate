﻿using Microsoft.AspNetCore.Identity;

namespace Base.Data.Identity;

public interface ITenantResolver
{
    Task<IdentityTenant<Guid>?> GetTenantByDomainAsync(string domain);
    Task<IdentityTenant<Guid>?> GetTenantBySubdomainAsync(string subdomain);
}
