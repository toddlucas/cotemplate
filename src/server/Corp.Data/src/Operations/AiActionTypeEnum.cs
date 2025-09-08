namespace Corp.Operations;

/// <summary>
/// AI action type enumeration class.
/// </summary>
public record AiActionTypeEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 20;

    public static AiActionTypeEnum CreateEntity => new(10, nameof(AiActionType.create_entity), "Create Entity");
    public static AiActionTypeEnum CreateTask => new(20, nameof(AiActionType.create_task), "Create Task");
    public static AiActionTypeEnum Summarize => new(30, nameof(AiActionType.summarize), "Summarize");
    public static AiActionTypeEnum Extract => new(40, nameof(AiActionType.extract), "Extract");
    public static AiActionTypeEnum Classify => new(50, nameof(AiActionType.classify), "Classify");
    public static AiActionTypeEnum Other => new(60, nameof(AiActionType.other), "Other");

    public static IEnumerable<AiActionTypeEnum> GetAll() => GetAll<AiActionTypeEnum>();
}
