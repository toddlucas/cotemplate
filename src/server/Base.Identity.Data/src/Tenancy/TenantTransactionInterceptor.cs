using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;

using Corp.Identity;

namespace Corp.Data.Identity;

/// <summary>
/// EF Core interceptor that sets the tenant ID for each database transaction.
/// This ensures that Row Level Security (RLS) policies can access the current tenant context.
/// </summary>
public sealed class TenantTransactionInterceptor : IDbTransactionInterceptor
{
    private readonly TenantContext<string> _tenantContext;

    public TenantTransactionInterceptor(TenantContext<string> tenantContext)
    {
        _tenantContext = tenantContext;
    }

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
        if (connection?.State == ConnectionState.Open &&
            connection is NpgsqlConnection npgConn &&
            npgConn.FullState.HasFlag(ConnectionState.Open))
        {
            if (connection.BeginTransaction() == eventData.Transaction) { /* no-op */ }
            // When EF attaches to an existing ambient transaction, set the tenant too:
            await SetTenantAsync(eventData.Transaction!, cancellationToken);
        }
    }

    private async Task SetTenantAsync(DbTransaction transaction, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.CurrentId;
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
        await cmd.ExecuteScalarAsync(cancellationToken);
    }

    // Synchronous versions for completeness (though async is preferred)
    public void TransactionStarted(DbTransaction transaction, TransactionEndEventData eventData)
    {
        SetTenant(transaction);
    }

    public void TransactionUsed(DbConnection connection, TransactionEventData eventData)
    {
        if (connection?.State == ConnectionState.Open &&
            connection is NpgsqlConnection npgConn &&
            npgConn.FullState.HasFlag(ConnectionState.Open))
        {
            if (connection.BeginTransaction() == eventData.Transaction) { /* no-op */ }
            // When EF attaches to an existing ambient transaction, set the tenant too:
            SetTenant(eventData.Transaction!);
        }
    }

    private void SetTenant(DbTransaction transaction)
    {
        var tenantId = _tenantContext.CurrentId;
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
        cmd.ExecuteScalar();
    }
}
