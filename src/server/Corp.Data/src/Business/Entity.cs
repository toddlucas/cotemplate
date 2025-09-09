namespace Corp.Business;

using TRecord = Entity;

public class Entity : EntityModel, ITemporal
{
    #region Navigation properties

    /// <summary>
    /// The organization.
    /// </summary>
    public Organization Organization { get; set; } = null!;

    /// <summary>
    /// The entity type enumeration.
    /// </summary>
    public EntityTypeEnum? EntityTypeEnum { get; set; }

    /// <summary>
    /// The ownership model enumeration.
    /// </summary>
    public OwnershipModelEnum? OwnershipModelEnum { get; set; }

    /// <summary>
    /// The entity status enumeration.
    /// </summary>
    public EntityStatusEnum? EntityStatusEnum { get; set; }

    /// <summary>
    /// The entity roles.
    /// </summary>
    public EntityRole[] Roles { get; set; } = [];

    /// <summary>
    /// The parent entity relationships.
    /// </summary>
    public EntityRelationship[] ParentRelationships { get; set; } = [];

    /// <summary>
    /// The child entity relationships.
    /// </summary>
    public EntityRelationship[] ChildRelationships { get; set; } = [];

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
        modelBuilder.Entity<TRecord>().ToTable(nameof(Entity));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.OrgId).HasColumnName("org_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<TRecord>().Property(x => x.LegalName).HasColumnName("legal_name");
        modelBuilder.Entity<TRecord>().Property(x => x.EntityTypeId).HasColumnName("entity_type_id");
        modelBuilder.Entity<TRecord>().Property(x => x.FormationDate).HasColumnName("formation_date");
        modelBuilder.Entity<TRecord>().Property(x => x.JurisdictionCountry).HasColumnName("jurisdiction_country");
        modelBuilder.Entity<TRecord>().Property(x => x.JurisdictionRegion).HasColumnName("jurisdiction_region");
        modelBuilder.Entity<TRecord>().Property(x => x.Ein).HasColumnName("ein");
        modelBuilder.Entity<TRecord>().Property(x => x.StateFileNumber).HasColumnName("state_file_number");
        modelBuilder.Entity<TRecord>().Property(x => x.RegisteredAgent).HasColumnName("registered_agent");
        modelBuilder.Entity<TRecord>().Property(x => x.OwnershipModelId).HasColumnName("ownership_model_id");
        modelBuilder.Entity<TRecord>().Property(x => x.StatusId).HasColumnName("status_id");
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

        // Enumeration relationships
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.EntityTypeEnum)
            .WithMany()
            .HasForeignKey(x => x.EntityTypeId)
            .IsRequired();

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.OwnershipModelEnum)
            .WithMany()
            .HasForeignKey(x => x.OwnershipModelId);

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.EntityStatusEnum)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .IsRequired();

        // Indexes
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.OrgId });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.StatusId});

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord
        {
            Id = 1,
            TenantId = 1,
            OrgId = 1,
            Name = "Sample LLC",
            LegalName = "Sample LLC",
            EntityTypeId = nameof(EntityType.llc),
            FormationDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            JurisdictionCountry = "US",
            JurisdictionRegion = "US-CA",
            OwnershipModelId = nameof(OwnershipModel.member_managed),
            StatusId = nameof(EntityStatus.active),
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        });
    }
}
