namespace Corp.Workflow;

/// <summary>
/// Priority enumeration class.
/// </summary>
public record PriorityEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 10;

    public static PriorityEnum Low => new(10, nameof(Priority.low), "Low");
    public static PriorityEnum Normal => new(20, nameof(Priority.normal), "Normal");
    public static PriorityEnum High => new(30, nameof(Priority.high), "High");

    public static IEnumerable<PriorityEnum> GetAll() => GetAll<PriorityEnum>();
}
