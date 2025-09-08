namespace Corp.Operations;

/// <summary>
/// Audit action enumeration class.
/// </summary>
public record AuditActionEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 20;

    public static AuditActionEnum Create => new(10, nameof(AuditAction.create), "Create");
    public static AuditActionEnum Read => new(20, nameof(AuditAction.read), "Read");
    public static AuditActionEnum Update => new(30, nameof(AuditAction.update), "Update");
    public static AuditActionEnum Delete => new(40, nameof(AuditAction.delete), "Delete");
    public static AuditActionEnum Login => new(50, nameof(AuditAction.login), "Login");
    public static AuditActionEnum Logout => new(60, nameof(AuditAction.logout), "Logout");
    public static AuditActionEnum AiAction => new(70, nameof(AuditAction.ai_action), "AI Action");
    public static AuditActionEnum Other => new(80, nameof(AuditAction.other), "Other");

    public static IEnumerable<AuditActionEnum> GetAll() => GetAll<AuditActionEnum>();
}
