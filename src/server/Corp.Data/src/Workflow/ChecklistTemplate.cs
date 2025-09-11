namespace Corp.Workflow;

using TRecord = ChecklistTemplate;

public class ChecklistTemplate : ChecklistTemplateModel, ITemporal
{
    #region Internal properties

#if RESELLER
    /// <summary>
    /// The group ID this checklist template belongs to.
    /// </summary>
    [Display(Name = "Group ID")]
    [Required]
    public string GroupId { get; set; } = null!;
#endif

    #endregion Internal properties

    #region Navigation properties

    /// <summary>
    /// The task templates in this checklist template.
    /// </summary>
    public TaskTemplate[] TaskTemplates { get; set; } = [];

    /// <summary>
    /// The checklist scope enumeration.
    /// </summary>
    public ChecklistScopeEnum? ChecklistScopeEnum { get; set; }

    /// <summary>
    /// The source type enumeration.
    /// </summary>
    public SourceTypeEnum? SourceTypeEnum { get; set; }

    #endregion Navigation properties

    #region ITemporal

    /// <summary>
    /// The created timestamp.
    /// </summary>
    [Display(Name = "Created at")]
    [Description("The date and time this record was created, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The updated timestamp.
    /// </summary>
    [Display(Name = "Updated at")]
    [Description("The date and time this record was last updated, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// The deleted timestamp.
    /// </summary>
    [Display(Name = "Deleted at")]
    [Description("The date and time this record was deleted, or null, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTime? DeletedAt { get; set; }

    #endregion ITemporal

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table
        modelBuilder.Entity<TRecord>().ToTable(nameof(ChecklistTemplate));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
#if RESELLER
        modelBuilder.Entity<TRecord>().Property(x => x.GroupId).HasColumnName("group_id");
#endif
        modelBuilder.Entity<TRecord>().Property(x => x.ScopeId).HasColumnName("scope_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<TRecord>().Property(x => x.Version).HasColumnName("version");
        modelBuilder.Entity<TRecord>().Property(x => x.SourceTypeId).HasColumnName("source_type_id");
        modelBuilder.Entity<TRecord>().Property(x => x.JurisdictionCountry).HasColumnName("jurisdiction_country");
        modelBuilder.Entity<TRecord>().Property(x => x.JurisdictionRegion).HasColumnName("jurisdiction_region");
        modelBuilder.Entity<TRecord>().Property(x => x.AppliesTo).HasColumnName("applies_to");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasMany(x => x.TaskTemplates)
            .WithOne(y => y.ChecklistTemplate)
            .HasForeignKey(y => y.ChecklistTemplateId)
            .IsRequired();

        // Enumeration relationships
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.ChecklistScopeEnum)
            .WithMany()
            .HasForeignKey(x => x.ScopeId)
            .IsRequired();

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.SourceTypeEnum)
            .WithMany()
            .HasForeignKey(x => x.SourceTypeId)
            .IsRequired();

        // Indexes
#if RESELLER
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.GroupId);
#endif
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.ScopeId });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.JurisdictionCountry, b.JurisdictionRegion });

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord
        {
            Id = 1,
#if RESELLER
            GroupId = IdentitySeedData.GroupId,
#endif
            ScopeId = nameof(ChecklistScope.entity),
            Name = "Sample Checklist Template",
            Version = "1.0",
            SourceTypeId = nameof(SourceType.system),
            JurisdictionCountry = "US",
            JurisdictionRegion = "US-CA",
            AppliesTo = "{\"entity_types\":[\"llc\",\"c_corp\"]}",
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        });
    }
}
