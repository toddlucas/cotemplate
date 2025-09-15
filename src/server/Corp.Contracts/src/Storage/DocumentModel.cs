using Corp.Business;

namespace Corp.Storage;

/// <summary>
/// Represents a document in the system.
/// </summary>
public class DocumentModel
{
    /// <summary>
    /// The document ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The organization ID .
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
    /// The document title.
    /// </summary>
    [Display(Name = "Title")]
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// The document category. See <see cref="DocumentCategoryEnum"/>.
    /// </summary>
    [Display(Name = "Category")]
    [Required]
    [StringLength(20)]
    public string CategoryId { get; set; } = null!;

    /// <summary>
    /// The document storage URI.
    /// </summary>
    [Display(Name = "Storage URI")]
    [StringLength(500)]
    public string? StorageUri { get; set; }

    /// <summary>
    /// The document MIME type.
    /// </summary>
    [Display(Name = "MIME Type")]
    [StringLength(100)]
    public string? MimeType { get; set; }

    /// <summary>
    /// The document hash for integrity verification.
    /// </summary>
    [Display(Name = "Hash")]
    [StringLength(64)]
    public string? Hash { get; set; }

    /// <summary>
    /// The person who uploaded this document.
    /// </summary>
    [Display(Name = "Uploaded By")]
    [StringLength(255)]
    public long? UploadedBy { get; set; }

    /// <summary>
    /// The upload timestamp.
    /// </summary>
    [Display(Name = "Uploaded At")]
    public DateTime? UploadedAt { get; set; }

    /// <summary>
    /// Additional metadata for the document.
    /// </summary>
    [Display(Name = "Metadata")]
    public string? Metadata { get; set; }
}

/// <summary>
/// Detailed document model with related entities and temporal tracking.
/// </summary>
public class DocumentDetailModel : DocumentModel, ITemporal
{
    /// <summary>
    /// The organization this document belongs to.
    /// </summary>
    public OrganizationModel Organization { get; set; } = null!;

    /// <summary>
    /// The entity this document is associated with.
    /// </summary>
    public EntityModel? Entity { get; set; }

    /// <summary>
    /// The person this document is associated with.
    /// </summary>
    public PersonModel? Person { get; set; }

    /// <summary>
    /// The extracted fields for this document.
    /// </summary>
    public ExtractedFieldModel[] ExtractedFields { get; set; } = [];

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
