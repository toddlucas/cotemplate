using Corp.Data.Identity;

namespace Corp.Data.Identity;

/// <summary>
/// Provides ambient access to the current request's database guard.
/// This allows Metalama aspects to access the guard without requiring dependency injection.
/// </summary>
public static class AmbientRequestGuard
{
    private static readonly AsyncLocal<IRequestDbGuard?> _current = new();

    /// <summary>
    /// Gets the current request's database guard.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no guard is available in the current context.</exception>
    public static IRequestDbGuard Current =>
        _current.Value ?? throw new InvalidOperationException("No IRequestDbGuard available in the current request context. Ensure the request middleware is properly configured.");

    /// <summary>
    /// Sets the current request's database guard.
    /// This should be called by the request middleware at the start of each request.
    /// </summary>
    /// <param name="guard">The database guard for the current request</param>
    /// <returns>A disposable that will restore the previous guard when disposed</returns>
    public static IDisposable Use(IRequestDbGuard guard)
    {
        var previous = _current.Value;
        _current.Value = guard;
        return new GuardScope(previous);
    }

    /// <summary>
    /// Checks if a database guard is available in the current context.
    /// </summary>
    public static bool IsAvailable => _current.Value != null;

    private sealed class GuardScope : IDisposable
    {
        private readonly IRequestDbGuard? _previous;

        public GuardScope(IRequestDbGuard? previous)
        {
            _previous = previous;
        }

        public void Dispose()
        {
            _current.Value = _previous;
        }
    }
}
