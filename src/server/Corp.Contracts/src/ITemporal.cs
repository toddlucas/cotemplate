namespace Corp;

public interface ITemporal
{
    DateTime CreatedAt { get; }
    DateTime UpdatedAt { get; }
}

public interface ITemporalRecord : ITemporal
{
    DateTime? DeletedAt { get; }
}
