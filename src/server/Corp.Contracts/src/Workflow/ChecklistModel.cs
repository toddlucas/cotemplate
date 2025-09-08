using Corp.Business;

namespace Corp.Workflow;

/// <summary>
/// Represents a checklist instance.
/// </summary>
public class ChecklistModel
{
    /// <summary>
    /// The checklist instance ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The tenant ID this checklist instance belongs to.
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
    /// The entity ID (optional).
    /// </summary>
    [Display(Name = "Entity ID")]
    public long? EntityId { get; set; }

    /// <summary>
    /// The person ID (optional).
    /// </summary>
    [Display(Name = "Person ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// The template ID (optional).
    /// </summary>
    [Display(Name = "Template ID")]
    public long? TemplateId { get; set; }

    /// <summary>
    /// The checklist instance name.
    /// </summary>
    [Display(Name = "Name")]
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The checklist instance status. See <see cref="ChecklistStatusEnum"/>.
    /// </summary>
    [Display(Name = "Status")]
    [Required]
    [StringLength(10)]
    public string StatusId { get; set; } = null!;

    /// <summary>
    /// How this checklist was created. See <see cref="SourceTypeEnum"/>.
    /// </summary>
    [Display(Name = "Created From")]
    [Required]
    [StringLength(10)]
    public string CreatedFromId { get; set; } = null!;

    /// <summary>
    /// Additional metadata for the checklist instance.
    /// </summary>
    [Display(Name = "Metadata")]
    public string? Metadata { get; set; }
}

/// <summary>
/// Detailed checklist model with related entities and temporal tracking.
/// </summary>
public class ChecklistDetailModel : ChecklistModel, ITemporal
{
    /// <summary>
    /// The organization this checklist instance belongs to.
    /// </summary>
    public OrganizationModel Organization { get; set; } = null!;

    /// <summary>
    /// The entity this checklist instance is associated with.
    /// </summary>
    public EntityModel? Entity { get; set; }

    /// <summary>
    /// The person this checklist instance is associated with.
    /// </summary>
    public PersonModel? Person { get; set; }

    /// <summary>
    /// The template this checklist instance was created from.
    /// </summary>
    public ChecklistTemplateModel? Template { get; set; }

    /// <summary>
    /// The tasks for this checklist instance.
    /// </summary>
    public TaskModel[] Tasks { get; set; } = [];

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
