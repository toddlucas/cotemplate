using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Corp.Data;

/// <summary>
/// The app database context.
/// </summary>
public class CorpDbContext : IdentityDbContext // <IdentityUser<long>, IdentityRole<long>, long>
{
    public CorpDbContext(DbContextOptions<CorpDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteAppDbContext" />
    /// class using the specified options.
    /// </summary>
    /// <remarks>
    /// Requires a non-generic DbContextOptions in order to be used with
    /// SqliteDatabaseSet. But this is at odds with design-time creation. So we
    /// must use our own design-time factory.
    /// https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation
    /// </remarks>
    private CorpDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public static CorpDbContext Create(DbContextOptions options)
    {
        return new CorpDbContext(options);
    }

    #region Identity

    // public DbSet<Profile> Profiles { get; set; } = null!;

    #endregion Identity

    protected bool IsUsingSqliteProvider => Database.ProviderName!.Contains("Sqlite", StringComparison.OrdinalIgnoreCase);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (IsUsingSqliteProvider)
            modelBuilder.AddSqliteDateTimeOffset();

        // ApplicationTenant.OnModelCreating(modelBuilder);
        IdentityModel.OnModelCreating(modelBuilder);

        // Identity
        //Profile.OnModelCreating(modelBuilder);

        modelBuilder.Snakeify();

        //EnumerationBuilder.OnStringCreating(modelBuilder, LanguageCode.GetAll(), LanguageCode.KeyLength, "language_code");
    }
}
