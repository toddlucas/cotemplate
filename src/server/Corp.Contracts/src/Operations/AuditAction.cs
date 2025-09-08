namespace Corp.Operations;

/// <summary>
/// Audit action types.
/// </summary>
public enum AuditAction
{
    create,
    read,
    update,
    delete,
    login,
    logout,
    ai_action,
    other
}
