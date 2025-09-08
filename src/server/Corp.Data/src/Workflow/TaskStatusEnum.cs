namespace Corp.Workflow;

/// <summary>
/// Task status enumeration class.
/// </summary>
public record TaskStatusEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 20;

    public static TaskStatusEnum Todo => new(10, nameof(TaskStatus.todo), "To Do");
    public static TaskStatusEnum InProgress => new(20, nameof(TaskStatus.in_progress), "In Progress");
    public static TaskStatusEnum Blocked => new(30, nameof(TaskStatus.blocked), "Blocked");
    public static TaskStatusEnum Done => new(40, nameof(TaskStatus.done), "Done");
    public static TaskStatusEnum Skipped => new(50, nameof(TaskStatus.skipped), "Skipped");

    public static IEnumerable<TaskStatusEnum> GetAll() => GetAll<TaskStatusEnum>();
}
