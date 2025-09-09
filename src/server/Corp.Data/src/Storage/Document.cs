namespace Corp.Storage;

using TRecord = Document;

public class Document : DocumentModel, ITemporal
{
    #region Navigation properties

    /// <summary>
    /// The organization.
    /// </summary>
    public Access.Organization? Organization { get; set; }

    /// <summary>
    /// The entity.
    /// </summary>
    public Business.Entity? Entity { get; set; }

    /// <summary>
    /// The person.
    /// </summary>
    public Access.Person? Person { get; set; }

    /// <summary>
    /// The person who uploaded the document.
    /// </summary>
    public Access.Person UploadedByPerson { get; set; } = null!;

    /// <summary>
    /// The extracted fields from this document.
    /// </summary>
    public ExtractedField[] ExtractedFields { get; set; } = [];

    /// <summary>
    /// The document category enumeration.
    /// </summary>
    public DocumentCategoryEnum? DocumentCategoryEnum { get; set; }

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
        modelBuilder.Entity<TRecord>().ToTable(nameof(Document));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.OrgId).HasColumnName("org_id");
        modelBuilder.Entity<TRecord>().Property(x => x.EntityId).HasColumnName("entity_id");
        modelBuilder.Entity<TRecord>().Property(x => x.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Title).HasColumnName("title");
        modelBuilder.Entity<TRecord>().Property(x => x.CategoryId).HasColumnName("category_id");
        modelBuilder.Entity<TRecord>().Property(x => x.StorageUri).HasColumnName("storage_uri");
        modelBuilder.Entity<TRecord>().Property(x => x.MimeType).HasColumnName("mime_type");
        modelBuilder.Entity<TRecord>().Property(x => x.Hash).HasColumnName("hash");
        modelBuilder.Entity<TRecord>().Property(x => x.UploadedBy).HasColumnName("uploaded_by");
        modelBuilder.Entity<TRecord>().Property(x => x.UploadedAt).HasColumnName("uploaded_at");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrgId)
            .IsRequired(false);

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Entity)
            .WithMany()
            .HasForeignKey(x => x.EntityId)
            .IsRequired(false);

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .IsRequired(false);

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.UploadedByPerson)
            .WithMany()
            .HasForeignKey(x => x.UploadedBy)
            .IsRequired();

        // Enumeration relationships
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.DocumentCategoryEnum)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();

        // Indexes
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.EntityId });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.CategoryId });

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord
        {
            Id = 1,
            TenantId = 1,
            OrgId = 1,
            EntityId = 1,
            Title = "Sample Document",
            CategoryId = nameof(DocumentCategory.formation),
            StorageUri = "s3://sample-bucket/sample-document.pdf",
            MimeType = "application/pdf",
            Hash = "sha256:sample-hash",
            UploadedBy = 1,
            UploadedAt = createdAt,
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        });
    }
}
