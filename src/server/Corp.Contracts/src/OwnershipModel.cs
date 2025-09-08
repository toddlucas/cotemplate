using System.ComponentModel.DataAnnotations;

namespace Corp;

/// <summary>
/// Represents an ownership relationship between a person and an entity.
/// </summary>
public class OwnershipModel
{
    /// <summary>
    /// The ownership ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The organization this ownership belongs to.
    /// </summary>
    [Display(Name = "Organization ID")]
    [Required]
    public long OrganizationId { get; set; }

    /// <summary>
    /// The person who owns the entity.
    /// </summary>
    [Display(Name = "Person ID")]
    [Required]
    public long PersonId { get; set; }

    /// <summary>
    /// The entity being owned.
    /// </summary>
    [Display(Name = "Entity ID")]
    [Required]
    public long EntityId { get; set; }

    /// <summary>
    /// The ownership percentage (0-100).
    /// </summary>
    [Display(Name = "Ownership Percentage")]
    [Range(0, 100)]
    public decimal OwnershipPercentage { get; set; }

    /// <summary>
    /// The ownership type (Member, Shareholder, Beneficiary, etc.).
    /// </summary>
    [Display(Name = "Ownership Type")]
    [StringLength(50)]
    public string? OwnershipType { get; set; }

    /// <summary>
    /// The effective date of this ownership.
    /// </summary>
    [Display(Name = "Effective Date")]
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// The end date of this ownership (if applicable).
    /// </summary>
    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Whether this ownership is active.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The number of shares/units owned (if applicable).
    /// </summary>
    [Display(Name = "Shares/Units")]
    public decimal? Shares { get; set; }

    /// <summary>
    /// The class of shares/units (if applicable).
    /// </summary>
    [Display(Name = "Share Class")]
    [StringLength(50)]
    public string? ShareClass { get; set; }

    /// <summary>
    /// Whether this person has voting rights.
    /// </summary>
    [Display(Name = "Voting Rights")]
    public bool HasVotingRights { get; set; } = true;

    /// <summary>
    /// The voting percentage (may differ from ownership percentage).
    /// </summary>
    [Display(Name = "Voting Percentage")]
    [Range(0, 100)]
    public decimal? VotingPercentage { get; set; }

    /// <summary>
    /// Notes about this ownership relationship.
    /// </summary>
    [Display(Name = "Notes")]
    [StringLength(1000)]
    public string? Notes { get; set; }
}

/// <summary>
/// Detailed ownership model with related entities and temporal tracking.
/// </summary>
public class OwnershipDetailModel : OwnershipModel, ITemporal
{
    /// <summary>
    /// The organization this ownership belongs to.
    /// </summary>
    public OrganizationModel Organization { get; set; } = null!;

    /// <summary>
    /// The person who owns the entity.
    /// </summary>
    public PersonModel Person { get; set; } = null!;

    /// <summary>
    /// The entity being owned.
    /// </summary>
    public EntityModel Entity { get; set; } = null!;

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
