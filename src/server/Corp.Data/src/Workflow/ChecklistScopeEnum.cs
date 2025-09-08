namespace Corp.Workflow;

/// <summary>
/// Checklist scope enumeration class.
/// </summary>
public record ChecklistScopeEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 10;

    public static ChecklistScopeEnum Org => new(10, nameof(ChecklistScope.org), "Organization");
    public static ChecklistScopeEnum Entity => new(20, nameof(ChecklistScope.entity), "Entity");
    public static ChecklistScopeEnum Person => new(30, nameof(ChecklistScope.person), "Person");

    public static IEnumerable<ChecklistScopeEnum> GetAll() => GetAll<ChecklistScopeEnum>();
}
