namespace Corp.Identity;

public class TenantContext<TKey> where TKey : IEquatable<TKey>
{
    public string? Hostname { get; set; }
    public string? Subdomain { get; set; }

    public TKey? HostnameTenantId { get; set; }
    public TKey? SubdomainTenantId { get; set; }
    public TKey? UserTenantId { get; set; }
    public TKey? CurrentId { get; set; }

    public TKey TenantId => CurrentId ?? throw new Exception("Required tenant context is not present.");
}
