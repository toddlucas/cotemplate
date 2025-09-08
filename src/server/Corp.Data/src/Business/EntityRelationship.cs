namespace Corp.Business;

using TRecord = EntityRelationship;

public class EntityRelationship : EntityRelationshipModel, ITemporal
{
    #region Navigation properties

    /// <summary>
    /// The parent entity.
    /// </summary>
    public Entity ParentEntity { get; set; } = null!;

    /// <summary>
    /// The child entity.
    /// </summary>
    public Entity ChildEntity { get; set; } = null!;

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
        modelBuilder.Entity<TRecord>().ToTable(nameof(EntityRelationship));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.ParentEntityId).HasColumnName("parent_entity_id");
        modelBuilder.Entity<TRecord>().Property(x => x.ChildEntityId).HasColumnName("child_entity_id");
        modelBuilder.Entity<TRecord>().Property(x => x.RelationshipTypeId).HasColumnName("relationship_type_id");
        modelBuilder.Entity<TRecord>().Property(x => x.PercentOwnership).HasColumnName("percent_ownership");
        modelBuilder.Entity<TRecord>().Property(x => x.StartAt).HasColumnName("start_at");
        modelBuilder.Entity<TRecord>().Property(x => x.EndAt).HasColumnName("end_at");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.ParentEntity)
            .WithMany(y => y.ChildRelationships)
            .HasForeignKey(x => x.ParentEntityId)
            .IsRequired();

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.ChildEntity)
            .WithMany(y => y.ParentRelationships)
            .HasForeignKey(x => x.ChildEntityId)
            .IsRequired();

        // Indexes
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.ParentEntityId, b.ChildEntityId, b.RelationshipTypeId })
            .IsUnique();

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord
        {
            Id = 1,
            TenantId = 1,
            ParentEntityId = 1,
            ChildEntityId = 1,
            RelationshipTypeId = nameof(EntityRelationshipType.owns),
            PercentOwnership = 1.0000m,
            StartAt = createdAt,
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        });
    }
}
