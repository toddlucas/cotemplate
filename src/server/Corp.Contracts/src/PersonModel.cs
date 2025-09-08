using System.ComponentModel.DataAnnotations;

namespace Corp;

/// <summary>
/// Represents a person (advisor, owner, member, etc.) in the system.
/// </summary>
public class PersonModel
{
    /// <summary>
    /// The person ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The organization this person belongs to.
    /// </summary>
    [Display(Name = "Organization ID")]
    [Required]
    public long OrganizationId { get; set; }

    /// <summary>
    /// The person's first name.
    /// </summary>
    [Display(Name = "First Name")]
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// The person's last name.
    /// </summary>
    [Display(Name = "Last Name")]
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// The person's middle name or initial.
    /// </summary>
    [Display(Name = "Middle Name")]
    [StringLength(100)]
    public string? MiddleName { get; set; }

    /// <summary>
    /// The person's email address.
    /// </summary>
    [Display(Name = "Email")]
    [EmailAddress]
    [StringLength(255)]
    public string? Email { get; set; }

    /// <summary>
    /// The person's phone number.
    /// </summary>
    [Display(Name = "Phone")]
    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }

    /// <summary>
    /// The person's title or role.
    /// </summary>
    [Display(Name = "Title")]
    [StringLength(100)]
    public string? Title { get; set; }

    /// <summary>
    /// The person's role type (Owner, Manager, Member, Advisor, etc.).
    /// </summary>
    [Display(Name = "Role Type")]
    [StringLength(50)]
    public string? RoleType { get; set; }

    /// <summary>
    /// The person's address.
    /// </summary>
    [Display(Name = "Address")]
    [StringLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// The person's date of birth.
    /// </summary>
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// The person's SSN (encrypted).
    /// </summary>
    [Display(Name = "SSN")]
    [StringLength(50)]
    public string? Ssn { get; set; }

    /// <summary>
    /// Whether this person is active.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether this person is an advisor (can work across organizations).
    /// </summary>
    [Display(Name = "Is Advisor")]
    public bool IsAdvisor { get; set; } = false;

    /// <summary>
    /// The person's notes or additional information.
    /// </summary>
    [Display(Name = "Notes")]
    [StringLength(2000)]
    public string? Notes { get; set; }
}

/// <summary>
/// Detailed person model with related entities and temporal tracking.
/// </summary>
public class PersonDetailModel : PersonModel, ITemporal
{
    /// <summary>
    /// The organization this person belongs to.
    /// </summary>
    public OrganizationModel Organization { get; set; } = null!;

    /// <summary>
    /// The ownership relationships for this person.
    /// </summary>
    public OwnershipModel[] Ownerships { get; set; } = [];

    /// <summary>
    /// The obligations/tasks assigned to this person.
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
