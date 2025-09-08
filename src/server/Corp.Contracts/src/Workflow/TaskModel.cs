using Corp.Business;
using Corp.Storage;

namespace Corp.Workflow;

/// <summary>
/// Represents a task in the system.
/// </summary>
public class TaskModel
{
    /// <summary>
    /// The task ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The tenant ID this task belongs to.
    /// </summary>
    [Display(Name = "Tenant ID")]
    [Required]
    public long TenantId { get; set; }

    /// <summary>
    /// The organization ID.
    /// </summary>
    [Display(Name = "Organization ID")]
    public long OrgId { get; set; }

    /// <summary>
    /// The checklist instance ID (optional).
    /// </summary>
    [Display(Name = "Checklist ID")]
    public long? ChecklistId { get; set; }

    /// <summary>
    /// The entity ID (optional).
    /// </summary>
    [Display(Name = "Entity ID")]
    public long? EntityId { get; set; }

    /// <summary>
    /// The task name.
    /// </summary>
    [Display(Name = "Name")]
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The task status. See <see cref="TaskStatusEnum"/>.
    /// </summary>
    [Display(Name = "Status")]
    [Required]
    [StringLength(20)]
    public string StatusId { get; set; } = null!;

    /// <summary>
    /// The task priority. See <see cref="PriorityEnum"/>.
    /// </summary>
    [Display(Name = "Priority")]
    [StringLength(10)]
    public string? PriorityId { get; set; }

    /// <summary>
    /// The person assigned to this task.
    /// </summary>
    [Display(Name = "Assignee Person ID")]
    public long? AssigneePersonId { get; set; }

    /// <summary>
    /// The due date for this task.
    /// </summary>
    [Display(Name = "Due Date")]
    public DateTime? DueAt { get; set; }

    /// <summary>
    /// The start date for this task.
    /// </summary>
    [Display(Name = "Start Date")]
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// The completion date for this task.
    /// </summary>
    [Display(Name = "Completion Date")]
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// The recurrence rule for this task (RFC5545 text).
    /// </summary>
    [Display(Name = "Recurrence Rule")]
    [StringLength(500)]
    public string? RecurrenceRule { get; set; }

    /// <summary>
    /// The source task template ID.
    /// </summary>
    [Display(Name = "Source Task Template ID")]
    public long? SourceTaskTemplateId { get; set; }

    /// <summary>
    /// The evidence document ID.
    /// </summary>
    [Display(Name = "Evidence Document ID")]
    public long? EvidenceDocumentId { get; set; }

    /// <summary>
    /// The AI summary for this task.
    /// </summary>
    [Display(Name = "AI Summary")]
    [StringLength(2000)]
    public string? AiSummary { get; set; }

    /// <summary>
    /// Additional metadata for the task.
    /// </summary>
    [Display(Name = "Metadata")]
    public string? Metadata { get; set; }
}

/// <summary>
/// Detailed task model with related entities and temporal tracking.
/// </summary>
public class TaskDetailModel : TaskModel, ITemporal
{
    /// <summary>
    /// The organization this task belongs to.
    /// </summary>
    public OrganizationModel Organization { get; set; } = null!;

    /// <summary>
    /// The checklist instance this task belongs to.
    /// </summary>
    public ChecklistModel? Checklist { get; set; }

    /// <summary>
    /// The entity this task is associated with.
    /// </summary>
    public EntityModel? Entity { get; set; }

    /// <summary>
    /// The person assigned to this task.
    /// </summary>
    public PersonModel? AssigneePerson { get; set; }

    /// <summary>
    /// The source task template.
    /// </summary>
    public TaskTemplateModel? SourceTaskTemplate { get; set; }

    /// <summary>
    /// The evidence document.
    /// </summary>
    public DocumentModel? EvidenceDocument { get; set; }

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
}
