using Microsoft.EntityFrameworkCore.Migrations;

namespace Corp.Data;

/// <summary>
/// Manages Row-Level Security (RLS) policies for PostgreSQL.
/// This class provides methods to enable/disable RLS and create/remove policies
/// for tables with group_id/tenant_id or tenant_id only patterns.
/// </summary>
public static class RlsPolicyManager
{
    /// <summary>
    /// Tables that have both group_id and tenant_id columns.
    /// These tables are scoped by both reseller (group) and customer (tenant).
    /// </summary>
    private static readonly string[] TablesWithGroupAndTenant = new[]
    {
        "organization",
        "entity",
        "entity_relationship",
        "entity_role",
        "organization_member",
        "document",
        "extracted_field",
        "checklist",
        "task_record",
        "task_template"
    };

    /// <summary>
    /// Tables that have only tenant_id column.
    /// These tables are scoped only by customer (tenant).
    /// </summary>
    private static readonly string[] TablesWithTenantOnly = new[]
    {
        "person",
        "checklist_template"
    };

    /// <summary>
    /// Enables RLS and creates policies for all tables.
    /// Call this from your migration's Up() method.
    /// </summary>
    /// <param name="migrationBuilder">The migration builder instance</param>
    public static void EnableRls(MigrationBuilder migrationBuilder)
    {
        // Create the policy functions first
        CreatePolicyFunctions(migrationBuilder);

        // Enable RLS and create policies for tables with both group_id and tenant_id
        foreach (var table in TablesWithGroupAndTenant)
        {
            migrationBuilder.Sql($"ALTER TABLE {table} ENABLE ROW LEVEL SECURITY;");
            migrationBuilder.Sql($"CREATE POLICY rls_policy ON {table} FOR ALL USING (rls_group_tenant_policy(group_id, tenant_id));");
        }

        // Enable RLS and create policies for tables with tenant_id only
        foreach (var table in TablesWithTenantOnly)
        {
            migrationBuilder.Sql($"ALTER TABLE {table} ENABLE ROW LEVEL SECURITY;");
            migrationBuilder.Sql($"CREATE POLICY rls_policy ON {table} FOR ALL USING (rls_tenant_policy(tenant_id));");
        }
    }

    /// <summary>
    /// Disables RLS and removes all policies.
    /// Call this from your migration's Down() method.
    /// </summary>
    /// <param name="migrationBuilder">The migration builder instance</param>
    public static void DisableRls(MigrationBuilder migrationBuilder)
    {
        // Drop all policies
        foreach (var table in TablesWithGroupAndTenant.Concat(TablesWithTenantOnly))
        {
            migrationBuilder.Sql($"DROP POLICY IF EXISTS rls_policy ON {table};");
        }

        // Disable RLS on all tables
        foreach (var table in TablesWithGroupAndTenant.Concat(TablesWithTenantOnly))
        {
            migrationBuilder.Sql($"ALTER TABLE {table} DISABLE ROW LEVEL SECURITY;");
        }

        // Drop the policy functions
        DropPolicyFunctions(migrationBuilder);
    }

    /// <summary>
    /// Creates the PostgreSQL functions used by RLS policies.
    /// </summary>
    private static void CreatePolicyFunctions(MigrationBuilder migrationBuilder)
    {
        // Function for tables with both group_id and tenant_id
        migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION rls_group_tenant_policy(
                table_group_id uuid,
                table_tenant_id uuid
            ) RETURNS boolean AS $$
            BEGIN
                RETURN (
                    current_setting('app.group_id', true) IS NOT NULL
                    AND table_group_id = uuid(current_setting('app.group_id'))
                    AND (
                        current_setting('app.tenant_ids', true) IS NULL
                        OR table_tenant_id = ANY (string_to_array(current_setting('app.tenant_ids'), ',')::uuid[])
                    )
                );
            END;
            $$ LANGUAGE plpgsql SECURITY DEFINER;");

        // Function for tables with tenant_id only
        migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION rls_tenant_policy(
                table_tenant_id uuid
            ) RETURNS boolean AS $$
            BEGIN
                RETURN (
                    current_setting('app.tenant_id', true) IS NOT NULL
                    AND table_tenant_id = uuid(current_setting('app.tenant_id'))
                );
            END;
            $$ LANGUAGE plpgsql SECURITY DEFINER;");
    }

    /// <summary>
    /// Drops the PostgreSQL functions used by RLS policies.
    /// </summary>
    private static void DropPolicyFunctions(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP FUNCTION IF EXISTS rls_group_tenant_policy(uuid, uuid);");
        migrationBuilder.Sql("DROP FUNCTION IF EXISTS rls_tenant_policy(uuid);");
    }

    /// <summary>
    /// Adds a new table to the RLS policy management.
    /// Call this method to register a new table that should be included in RLS.
    /// </summary>
    /// <param name="tableName">The name of the table</param>
    /// <param name="hasGroupId">Whether the table has a group_id column</param>
    /// <param name="hasTenantId">Whether the table has a tenant_id column</param>
    public static void AddTable(string tableName, bool hasGroupId, bool hasTenantId)
    {
        // This method is for documentation purposes.
        // In practice, you would add the table to the appropriate array above
        // and then call EnableRls() again in a new migration.

        if (hasGroupId && hasTenantId)
        {
            // Add to TablesWithGroupAndTenant array
        }
        else if (hasTenantId)
        {
            // Add to TablesWithTenantOnly array
        }
        else
        {
            throw new ArgumentException("Table must have at least tenant_id column for RLS");
        }
    }
}
