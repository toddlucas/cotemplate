namespace Corp.Storage;

/// <summary>
/// Document category enumeration class.
/// </summary>
public record DocumentCategoryEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 20;

    public static DocumentCategoryEnum Formation => new(10, nameof(DocumentCategory.formation), "Formation");
    public static DocumentCategoryEnum Compliance => new(20, nameof(DocumentCategory.compliance), "Compliance");
    public static DocumentCategoryEnum Tax => new(30, nameof(DocumentCategory.tax), "Tax");
    public static DocumentCategoryEnum Contract => new(40, nameof(DocumentCategory.contract), "Contract");
    public static DocumentCategoryEnum Identifier => new(50, nameof(DocumentCategory.id), "ID"); // NOTE: Id hides inherited member.
    public static DocumentCategoryEnum Other => new(60, nameof(DocumentCategory.other), "Other");

    public static IEnumerable<DocumentCategoryEnum> GetAll() => GetAll<DocumentCategoryEnum>();
}
