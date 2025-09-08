namespace Corp.Storage;

/// <summary>
/// Represents an extracted field from AI document processing.
/// </summary>
public class ExtractedFieldModel
{
    /// <summary>
    /// The extracted field ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The tenant ID this extracted field belongs to.
    /// </summary>
    [Display(Name = "Tenant ID")]
    [Required]
    public long TenantId { get; set; }

    /// <summary>
    /// The organization ID.
    /// </summary>
    [Display(Name = "Organization ID")]
    [Required]
    public long OrgId { get; set; }

    /// <summary>
    /// The document ID this field was extracted from.
    /// </summary>
    [Display(Name = "Document ID")]
    [Required]
    public long DocumentId { get; set; }

    /// <summary>
    /// The schema key for this field.
    /// </summary>
    [Display(Name = "Schema Key")]
    [Required]
    [StringLength(100)]
    public string SchemaKey { get; set; } = null!;

    /// <summary>
    /// The extracted text value.
    /// </summary>
    [Display(Name = "Value Text")]
    [StringLength(1000)]
    public string? ValueText { get; set; }

    /// <summary>
    /// The extracted numeric value.
    /// </summary>
    [Display(Name = "Value Number")]
    public decimal? ValueNumber { get; set; }

    /// <summary>
    /// The extracted date value.
    /// </summary>
    [Display(Name = "Value Date")]
    public DateTime? ValueDate { get; set; }

    /// <summary>
    /// The confidence score for this extraction (0-100).
    /// </summary>
    [Display(Name = "Confidence")]
    [Range(0, 100)]
    public decimal? Confidence { get; set; }

    /// <summary>
    /// The revision number for this extraction.
    /// </summary>
    [Display(Name = "Revision")]
    public int Revision { get; set; } = 1;

    /// <summary>
    /// Who created this extraction (system or user).
    /// </summary>
    [Display(Name = "Created By")]
    [StringLength(50)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Additional metadata for the extracted field.
    /// </summary>
    [Display(Name = "Metadata")]
    public string? Metadata { get; set; }
}

/// <summary>
/// Detailed extracted field model with related entities and temporal tracking.
/// </summary>
public class ExtractedFieldDetailModel : ExtractedFieldModel, ITemporal
{
    /// <summary>
    /// The organization this extracted field belongs to.
    /// </summary>
    public OrganizationModel Organization { get; set; } = null!;

    /// <summary>
    /// The document this field was extracted from.
    /// </summary>
    public DocumentModel Document { get; set; } = null!;

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
