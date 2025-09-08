namespace Corp.Workflow;

using TRecord = TaskTemplate;

public class TaskTemplate : TaskTemplateModel, ITemporal
{
    #region Navigation properties

    /// <summary>
    /// The checklist template.
    /// </summary>
    public ChecklistTemplate ChecklistTemplate { get; set; } = null!;

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
        modelBuilder.Entity<TRecord>().ToTable(nameof(TaskTemplate));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.ChecklistTemplateId).HasColumnName("checklist_template_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<TRecord>().Property(x => x.DescriptionMd).HasColumnName("description_md");
        modelBuilder.Entity<TRecord>().Property(x => x.DefaultDueOffsetDays).HasColumnName("default_due_offset_days");
        modelBuilder.Entity<TRecord>().Property(x => x.RecurrenceRule).HasColumnName("recurrence_rule");
        modelBuilder.Entity<TRecord>().Property(x => x.PriorityId).HasColumnName("priority_id");
        modelBuilder.Entity<TRecord>().Property(x => x.RequiresEvidence).HasColumnName("requires_evidence");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.ChecklistTemplate)
            .WithMany()
            .HasForeignKey(x => x.ChecklistTemplateId)
            .IsRequired();

        // Enumeration relationships
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.PriorityEnum)
            .WithMany()
            .HasForeignKey(x => x.PriorityId);

        // Indexes
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.ChecklistTemplateId);

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord
        {
            Id = 1,
            TenantId = 1,
            ChecklistTemplateId = 1,
            Name = "Sample Task Template",
            DescriptionMd = "This is a sample task template",
            DefaultDueOffsetDays = 30,
            PriorityId = nameof(Priority.normal),
            RequiresEvidence = false,
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        });
    }
}
