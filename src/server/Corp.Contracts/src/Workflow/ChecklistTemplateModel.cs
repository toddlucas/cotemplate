namespace Corp.Workflow;

/// <summary>
/// Represents a checklist template (AI or manual source).
/// </summary>
public class ChecklistTemplateModel
{
    /// <summary>
    /// The checklist template ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The tenant ID this checklist template belongs to.
    /// </summary>
    [Display(Name = "Tenant ID")]
    [Required]
    public long TenantId { get; set; }

    /// <summary>
    /// The checklist scope. See <see cref="ChecklistScopeEnum"/>.
    /// </summary>
    [Display(Name = "Scope")]
    [Required]
    [StringLength(10)]
    public string ScopeId { get; set; } = null!;

    /// <summary>
    /// The checklist template name.
    /// </summary>
    [Display(Name = "Name")]
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The checklist template version.
    /// </summary>
    [Display(Name = "Version")]
    [StringLength(20)]
    public string? Version { get; set; }

    /// <summary>
    /// The source type for this template. See <see cref="SourceTypeEnum"/>.
    /// </summary>
    [Display(Name = "Source Type")]
    [Required]
    [StringLength(10)]
    public string SourceTypeId { get; set; } = null!;

    /// <summary>
    /// The jurisdiction country (optional).
    /// </summary>
    [Display(Name = "Jurisdiction Country")]
    [StringLength(2)]
    public string? JurisdictionCountry { get; set; }

    /// <summary>
    /// The jurisdiction region (optional).
    /// </summary>
    [Display(Name = "Jurisdiction Region")]
    [StringLength(10)]
    public string? JurisdictionRegion { get; set; }

    /// <summary>
    /// The conditions this template applies to (JSONB).
    /// </summary>
    [Display(Name = "Applies To")]
    public string? AppliesTo { get; set; }

    /// <summary>
    /// Additional metadata for the checklist template.
    /// </summary>
    [Display(Name = "Metadata")]
    public string? Metadata { get; set; }
}

/// <summary>
/// Detailed checklist template model with related entities and temporal tracking.
/// </summary>
public class ChecklistTemplateDetailModel : ChecklistTemplateModel, ITemporal
{
    /// <summary>
    /// The task templates for this checklist template.
    /// </summary>
    public TaskTemplateModel[] TaskTemplates { get; set; } = [];

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
