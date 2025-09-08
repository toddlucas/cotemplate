using System.ComponentModel.DataAnnotations;

namespace Corp;

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
    /// The organization this document belongs to.
    /// </summary>
    [Display(Name = "Organization ID")]
    [Required]
    public long OrganizationId { get; set; }

    /// <summary>
    /// The entity this document is associated with (optional).
    /// </summary>
    [Display(Name = "Entity ID")]
    public long? EntityId { get; set; }

    /// <summary>
    /// The document name/title.
    /// </summary>
    [Display(Name = "Name")]
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The document type (Articles of Incorporation, Operating Agreement, etc.).
    /// </summary>
    [Display(Name = "Type")]
    [Required]
    [StringLength(100)]
    public string Type { get; set; } = null!;

    /// <summary>
    /// The document description.
    /// </summary>
    [Display(Name = "Description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// The document file name.
    /// </summary>
    [Display(Name = "File Name")]
    [StringLength(255)]
    public string? FileName { get; set; }

    /// <summary>
    /// The document file size in bytes.
    /// </summary>
    [Display(Name = "File Size")]
    public long? FileSize { get; set; }

    /// <summary>
    /// The document MIME type.
    /// </summary>
    [Display(Name = "MIME Type")]
    [StringLength(100)]
    public string? MimeType { get; set; }

    /// <summary>
    /// The document file path or storage location.
    /// </summary>
    [Display(Name = "File Path")]
    [StringLength(500)]
    public string? FilePath { get; set; }

    /// <summary>
    /// The document's external storage URL (if stored externally).
    /// </summary>
    [Display(Name = "Storage URL")]
    [Url]
    [StringLength(1000)]
    public string? StorageUrl { get; set; }

    /// <summary>
    /// The document's hash for integrity verification.
    /// </summary>
    [Display(Name = "Hash")]
    [StringLength(64)]
    public string? Hash { get; set; }

    /// <summary>
    /// The document's creation date (from the document itself).
    /// </summary>
    [Display(Name = "Document Date")]
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// The document's expiration date (if applicable).
    /// </summary>
    [Display(Name = "Expiration Date")]
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Whether this document is active.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The document's status (Draft, Final, Superseded, etc.).
    /// </summary>
    [Display(Name = "Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// The document's version number.
    /// </summary>
    [Display(Name = "Version")]
    [StringLength(20)]
    public string? Version { get; set; }

    /// <summary>
    /// Whether this document has been processed by AI extraction.
    /// </summary>
    [Display(Name = "AI Processed")]
    public bool IsAiProcessed { get; set; } = false;

    /// <summary>
    /// The confidence score from AI extraction (0-100).
    /// </summary>
    [Display(Name = "AI Confidence")]
    [Range(0, 100)]
    public decimal? AiConfidence { get; set; }

    /// <summary>
    /// The extracted data from AI processing (JSON).
    /// </summary>
    [Display(Name = "Extracted Data")]
    public string? ExtractedData { get; set; }

    /// <summary>
    /// Notes about this document.
    /// </summary>
    [Display(Name = "Notes")]
    [StringLength(2000)]
    public string? Notes { get; set; }
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
