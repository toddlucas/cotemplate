Nice—Metalama is a great fit. You can wrap the “read guard → switch to write” logic in compile-time aspects so your app code stays clean. Below are two workable patterns; pick the one that matches how you like to access scoped services.

---

# Pattern A (recommended): ambient, DI-safe, zero service locator

Use an **ambient accessor** (AsyncLocal) that the request middleware sets once. Your Metalama aspects then call the guard via this accessor.

### 1) The ambient accessor + guard (from earlier)

```csharp
public interface IRequestDbGuard : IAsyncDisposable
{
    Task EnsureReadAsync(CancellationToken ct = default);
    Task EnsureWriteAsync(CancellationToken ct = default);
}

public static class RequestGuard
{
    private static readonly AsyncLocal<IRequestDbGuard?> _current = new();
    public static IRequestDbGuard Current =>
        _current.Value ?? throw new InvalidOperationException("No IRequestDbGuard in scope.");
    public static IDisposable Use(IRequestDbGuard guard)
    {
        var prior = _current.Value;
        _current.Value = guard;
        return new Revert(() => _current.Value = prior);
        sealed class Revert : IDisposable { private readonly Action a; public Revert(Action a)=>this.a=a; public void Dispose()=>a(); }
    }
}
```

In ASP.NET Core middleware:

```csharp
app.Use(async (ctx, next) =>
{
    await using var scope = ctx.RequestServices.CreateAsyncScope();
    var guard = scope.ServiceProvider.GetRequiredService<IRequestDbGuard>();
    using (RequestGuard.Use(guard))
    {
        await next();
    }
    // guard disposed by DI at request end
});
```

### 2) Metalama aspects

```csharp
using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class TenantReadAttribute : OverrideMethodAspect
{
    public override async Task<dynamic?> OverrideMethodAsync()
    {
        await RequestGuard.Current.EnsureReadAsync();
        return await meta.ProceedAsync();
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class TenantWriteAttribute : OverrideMethodAspect
{
    public override async Task<dynamic?> OverrideMethodAsync()
    {
        await RequestGuard.Current.EnsureWriteAsync();
        return await meta.ProceedAsync();
    }
}
```

### 3) Apply by attribute or by convention

```csharp
// Attribute-based:
[TenantRead]                 // whole class defaults to read
public class InvoicesQueries { /* methods */ }

public class InvoicesCommands
{
    [TenantWrite]            // ensure write before executing
    public Task UpdateAsync(...) { ... }
}
```

Or automatically via a **fabric** (convention-based weaving):

```csharp
public class TenantFabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        amender.Outbound
            .SelectMany(t => t.Types)
            .SelectMany(t => t.Methods.Where(m => m.Name.StartsWith("Get") || m.Name.StartsWith("List")))
            .AddAspect(_ => new TenantReadAttribute());

        amender.Outbound
            .SelectMany(t => t.Types)
            .SelectMany(t => t.Methods.Where(m => m.Name is "Create" or "Update" or "Delete" or "Save"))
            .AddAspect(_ => new TenantWriteAttribute());
    }
}
```

> Pair this with the **EF `SaveChangesInterceptor`** that calls `EnsureWriteAsync()` just before `SaveChanges*` to catch any write path that wasn’t annotated.

---

# Pattern B: resolve the guard from DI inside aspects

If you prefer to inject from DI directly, use a static accessor to the request `IServiceProvider`, or Metalama’s DI helpers.

### 1) Request service provider accessor

```csharp
public static class RequestServices
{
    private static readonly AsyncLocal<IServiceProvider?> _current = new();
    public static IServiceProvider Current => _current.Value!;
    public static IDisposable Use(IServiceProvider sp) { /* same idea as RequestGuard.Use */ }
}
```

Middleware:

```csharp
app.Use(async (ctx, next) =>
{
    using (RequestServices.Use(ctx.RequestServices))
    {
        await next();
    }
});
```

### 2) Aspect resolving the guard

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class TenantReadAttribute : OverrideMethodAspect
{
    public override async Task<dynamic?> OverrideMethodAsync()
    {
        var guard = RequestServices.Current.GetRequiredService<IRequestDbGuard>();
        await guard.EnsureReadAsync();
        return await meta.ProceedAsync();
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class TenantWriteAttribute : OverrideMethodAspect
{
    public override async Task<dynamic?> OverrideMethodAsync()
    {
        var guard = RequestServices.Current.GetRequiredService<IRequestDbGuard>();
        await guard.EnsureWriteAsync();
        return await meta.ProceedAsync();
    }
}
```

> This pattern is still clean, but the ambient guard from **Pattern A** avoids resolving services in aspects and keeps testing simpler.

---

## How promotion works with these aspects

* Methods marked (or convention-matched) as **read**: `TenantReadAttribute` starts a tiny **READ ONLY** transaction via the guard before your code runs.
* If that flow calls `SaveChanges*`, your **`SaveChangesInterceptor`** (registered in EF) executes `EnsureWriteAsync()` which **rolls back** the read tx and opens a write tx.
* Methods marked as **write**: `TenantWriteAttribute` opens a write tx up front (skipping the read tx entirely).

This mirrors your desired semantics: **begin in read mode, switch to write only when necessary**, without leaking session state and while still triggering your **TransactionStarted** interceptor that sets `app.current_tenant` for RLS.

---

## Tips & gotchas

* Keep aspects **idempotent**: `EnsureReadAsync()` should be a no-op if a tx is already open (read or write). `EnsureWriteAsync()` should close a read tx and open a write tx if needed, else no-op.
* Keep transactions **short**: call `EnsureReadAsync()` as late as possible (just before DB work)—your aspect naturally does this at method entry.
* If you compose multiple aspects, set `AspectPriority` so `TenantWrite` runs before `TenantRead` if both get applied accidentally.
* Testing: expose a fake `IRequestDbGuard` and verify that read methods call `EnsureReadAsync()` and write methods cause `EnsureWriteAsync()` (or via `SaveChangesInterceptor`).

---

If you tell me whether you want **attribute-driven** or **convention-driven** weaving (fabric), I can hand you a ready-to-drop `.cs` file with the aspects, the fabric, and the DI wiring that matches your project layout.
