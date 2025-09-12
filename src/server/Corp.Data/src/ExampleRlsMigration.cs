using Microsoft.EntityFrameworkCore.Migrations;

namespace Corp.Data.Npgsql.Migrations.Corp;

/// <summary>
/// Example migration showing how to use RlsPolicyManager.
/// Copy this pattern into your actual migration files.
/// </summary>
public partial class ExampleRlsMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Simply call the RlsPolicyManager to enable RLS for all tables
        RlsPolicyManager.EnableRls(migrationBuilder);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Simply call the RlsPolicyManager to disable RLS for all tables
        RlsPolicyManager.DisableRls(migrationBuilder);
    }
}
