using Corp.Business;
using Corp.Workflow;

namespace Corp;

/// <summary>
/// Represents an organization (customer-facing container) in the system.
/// </summary>
public class OrganizationModel
{
    /// <summary>
    /// The organization ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The organization name.
    /// </summary>
    [Display(Name = "Name")]
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The organization code (human identifier).
    /// </summary>
    [Display(Name = "Code")]
    [StringLength(50)]
    public string? Code { get; set; }

    /// <summary>
    /// The parent organization ID (for sub-organizations).
    /// </summary>
    [Display(Name = "Parent Organization ID")]
    public long? ParentOrgId { get; set; }

    /// <summary>
    /// The organization status.
    /// </summary>
    [Display(Name = "Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// Additional metadata for the organization.
    /// </summary>
    [Display(Name = "Metadata")]
    public string? Metadata { get; set; }
}

/// <summary>
/// Detailed organization model with related entities and temporal tracking.
/// </summary>
public class OrganizationDetailModel : OrganizationModel, ITemporal
{
    /// <summary>
    /// The entities belonging to this organization.
    /// </summary>
    public EntityModel[] Entities { get; set; } = [];

    /// <summary>
    /// The organization members.
    /// </summary>
    public OrganizationMemberModel[] Members { get; set; } = [];

    /// <summary>
    /// The tasks for this organization.
    /// </summary>
    public TaskModel[] Tasks { get; set; } = [];

    /// <summary>
    /// The checklist instances for this organization.
    /// </summary>
    public ChecklistModel[] ChecklistInstances { get; set; } = [];

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
