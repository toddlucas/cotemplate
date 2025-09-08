using System.ComponentModel.DataAnnotations;

namespace Corp;

/// <summary>
/// Represents an organization (tenant/white-label container) in the system.
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
    /// The organization description.
    /// </summary>
    [Display(Name = "Description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// The organization's primary contact email.
    /// </summary>
    [Display(Name = "Contact Email")]
    [EmailAddress]
    [StringLength(255)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// The organization's website URL.
    /// </summary>
    [Display(Name = "Website")]
    [Url]
    [StringLength(500)]
    public string? Website { get; set; }

    /// <summary>
    /// The organization's timezone.
    /// </summary>
    [Display(Name = "Timezone")]
    [StringLength(50)]
    public string? Timezone { get; set; }

    /// <summary>
    /// Whether this organization is active.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The organization's branding theme (for white-label customization).
    /// </summary>
    [Display(Name = "Theme")]
    [StringLength(100)]
    public string? Theme { get; set; }

    /// <summary>
    /// The organization's custom domain (for white-label deployments).
    /// </summary>
    [Display(Name = "Custom Domain")]
    [StringLength(255)]
    public string? CustomDomain { get; set; }
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
    /// The people/advisors associated with this organization.
    /// </summary>
    public PersonModel[] People { get; set; } = [];

    /// <summary>
    /// The obligations/tasks for this organization.
    /// </summary>
    public ObligationModel[] Obligations { get; set; } = [];

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
