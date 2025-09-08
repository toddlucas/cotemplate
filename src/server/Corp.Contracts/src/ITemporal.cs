namespace Corp;

public interface ITemporal
{
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset UpdatedAt { get; }
    DateTimeOffset? DeletedAt { get; }
}
