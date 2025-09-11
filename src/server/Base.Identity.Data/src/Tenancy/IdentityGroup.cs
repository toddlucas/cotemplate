#if RESELLER
using Microsoft.AspNetCore.Identity;

namespace Microsoft.AspNetCore.Identity;

/// <summary>
/// The default implementation of <see cref="IdentityGroup{TKey}"/> which uses a string as a primary key.
/// </summary>
public class IdentityGroup : IdentityGroup<string>
{
    /// <summary>
    /// Initializes a new instance of <see cref="IdentityGroup"/>.
    /// </summary>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public IdentityGroup()
    {
        Id = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Represents a tenant in the identity system.
/// </summary>
/// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
public class IdentityGroup<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of <see cref="IdentityGroup{TKey}"/>.
    /// </summary>
    public IdentityGroup() { }

    /// <summary>
    /// Gets or sets the primary key for this tenant.
    /// </summary>
    [PersonalData]
    public virtual TKey Id { get; set; } = default!;

    public string? Domain { get; set; }
    public string? Subdomain { get; set; }
}
#endif
