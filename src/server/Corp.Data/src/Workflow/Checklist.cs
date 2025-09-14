using Corp.Access;
using Corp.Business;

namespace Corp.Workflow;

using TRecord = Checklist;

public class Checklist : ChecklistModel, ITemporal
{
    #region Internal properties

#if RESELLER
    /// <summary>
    /// The group ID this checklist belongs to.
    /// </summary>
    [Display(Name = "Group ID")]
    [Required]
    public Guid GroupId { get; set; }
#endif

    /// <summary>
    /// The tenant ID this checklist belongs to.
    /// </summary>
    [Display(Name = "Tenant ID")]
    [Required]
    public Guid TenantId { get; set; }

    #endregion Internal properties

    #region Navigation properties

    /// <summary>
    /// The organization.
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// The entity.
    /// </summary>
    public Entity? Entity { get; set; }

    /// <summary>
    /// The person.
    /// </summary>
    public Person? Person { get; set; }

    /// <summary>
    /// The checklist template.
    /// </summary>
    public ChecklistTemplate? Template { get; set; }

    /// <summary>
    /// The tasks in this checklist.
    /// </summary>
    public TaskRecord[] Tasks { get; set; } = [];

    /// <summary>
    /// The checklist status enumeration.
    /// </summary>
    public ChecklistStatusEnum? ChecklistStatusEnum { get; set; }

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
        modelBuilder.Entity<TRecord>().ToTable(nameof(Checklist));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
#if RESELLER
        modelBuilder.Entity<TRecord>().Property(x => x.GroupId).HasColumnName("group_id");
#endif
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.OrgId).HasColumnName("org_id");
        modelBuilder.Entity<TRecord>().Property(x => x.EntityId).HasColumnName("entity_id");
        modelBuilder.Entity<TRecord>().Property(x => x.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<TRecord>().Property(x => x.TemplateId).HasColumnName("template_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<TRecord>().Property(x => x.StatusId).HasColumnName("status_id");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedFromId).HasColumnName("created_from_id");
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
            .HasOne(x => x.Template)
            .WithMany()
            .HasForeignKey(x => x.TemplateId)
            .IsRequired(false);

        // Enumeration relationships
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.ChecklistStatusEnum)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .IsRequired();

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.SourceTypeEnum)
            .WithMany()
            .HasForeignKey(x => x.CreatedFromId)
            .IsRequired();

        // Indexes
#if RESELLER
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.GroupId);
#endif
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.OrgId });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.EntityId });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.PersonId });

        // Seed data (optional)
        var createdAt1 = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt1 = new DateTime(2024, 12, 19, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<TRecord>().HasData(
            new TRecord
            {
                Id = 1,
#if RESELLER
                GroupId = IdentitySeedData.GroupId,
#endif
                TenantId = IdentitySeedData.TenantId,
                OrgId = 1,
                EntityId = 1,
                TemplateId = 1,
                Name = "Compliance Checklist",
                StatusId = nameof(ChecklistStatus.active),
                CreatedFromId = nameof(SourceType.system),
                CreatedAt = createdAt1,
                UpdatedAt = updatedAt1
            }
        );
    }
}
