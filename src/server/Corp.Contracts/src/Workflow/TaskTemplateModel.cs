namespace Corp.Workflow;

/// <summary>
/// Represents a task template.
/// </summary>
public class TaskTemplateModel
{
    /// <summary>
    /// The task template ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The checklist template ID this task template belongs to.
    /// </summary>
    [Display(Name = "Checklist Template ID")]
    [Required]
    public long ChecklistTemplateId { get; set; }

    /// <summary>
    /// The task template name.
    /// </summary>
    [Display(Name = "Name")]
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The task template description (Markdown).
    /// </summary>
    [Display(Name = "Description")]
    [StringLength(2000)]
    public string? DescriptionMd { get; set; }

    /// <summary>
    /// The default due offset in days.
    /// </summary>
    [Display(Name = "Default Due Offset Days")]
    public int? DefaultDueOffsetDays { get; set; }

    /// <summary>
    /// The recurrence rule (RFC5545 text).
    /// </summary>
    [Display(Name = "Recurrence Rule")]
    [StringLength(500)]
    public string? RecurrenceRule { get; set; }

    /// <summary>
    /// The task priority. See <see cref="PriorityEnum"/>.
    /// </summary>
    [Display(Name = "Priority")]
    [StringLength(10)]
    public string? PriorityId { get; set; }

    /// <summary>
    /// Whether this task requires evidence.
    /// </summary>
    [Display(Name = "Requires Evidence")]
    public bool RequiresEvidence { get; set; } = false;

    /// <summary>
    /// Additional metadata for the task template.
    /// </summary>
    [Display(Name = "Metadata")]
    public string? Metadata { get; set; }
}

/// <summary>
/// Detailed task template model with related entities and temporal tracking.
/// </summary>
public class TaskTemplateDetailModel : TaskTemplateModel, ITemporal
{
    /// <summary>
    /// The checklist template this task template belongs to.
    /// </summary>
    public ChecklistTemplateModel ChecklistTemplate { get; set; } = null!;

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
    //[Display(Name = "Deleted at")]
    //[Description("The date and time this record was deleted, or null, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    //public DateTime? DeletedAt { get; set; }

    #endregion ITemporal
}
