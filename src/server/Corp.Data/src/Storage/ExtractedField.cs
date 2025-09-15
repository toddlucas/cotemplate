namespace Corp.Storage;

using TRecord = ExtractedField;

public class ExtractedField : ExtractedFieldModel, ITemporalRecord
{
    #region Internal properties

#if RESELLER
    /// <summary>
    /// The group ID this extracted field belongs to.
    /// </summary>
    [Display(Name = "Group ID")]
    [Required]
    public Guid GroupId { get; set; }
#endif

    /// <summary>
    /// The tenant ID this extracted field belongs to.
    /// </summary>
    [Display(Name = "Tenant ID")]
    [Required]
    public Guid TenantId { get; set; }

    #endregion Internal properties

    #region Navigation properties

    /// <summary>
    /// The organization.
    /// </summary>
    public Organization Organization { get; set; } = null!;

    /// <summary>
    /// The document.
    /// </summary>
    public Document Document { get; set; } = null!;

    #endregion Navigation properties

    #region ITemporalRecord

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

    #endregion ITemporalRecord

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table
        modelBuilder.Entity<TRecord>().ToTable(nameof(ExtractedField));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
#if RESELLER
        modelBuilder.Entity<TRecord>().Property(x => x.GroupId).HasColumnName("group_id");
#endif
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.OrgId).HasColumnName("org_id");
        modelBuilder.Entity<TRecord>().Property(x => x.DocumentId).HasColumnName("document_id");
        modelBuilder.Entity<TRecord>().Property(x => x.SchemaKey).HasColumnName("schema_key");
        modelBuilder.Entity<TRecord>().Property(x => x.ValueText).HasColumnName("value_text");
        modelBuilder.Entity<TRecord>().Property(x => x.ValueNumber).HasColumnName("value_number");
        modelBuilder.Entity<TRecord>().Property(x => x.ValueDate).HasColumnName("value_date");
        modelBuilder.Entity<TRecord>().Property(x => x.Confidence).HasColumnName("confidence");
        modelBuilder.Entity<TRecord>().Property(x => x.Revision).HasColumnName("revision");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedBy).HasColumnName("created_by");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrgId)
            .IsRequired();

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Document)
            .WithMany(y => y.ExtractedFields)
            .HasForeignKey(x => x.DocumentId)
            .IsRequired();

        // Indexes
#if RESELLER
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.GroupId);
#endif
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.DocumentId, b.SchemaKey });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.SchemaKey });

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord
        {
            Id = 1,
#if RESELLER
            GroupId = IdentitySeedData.GroupId,
#endif
            TenantId = IdentitySeedData.TenantId,
            OrgId = 1,
            DocumentId = 1,
            SchemaKey = "entity_name",
            ValueText = "Sample LLC",
            Confidence = 0.95m,
            Revision = 1,
            CreatedBy = "system",
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        });
    }
}
