using Corp.Access;
using Corp.Business;
using Corp.Storage;

namespace Corp.Workflow;

using TRecord = TaskRecord;

public class TaskRecord : TaskModel, ITemporal
{
    #region Internal properties

#if RESELLER
    /// <summary>
    /// The group ID this task belongs to.
    /// </summary>
    [Display(Name = "Group ID")]
    [Required]
    public Guid GroupId { get; set; }
#endif

    /// <summary>
    /// The tenant ID this task belongs to.
    /// </summary>
    [Display(Name = "Tenant ID")]
    [Required]
    public Guid TenantId { get; set; }

    #endregion Internal properties

    #region Navigation properties

    /// <summary>
    /// The checklist instance.
    /// </summary>
    public Checklist? Checklist { get; set; }

    /// <summary>
    /// The organization.
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// The entity.
    /// </summary>
    public Entity? Entity { get; set; }

    /// <summary>
    /// The assigned person.
    /// </summary>
    public Person? AssigneePerson { get; set; }

    /// <summary>
    /// The source task template.
    /// </summary>
    public TaskTemplate? SourceTaskTemplate { get; set; }

    /// <summary>
    /// The evidence document.
    /// </summary>
    public Document? EvidenceDocument { get; set; }

    /// <summary>
    /// The task status enumeration.
    /// </summary>
    public TaskStatusEnum? TaskStatusEnum { get; set; }

    /// <summary>
    /// The priority enumeration.
    /// </summary>
    public PriorityEnum? PriorityEnum { get; set; }

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
        modelBuilder.Entity<TRecord>().ToTable(nameof(TaskRecord));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
#if RESELLER
        modelBuilder.Entity<TRecord>().Property(x => x.GroupId).HasColumnName("group_id");
#endif
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.OrgId).HasColumnName("org_id");
        modelBuilder.Entity<TRecord>().Property(x => x.ChecklistId).HasColumnName("checklist_id");
        modelBuilder.Entity<TRecord>().Property(x => x.EntityId).HasColumnName("entity_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<TRecord>().Property(x => x.StatusId).HasColumnName("status_id");
        modelBuilder.Entity<TRecord>().Property(x => x.PriorityId).HasColumnName("priority_id");
        modelBuilder.Entity<TRecord>().Property(x => x.AssigneePersonId).HasColumnName("assignee_person_id");
        modelBuilder.Entity<TRecord>().Property(x => x.DueAt).HasColumnName("due_at");
        modelBuilder.Entity<TRecord>().Property(x => x.StartedAt).HasColumnName("started_at");
        modelBuilder.Entity<TRecord>().Property(x => x.CompletedAt).HasColumnName("completed_at");
        modelBuilder.Entity<TRecord>().Property(x => x.RecurrenceRule).HasColumnName("recurrence_rule");
        modelBuilder.Entity<TRecord>().Property(x => x.SourceTaskTemplateId).HasColumnName("source_task_template_id");
        modelBuilder.Entity<TRecord>().Property(x => x.EvidenceDocumentId).HasColumnName("evidence_document_id");
        modelBuilder.Entity<TRecord>().Property(x => x.AiSummary).HasColumnName("ai_summary");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Checklist)
            .WithMany()
            .HasForeignKey(x => x.ChecklistId)
            .IsRequired(false);

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
            .HasOne(x => x.AssigneePerson)
            .WithMany()
            .HasForeignKey(x => x.AssigneePersonId)
            .IsRequired(false);

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.SourceTaskTemplate)
            .WithMany()
            .HasForeignKey(x => x.SourceTaskTemplateId)
            .IsRequired(false);

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.EvidenceDocument)
            .WithMany()
            .HasForeignKey(x => x.EvidenceDocumentId)
            .IsRequired(false);

        // Enumeration relationships
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.TaskStatusEnum)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .IsRequired();

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.PriorityEnum)
            .WithMany()
            .HasForeignKey(x => x.PriorityId);

        // Indexes
#if RESELLER
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.GroupId);
#endif
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.StatusId, b.DueAt });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.AssigneePersonId });

        // Seed data (optional)
        var createdAt1 = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        var createdAt2 = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc);
        var createdAt3 = new DateTime(2024, 3, 25, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt1 = new DateTime(2024, 12, 19, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt2 = new DateTime(2024, 12, 19, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt3 = new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Utc);

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
                Name = "Annual Review",
                StatusId = nameof(TaskStatus.todo),
                PriorityId = nameof(Priority.normal),
                AssigneePersonId = 1,
                DueAt = createdAt1.AddDays(30),
                CreatedAt = createdAt1,
                UpdatedAt = updatedAt1
            },
            new TRecord
            {
                Id = 2,
#if RESELLER
                GroupId = IdentitySeedData.GroupId,
#endif
                TenantId = IdentitySeedData.TenantId,
                OrgId = 1,
                EntityId = 2,
                Name = "Budget Planning",
                StatusId = nameof(TaskStatus.done),
                PriorityId = nameof(Priority.high),
                AssigneePersonId = 2,
                DueAt = createdAt2.AddDays(15),
                CompletedAt = createdAt2.AddDays(10),
                CreatedAt = createdAt2,
                UpdatedAt = updatedAt2
            },
            new TRecord
            {
                Id = 3,
#if RESELLER
                GroupId = IdentitySeedData.GroupId,
#endif
                TenantId = IdentitySeedData.TenantId,
                OrgId = 2,
                EntityId = 3,
                Name = "Q4 Report",
                StatusId = nameof(TaskStatus.todo),
                PriorityId = nameof(Priority.normal),
                AssigneePersonId = 3,
                DueAt = createdAt3.AddDays(45),
                CreatedAt = createdAt3,
                UpdatedAt = updatedAt3
            }
        );
    }
}
