using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Corp.Access;
using Corp.Business;
using Corp.Operations;
using Corp.Storage;
using Corp.Workflow;

namespace Corp.Data;

/// <summary>
/// The app database context.
/// </summary>
public class CorpDbContext : IdentityDbContext // <IdentityUser<long>, IdentityRole<long>, long>
{
    public CorpDbContext(DbContextOptions<CorpDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteAppDbContext" />
    /// class using the specified options.
    /// </summary>
    /// <remarks>
    /// Requires a non-generic DbContextOptions in order to be used with
    /// SqliteDatabaseSet. But this is at odds with design-time creation. So we
    /// must use our own design-time factory.
    /// https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation
    /// </remarks>
    private CorpDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public static CorpDbContext Create(DbContextOptions options)
    {
        return new CorpDbContext(options);
    }

    #region Identity

    // public DbSet<Profile> Profiles { get; set; } = null!;

    #endregion Identity

    protected bool IsUsingSqliteProvider => Database.ProviderName!.Contains("Sqlite", StringComparison.OrdinalIgnoreCase);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (IsUsingSqliteProvider)
            modelBuilder.AddSqliteDateTimeOffset();

        // ApplicationTenant.OnModelCreating(modelBuilder);
        IdentityModel.OnModelCreating(modelBuilder);

        // Identity
        //Profile.OnModelCreating(modelBuilder);

        modelBuilder.Snakeify();

        // Access enumerations
        EnumerationBuilder.OnStringCreating(modelBuilder, OrganizationMemberRoleEnum.GetAll(), OrganizationMemberRoleEnum.KeyLength, "organization_member_role");

        // Business enumerations
        EnumerationBuilder.OnStringCreating(modelBuilder, EntityTypeEnum.GetAll(), EntityTypeEnum.KeyLength, "entity_type");
        EnumerationBuilder.OnStringCreating(modelBuilder, EntityRelationshipTypeEnum.GetAll(), EntityRelationshipTypeEnum.KeyLength, "entity_relationship_type");
        EnumerationBuilder.OnStringCreating(modelBuilder, EntityRoleTypeEnum.GetAll(), EntityRoleTypeEnum.KeyLength, "entity_role_type");
        EnumerationBuilder.OnStringCreating(modelBuilder, EntityStatusEnum.GetAll(), EntityStatusEnum.KeyLength, "entity_status");
        EnumerationBuilder.OnStringCreating(modelBuilder, OwnershipModelEnum.GetAll(), OwnershipModelEnum.KeyLength, "ownership_model");

        // Operations enumerations
        EnumerationBuilder.OnStringCreating(modelBuilder, AiActionTypeEnum.GetAll(), AiActionTypeEnum.KeyLength, "ai_action_type");
        EnumerationBuilder.OnStringCreating(modelBuilder, AuditActionEnum.GetAll(), AuditActionEnum.KeyLength, "audit_action");
        EnumerationBuilder.OnStringCreating(modelBuilder, AiChannelEnum.GetAll(), AiChannelEnum.KeyLength, "ai_channel");
        EnumerationBuilder.OnStringCreating(modelBuilder, FeedbackTargetTypeEnum.GetAll(), FeedbackTargetTypeEnum.KeyLength, "feedback_target_type");

        // Storage enumerations
        EnumerationBuilder.OnStringCreating(modelBuilder, DocumentCategoryEnum.GetAll(), DocumentCategoryEnum.KeyLength, "document_category");

        // Workflow enumerations
        EnumerationBuilder.OnStringCreating(modelBuilder, ReminderStatusEnum.GetAll(), ReminderStatusEnum.KeyLength, "reminder_status");
        EnumerationBuilder.OnStringCreating(modelBuilder, SourceTypeEnum.GetAll(), SourceTypeEnum.KeyLength, "source_type");
        EnumerationBuilder.OnStringCreating(modelBuilder, ReminderChannelEnum.GetAll(), ReminderChannelEnum.KeyLength, "reminder_channel");
        EnumerationBuilder.OnStringCreating(modelBuilder, ChecklistStatusEnum.GetAll(), ChecklistStatusEnum.KeyLength, "checklist_status");
        EnumerationBuilder.OnStringCreating(modelBuilder, ChecklistScopeEnum.GetAll(), ChecklistScopeEnum.KeyLength, "checklist_scope");
        EnumerationBuilder.OnStringCreating(modelBuilder, PriorityEnum.GetAll(), PriorityEnum.KeyLength, "priority");
        EnumerationBuilder.OnStringCreating(modelBuilder, TaskStatusEnum.GetAll(), TaskStatusEnum.KeyLength, "task_status");
    }
}
