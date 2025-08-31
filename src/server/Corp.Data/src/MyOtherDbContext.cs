namespace Corp.Data;

/// <summary>
/// The data database context.
/// </summary>
public class MyOtherDbContext : DbContext
{
    public MyOtherDbContext(DbContextOptions<MyOtherDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteDataDbContext" />
    /// class using the specified options.
    /// </summary>
    /// <remarks>
    /// Requires a non-generic DbContextOptions in order to be used with
    /// SqliteDatabaseSet. But this is at odds with design-time creation. So we
    /// must use our own design-time factory.
    /// https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation
    /// </remarks>
    private MyOtherDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public static MyOtherDbContext Create(DbContextOptions options)
    {
        return new MyOtherDbContext(options);
    }

    #region Other

    // public DbSet<EditBatch> EditBatches { get; set; } = null!;

    #endregion Other

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

        // Other
        //EditBatch.OnModelCreating(modelBuilder);

        modelBuilder.Snakeify();

        //EnumerationBuilder.OnCreating<EditBatchStatus, int>(modelBuilder, EditBatchStatus.GetAll(), "edit_batch_status");
        //EnumerationBuilder.OnStringCreating(modelBuilder, LanguageCode.GetAll(), LanguageCode.KeyLength, "language_code");
    }
}
