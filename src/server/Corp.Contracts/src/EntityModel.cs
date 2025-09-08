using System.ComponentModel.DataAnnotations;

namespace Corp;

/// <summary>
/// Represents a business entity (LLC, Corporation, Trust, etc.) in the system.
/// </summary>
public class EntityModel
{
    /// <summary>
    /// The entity ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The organization this entity belongs to.
    /// </summary>
    [Display(Name = "Organization ID")]
    [Required]
    public long OrganizationId { get; set; }

    /// <summary>
    /// The entity name.
    /// </summary>
    [Display(Name = "Name")]
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The entity type (LLC, Corporation, Trust, Partnership, etc.).
    /// </summary>
    [Display(Name = "Type")]
    [Required]
    [StringLength(50)]
    public string Type { get; set; } = null!;

    /// <summary>
    /// The entity's legal name (may differ from display name).
    /// </summary>
    [Display(Name = "Legal Name")]
    [StringLength(200)]
    public string? LegalName { get; set; }

    /// <summary>
    /// The entity's state of formation.
    /// </summary>
    [Display(Name = "State")]
    [StringLength(2)]
    public string? State { get; set; }

    /// <summary>
    /// The entity's country of formation.
    /// </summary>
    [Display(Name = "Country")]
    [StringLength(2)]
    public string? Country { get; set; } = "US";

    /// <summary>
    /// The entity's formation date.
    /// </summary>
    [Display(Name = "Formation Date")]
    public DateTime? FormationDate { get; set; }

    /// <summary>
    /// The entity's EIN/Tax ID.
    /// </summary>
    [Display(Name = "EIN")]
    [StringLength(20)]
    public string? Ein { get; set; }

    /// <summary>
    /// The entity's registered agent name.
    /// </summary>
    [Display(Name = "Registered Agent")]
    [StringLength(200)]
    public string? RegisteredAgent { get; set; }

    /// <summary>
    /// The entity's registered agent address.
    /// </summary>
    [Display(Name = "Registered Agent Address")]
    [StringLength(500)]
    public string? RegisteredAgentAddress { get; set; }

    /// <summary>
    /// The entity's business address.
    /// </summary>
    [Display(Name = "Business Address")]
    [StringLength(500)]
    public string? BusinessAddress { get; set; }

    /// <summary>
    /// The entity's business purpose/description.
    /// </summary>
    [Display(Name = "Purpose")]
    [StringLength(1000)]
    public string? Purpose { get; set; }

    /// <summary>
    /// Whether this entity is active.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The entity's status (Active, Inactive, Dissolved, etc.).
    /// </summary>
    [Display(Name = "Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// The entity's dissolution date (if applicable).
    /// </summary>
    [Display(Name = "Dissolution Date")]
    public DateTime? DissolutionDate { get; set; }
}

/// <summary>
/// Detailed entity model with related entities and temporal tracking.
/// </summary>
public class EntityDetailModel : EntityModel, ITemporal
{
    /// <summary>
    /// The organization this entity belongs to.
    /// </summary>
    public OrganizationModel Organization { get; set; } = null!;

    /// <summary>
    /// The ownership relationships for this entity.
    /// </summary>
    public OwnershipModel[] Ownerships { get; set; } = [];

    /// <summary>
    /// The documents associated with this entity.
    /// </summary>
    public DocumentModel[] Documents { get; set; } = [];

    /// <summary>
    /// The obligations/tasks for this entity.
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
