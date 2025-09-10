using Microsoft.EntityFrameworkCore.Diagnostics;

using Corp.Data.Identity;

namespace Corp.Data.Identity;

/// <summary>
/// EF Core interceptor that automatically promotes read transactions to write transactions
/// when SaveChanges is called. This ensures that write operations always have the proper
/// transaction context without manual intervention.
/// </summary>
public sealed class WriteGuardInterceptor : SaveChangesInterceptor
{
    private readonly IRequestDbGuardFactory _guardFactory;

    public WriteGuardInterceptor(IRequestDbGuardFactory guardFactory)
    {
        _guardFactory = guardFactory ?? throw new ArgumentNullException(nameof(guardFactory));
    }

    /// <summary>
    /// Intercepts SaveChangesAsync calls to ensure a write transaction is active.
    /// </summary>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var guard = _guardFactory.CreateGuard();
        await guard.EnsureWriteAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// Intercepts SaveChanges calls to ensure a write transaction is active.
    /// </summary>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var guard = _guardFactory.CreateGuard();
        guard.EnsureWriteAsync().GetAwaiter().GetResult();
        return result;
    }
}
