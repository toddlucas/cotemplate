Yes—this is a nice pattern. You can keep **one “read guard” per request** that:

1. lazily starts a short **READ ONLY** transaction before the first query,
2. auto-disposes it at request end, and
3. if a write is about to happen, it **closes the read tx and opens a write tx** first.

Below is a production-style minimal implementation for **EF Core + Npgsql**. It plays well with your existing **transaction interceptor** that sets `app.current_tenant` on every transaction.

# 1) The guard interface

```csharp
public interface IRequestDbGuard : IAsyncDisposable
{
    Task EnsureReadAsync(CancellationToken ct = default);   // start/keep a read tx
    Task EnsureWriteAsync(CancellationToken ct = default);  // switch to write tx if needed
}
```

# 2) Implementation (one DbContext per request)

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public sealed class RequestDbGuard<TDb> : IRequestDbGuard where TDb : DbContext
{
    private readonly TDb _db;
    private IDbContextTransaction? _tx;
    private bool _isReadOnly; // tracks current tx mode

    public RequestDbGuard(TDb db) => _db = db;

    public async Task EnsureReadAsync(CancellationToken ct = default)
    {
        if (_tx != null) return; // already in a tx (read or write) -> OK for reads
        _tx = await _db.Database.BeginTransactionAsync(ct);
        _isReadOnly = true;
        // Make the tx read-only at the DB level
        await _db.Database.ExecuteSqlRawAsync("SET TRANSACTION READ ONLY", ct);
        // Your TransactionStarted interceptor will set app.current_tenant here
    }

    public async Task EnsureWriteAsync(CancellationToken ct = default)
    {
        // Already write-capable? nothing to do
        if (_tx != null && !_isReadOnly) return;

        // If we are in a read-only tx, close it first
        if (_tx != null && _isReadOnly)
        {
            // reads don’t need to persist; either Commit or Rollback is fine—Rollback is slightly cheaper
            await _tx.RollbackAsync(ct);
            await _tx.DisposeAsync();
            _tx = null;
        }

        // Start a new write-capable tx
        _tx = await _db.Database.BeginTransactionAsync(ct);
        _isReadOnly = false;
        // (No READ ONLY here; writes allowed)
        // Tenant GUC set by your TransactionStarted interceptor again
    }

    public async ValueTask DisposeAsync()
    {
        if (_tx == null) return;
        try
        {
            // If it was a read-only guard and still open, just commit (or rollback) to release quickly
            if (_isReadOnly) await _tx.CommitAsync();
            else             await _tx.CommitAsync();
        }
        catch
        {
            try { await _tx.RollbackAsync(); } catch { /* swallow */ }
            throw;
        }
        finally
        {
            await _tx.DisposeAsync();
            _tx = null;
        }
    }
}
```

# 3) Wire it up (per-request scope)

```csharp
services.AddScoped<IRequestDbGuard, RequestDbGuard<AppDbContext>>();

// Optional: a small middleware to ensure disposal if you don’t use `await using`:
app.Use(async (ctx, next) =>
{
    await using var scope = ctx.RequestServices.CreateAsyncScope();
    ctx.Features.Set(scope); // if you want manual access later
    await next();
});
```

# 4) Use it in your controllers/services

**Reads only (guard starts a lightweight read tx):**

```csharp
public async Task<IReadOnlyList<InvoiceDto>> GetInvoices(
    [FromServices] AppDbContext db,
    [FromServices] IRequestDbGuard guard,
    CancellationToken ct)
{
    await guard.EnsureReadAsync(ct);

    return await db.Invoices
        .Where(i => /* … */)
        .AsNoTracking()
        .Select(i => new InvoiceDto { /* … */ })
        .ToListAsync(ct);
} // guard disposed at request end → tx ends
```

**A flow that *might* write (auto “promotion” by switching tx):**

```csharp
public async Task<IActionResult> MaybeUpdateAsync(
    AppDbContext db, IRequestDbGuard guard, CancellationToken ct)
{
    await guard.EnsureReadAsync(ct);

    var entity = await db.Entities.FirstOrDefaultAsync(/* … */, ct);
    if (entity is null) return NotFound();

    if (NeedsUpdate(entity))
    {
        await guard.EnsureWriteAsync(ct);               // <-- closes read tx, opens write tx
        entity.Status = "Updated";
        await db.SaveChangesAsync(ct);                  // write happens under write tx
    }

    return Ok();
}
```

> This gives you the behavior you described: you can begin in “read mode,” and only if a write is needed do you **dispose the read tx and start a write tx**.

# 5) Optional polish

* **Make writes automatic:** add an `ISaveChangesInterceptor` that calls `EnsureWriteAsync()` before any save:

  ```csharp
  public sealed class WriteGuardInterceptor : SaveChangesInterceptor
  {
      private readonly IRequestDbGuard _guard;
      public WriteGuardInterceptor(IRequestDbGuard guard) => _guard = guard;
      public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
          DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
      {
          await _guard.EnsureWriteAsync(ct);
          return result;
      }
  }
  ```

  Register with `options.AddInterceptors(new WriteGuardInterceptor(...))`.

* **Start late, end early:** call `EnsureReadAsync()` immediately before the first DB call; don’t wrap unrelated I/O.

* **Multiple DbContexts:** either give each its own guard (simple), or make a guard that controls multiple contexts (track a dictionary of `(DbContext -> tx)`).

* **Ambient/AsyncLocal variant:** if you prefer not to pass the guard around, expose it via `AsyncLocal<IRequestDbGuard>` and a small helper. Passing the interface explicitly is clearer and testable.

* **Transactions + RDS Proxy:** both read and write transactions will “pin” a backend for their lifetime. You’re minimizing this by (a) starting lazily and (b) ending on request completion or earlier on write.

# 6) Why this works well with your tenant RLS

* Your **TransactionStarted** interceptor sets `app.current_tenant` whenever the guard opens a tx (read or write).
* Read tx uses `SET TRANSACTION READ ONLY`, so it’s WAL-free and cheap.
* When you need to write, you close the read tx and open a fresh write tx—there’s no attempt to “promote,” which Postgres doesn’t support.

If you want, I can adapt this to a small NuGet-style package with an **extension method** like `services.AddRequestDbGuard<TDb>()` and an attribute you can place on controller actions to auto-ensure read before execution.
