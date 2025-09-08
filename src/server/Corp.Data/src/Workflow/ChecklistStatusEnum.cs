namespace Corp.Workflow;

/// <summary>
/// Checklist status enumeration class.
/// </summary>
public record ChecklistStatusEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 10;

    public static ChecklistStatusEnum Draft => new(10, nameof(ChecklistStatus.draft), "Draft");
    public static ChecklistStatusEnum Active => new(20, nameof(ChecklistStatus.active), "Active");
    public static ChecklistStatusEnum Archived => new(30, nameof(ChecklistStatus.archived), "Archived");

    public static IEnumerable<ChecklistStatusEnum> GetAll() => GetAll<ChecklistStatusEnum>();
}
