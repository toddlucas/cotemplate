namespace Corp.Operations;

/// <summary>
/// Feedback target type enumeration class.
/// </summary>
public record FeedbackTargetTypeEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 20;

    public static FeedbackTargetTypeEnum Task => new(10, nameof(FeedbackTargetType.task), "Task");
    public static FeedbackTargetTypeEnum Checklist => new(20, nameof(FeedbackTargetType.checklist), "Checklist");
    public static FeedbackTargetTypeEnum Document => new(30, nameof(FeedbackTargetType.document), "Document");
    public static FeedbackTargetTypeEnum Extraction => new(40, nameof(FeedbackTargetType.extraction), "Extraction");
    public static FeedbackTargetTypeEnum Summary => new(50, nameof(FeedbackTargetType.summary), "Summary");

    public static IEnumerable<FeedbackTargetTypeEnum> GetAll() => GetAll<FeedbackTargetTypeEnum>();
}
