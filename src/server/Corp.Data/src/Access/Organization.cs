namespace Corp;

using TRecord = Organization;

public class Organization : OrganizationModel, ITemporal
{
    #region Navigation properties

    /// <summary>
    /// The organization members.
    /// </summary>
    public OrganizationMember[] Members { get; set; } = [];

    /// <summary>
    /// The child organizations.
    /// </summary>
    public Organization[] ChildOrganizations { get; set; } = [];

    /// <summary>
    /// The parent organization.
    /// </summary>
    public Organization? ParentOrganization { get; set; }

    #endregion Navigation properties

    #region ITemporal

    /// <summary>
    /// The created timestamp.
    /// </summary>
    [Display(Name = "Created at")]
    [Description("The date and time this record was created, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The updated timestamp.
    /// </summary>
    [Display(Name = "Updated at")]
    [Description("The date and time this record was last updated, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// The deleted timestamp.
    /// </summary>
    [Display(Name = "Deleted at")]
    [Description("The date and time this record was deleted, or null, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset? DeletedAt { get; set; }

    #endregion ITemporal

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table
        modelBuilder.Entity<TRecord>().ToTable(nameof(Organization));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<TRecord>().Property(x => x.Code).HasColumnName("code");
        modelBuilder.Entity<TRecord>().Property(x => x.ParentOrgId).HasColumnName("parent_org_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Status).HasColumnName("status");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.ParentOrganization)
            .WithMany(y => y.ChildOrganizations)
            .HasForeignKey(x => x.ParentOrgId)
            .IsRequired(false);

        // Indexes
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.Name });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.Code })
            .IsUnique()
            .HasFilter("code IS NOT NULL");

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 1, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord
        {
            Id = 1,
            TenantId = 1,
            Name = "Sample Organization",
            Code = "SAMPLE",
            Status = "active",
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        });
    }
}
