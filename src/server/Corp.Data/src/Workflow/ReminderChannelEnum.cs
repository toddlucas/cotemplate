namespace Corp.Workflow;

/// <summary>
/// Reminder channel enumeration class.
/// </summary>
public record ReminderChannelEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 10;

    public static ReminderChannelEnum Email => new(10, nameof(ReminderChannel.email), "Email");
    public static ReminderChannelEnum InApp => new(20, nameof(ReminderChannel.inapp), "In-App");
    public static ReminderChannelEnum Webhook => new(30, nameof(ReminderChannel.webhook), "Webhook");

    public static IEnumerable<ReminderChannelEnum> GetAll() => GetAll<ReminderChannelEnum>();
}
