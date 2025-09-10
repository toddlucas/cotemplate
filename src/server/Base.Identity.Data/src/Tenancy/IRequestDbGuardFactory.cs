using Corp.Identity;

namespace Corp.Data.Identity;

/// <summary>
/// Factory for creating IRequestDbGuard instances. This breaks circular dependencies
/// by deferring the creation of the guard until it's actually needed.
/// </summary>
public interface IRequestDbGuardFactory
{
    /// <summary>
    /// Creates a new IRequestDbGuard instance for the current request.
    /// </summary>
    IRequestDbGuard CreateGuard();
}
