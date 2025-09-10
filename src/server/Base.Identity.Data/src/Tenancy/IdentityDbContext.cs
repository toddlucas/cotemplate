using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Identity;

/// <summary>
/// Base class for the Entity Framework database context used for identity.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TTenant">The type of tenant objects.</typeparam>
public class IdentityDbContext<TUser, TTenant> : IdentityDbContext<TUser, IdentityRole, TTenant, string>
    where TUser : TenantIdentityUser
    where TTenant : IdentityTenant
{
    /// <summary>
    /// Initializes a new instance of <see cref="IdentityDbContext"/>.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public IdentityDbContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityDbContext" /> class.
    /// </summary>
    protected IdentityDbContext() { }
}

/// <summary>
/// Base class for the Entity Framework database context used for identity.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TRole">The type of role objects.</typeparam>
/// <typeparam name="TTenant">The type of tenant objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
public class IdentityDbContext<TUser, TRole, TTenant, TKey> : IdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
    where TUser : TenantIdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TTenant : IdentityTenant<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the db context.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public IdentityDbContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    protected IdentityDbContext() { }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of Tenants.
    /// </summary>
    public virtual DbSet<TTenant> Tenants { get; set; } = default!;

    /// <summary>
    /// Configures the schema needed for the identity framework.
    /// </summary>
    /// <param name="builder">
    /// The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //
        // User overrides and adjustments.
        //

        // Override the uniqueness constraint set in the IdentityUserContext base.
        builder.Entity<TUser>().HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique(false);
        // builder.Entity<TUser>().HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

        builder.Entity<TUser>()
            .HasIndex(u => new { u.NormalizedUserName, u.TenantId })
            .HasDatabaseName("UserNameTenantIndex")
            .IsUnique();

        // Although the base identity context doesn't require uniqueness here,
        // we probably should.
        builder.Entity<TUser>()
            .HasIndex(u => new { u.NormalizedEmail, u.TenantId })
            .HasDatabaseName("EmailTenantIndex")
            .IsUnique();

        //
        // Tenants
        //

        builder.Entity<TTenant>().ToTable("AspNetTenants");

        builder.Entity<TTenant>()
            .HasMany<TUser>()
            .WithOne()
            .HasForeignKey(e => e.TenantId)
            .IsRequired();
    }
}
