namespace Corp.Workflow;

/// <summary>
/// Reminder status enumeration class.
/// </summary>
public record ReminderStatusEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 10;

    public static ReminderStatusEnum Scheduled => new(10, nameof(ReminderStatus.scheduled), "Scheduled");
    public static ReminderStatusEnum Sent => new(20, nameof(ReminderStatus.sent), "Sent");
    public static ReminderStatusEnum Failed => new(30, nameof(ReminderStatus.failed), "Failed");

    public static IEnumerable<ReminderStatusEnum> GetAll() => GetAll<ReminderStatusEnum>();
}
