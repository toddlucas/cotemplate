namespace Corp;

public interface ITemporal
{
    DateTime CreatedAt { get; }
    DateTime UpdatedAt { get; }
    DateTime? DeletedAt { get; }
}
