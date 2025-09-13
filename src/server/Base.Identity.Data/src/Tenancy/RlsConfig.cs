using System.Data.Common;

namespace Base.Data.Identity;

public class RlsConfig
{
    public static void SetTenantConfig(DbCommand cmd, Guid groupId, Guid tenantId)
    {
#if RESELLER
        SetGroupConfig(cmd, groupId);
        cmd.ExecuteScalar();

        // Clear parameters and set tenant context
        cmd.Parameters.Clear();
#endif

        SetTenantConfig(cmd, tenantId);
        cmd.ExecuteScalar();
    }

    public static async Task SetTenantConfigAsync(DbCommand cmd, Guid groupId, Guid tenantId, CancellationToken cancellationToken)
    {
#if RESELLER
        SetGroupConfig(cmd, groupId);
        await cmd.ExecuteScalarAsync(cancellationToken);

        // Clear parameters and set tenant context
        cmd.Parameters.Clear();
#endif

        SetTenantConfig(cmd, tenantId);
        await cmd.ExecuteScalarAsync(cancellationToken);
    }

    /// <summary>
    /// Sets the tenant context for the current transaction to enable Row Level Security.
    /// </summary>
    public static void SetGroupConfig(DbCommand cmd, Guid groupId)
    {
        // Use set_config(..., is_local := true) → transaction-scoped (auto-reset).
        // Parameterized to avoid injection; works across all Npgsql versions.
#if RESELLER
        cmd.CommandText = "select set_config('app.group_id', @group, true);";
        var groupParam = cmd.CreateParameter();
        groupParam.ParameterName = "@group";
        groupParam.Value = groupId.ToString(); // Convert GUID to string
        cmd.Parameters.Add(groupParam);
#endif
    }

    public static void SetTenantConfig(DbCommand cmd, Guid tenantId)
    {
        cmd.CommandText = "select set_config('app.tenant_id', @tenant, true);";
        var tenantParam = cmd.CreateParameter();
        tenantParam.ParameterName = "@tenant";
        tenantParam.Value = tenantId.ToString(); // Convert GUID to string
        cmd.Parameters.Add(tenantParam);
    }
}
