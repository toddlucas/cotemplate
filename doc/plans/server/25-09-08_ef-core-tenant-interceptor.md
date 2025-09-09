Awesome—EF Core + Npgsql is a great fit. Here’s a robust pattern that (a) sets the tenant **per transaction** using a safe GUC, (b) avoids leakage with connection pooling, and (c) plays nicely with RLS.

# Overview

* Store the request’s tenant id in a scoped service (e.g., `ITenantProvider`).
* Use an **EF Core interceptor** to run `SELECT set_config('app.current_tenant', $1, true)` at the start of every DB **transaction**. (`true` → transaction-scoped = resets automatically on commit/rollback.)
* Make sure your app runs inside transactions when needed (EF does this for `SaveChanges*`; for read queries you can add a light read-only transaction if you want RLS protection there too).
* Keep your DB objects owned by a migration/admin role; your app connects as a non-owner role so RLS applies.

---

## 1) Scoped tenant provider

```csharp
public interface ITenantProvider
{
    string? CurrentTenantId { get; }  // store as string/Guid.ToString()
}

public sealed class HttpTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _http;
    public HttpTenantProvider(IHttpContextAccessor http) => _http = http;

    public string? CurrentTenantId =>
        _http.HttpContext?.Items["TenantId"] as string; // set this in middleware after auth
}
```

Register:

```csharp
services.AddHttpContextAccessor();
services.AddScoped<ITenantProvider, HttpTenantProvider>();
```

Your auth/tenant-resolution middleware should put a canonical UUID string into `HttpContext.Items["TenantId"]`.

---

## 2) EF Core interceptor (sets `app.current_tenant` per transaction)

We hook `IDbTransactionInterceptor.TransactionStarted` and `TransactionUsed` (the latter covers ambient/externally-created transactions).

```csharp
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;

public sealed class TenantTransactionInterceptor : IDbTransactionInterceptor
{
    private readonly ITenantProvider _tenant;

    public TenantTransactionInterceptor(ITenantProvider tenant) => _tenant = tenant;

    public async Task TransactionStartedAsync(
        DbTransaction transaction,
        TransactionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        await SetTenantAsync(transaction, cancellationToken);
    }

    public async Task TransactionUsedAsync(
        DbConnection connection,
        TransactionEventData eventData,
        CancellationToken cancellationToken = default)
    {
        if (connection?.State == System.Data.ConnectionState.Open &&
            connection is NpgsqlConnection npgConn &&
            npgConn.FullState.HasFlag(NpgsqlConnection.FullState.Open))
        {
            if (connection.BeginTransaction() == eventData.Transaction) { /* no-op */ }
            // When EF attaches to an existing ambient transaction, set the tenant too:
            await SetTenantAsync(eventData.Transaction!, cancellationToken);
        }
    }

    private async Task SetTenantAsync(DbTransaction transaction, CancellationToken ct)
    {
        var tenantId = _tenant.CurrentTenantId;
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new InvalidOperationException("No tenant id available for this request.");

        // Use set_config(..., is_local := true) → transaction-scoped (auto-reset).
        // Parameterized to avoid injection; works across all Npgsql versions.
        var cmd = transaction.Connection!.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "select set_config('app.current_tenant', @tenant, true);";
        var p = cmd.CreateParameter();
        p.ParameterName = "@tenant";
        p.Value = tenantId; // canonical UUID string
        cmd.Parameters.Add(p);
        await cmd.ExecuteScalarAsync(ct);
    }
}
```

Register the interceptor with your DbContext:

```csharp
services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(
        sp.GetRequiredService<IConfiguration>().GetConnectionString("Default"));
    options.AddInterceptors(sp.GetRequiredService<TenantTransactionInterceptor>());
});
services.AddScoped<TenantTransactionInterceptor>();
```

---

## 3) Ensure reads also run with a tenant set

`SaveChanges*` already runs in a transaction, so the interceptor fires. For plain `ToListAsync()`/read queries, EF Core **does not** open a transaction by default. You have three good options:

**Option A (simple, recommended):** Wrap read operations you care about in a short read-only transaction:

```csharp
await using var tx = await db.Database.BeginTransactionAsync(); // read-only with RLS
var rows = await db.Invoices.Where(...).ToListAsync();
await tx.CommitAsync();
```

**Option B (repository/unit-of-work layer):** Start a transaction per request (read-only unless writes occur). This guarantees the tenant is set for everything, at the cost of a small overhead.

**Option C (command interceptor alternative):** Use `IDbCommandInterceptor` to inject the `set_config` before the **first** command on a connection if no transaction is present, and immediately issue `reset_config('app.current_tenant')` afterward. This avoids long-lived session state, but is more moving parts. Most teams prefer transaction-scoped via Option A/B.

---

## 4) Npgsql pooling & RDS Proxy notes

* Using `set_config(..., true)` is **transaction-scoped**, so the GUC resets automatically—no leakage between requests, even with connection pooling.
* Any `SET`-ish state can cause **RDS Proxy pinning** for the lifetime of the transaction; keep transactions short and avoid temp tables/prepared statements if you need maximum multiplexing.
* If you later move to `NpgsqlDataSource`, you can still register the same EF interceptor; don’t set tenant in `SessionInitializationSql` because tenant varies per request.

---

## 5) Minimal middleware to capture tenant id (example)

```csharp
public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    public TenantResolutionMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        // Example: from JWT claim "tid" or header "X-Tenant-Id"
        var tid = ctx.User.FindFirst("tid")?.Value
                  ?? ctx.Request.Headers["X-Tenant-Id"].ToString();

        // validate as GUID
        if (!Guid.TryParse(tid, out var guid))
            throw new UnauthorizedAccessException("Invalid or missing tenant id.");

        ctx.Items["TenantId"] = guid.ToString();
        await _next(ctx);
    }
}
```

Register:

```csharp
app.UseMiddleware<TenantResolutionMiddleware>();
```

---

## 6) Recap of the DB side (for completeness)

* Every tenant-scoped table has `tenant_id uuid not null`.
* RLS policy:

```sql
ALTER TABLE public.invoice ENABLE ROW LEVEL SECURITY;

CREATE POLICY invoice_tenant_isolation ON public.invoice
USING (tenant_id = current_setting('app.current_tenant', true)::uuid)
WITH CHECK (tenant_id = current_setting('app.current_tenant', true)::uuid);

-- Defense-in-depth default for inserts:
ALTER TABLE public.invoice
  ALTER COLUMN tenant_id SET DEFAULT current_setting('app.current_tenant', true)::uuid;
```

* App connects as a non-owner role; owners are for migrations/admin only.

---

## 7) Quick test

1. Start a transaction, set tenant to A via your interceptor, try `SELECT * FROM invoice WHERE TRUE;` → only A’s rows.
2. In a new request/tenant B, repeat → only B’s rows.
3. Attempt cross-tenant `INSERT`/`UPDATE` → should fail due to `WITH CHECK`.

---

If you want, I can also drop in an **`IDbCommandInterceptor`** variant that handles non-transactional reads automatically (sets then resets the GUC around each command), but the transaction-scoped approach above is the cleanest and hardest to misuse.
