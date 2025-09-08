namespace Corp.Workflow;

/// <summary>
/// Source type enumeration class.
/// </summary>
public record SourceTypeEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 10;

    public static SourceTypeEnum System => new(10, nameof(SourceType.system), "System");
    public static SourceTypeEnum Ai => new(20, nameof(SourceType.ai), "AI");
    public static SourceTypeEnum Custom => new(30, nameof(SourceType.custom), "Custom");

    public static IEnumerable<SourceTypeEnum> GetAll() => GetAll<SourceTypeEnum>();
}
