namespace Corp;

/// <summary>
/// Entity types for business entities.
/// </summary>
public enum EntityType
{
    Llc,
    CCorp,
    SCorp,
    Lp,
    Llp,
    Trust,
    SoleProp,
    Plc,
    NonProfit,
    Spv,
    Other
}

/// <summary>
/// Ownership models for entities.
/// </summary>
public enum OwnershipModel
{
    MemberManaged,
    ManagerManaged,
    BoardManaged,
    TrusteeManaged
}

/// <summary>
/// Entity status values.
/// </summary>
public enum EntityStatus
{
    Draft,
    Active,
    Dissolved,
    Merged
}

/// <summary>
/// Task status values.
/// </summary>
public enum TaskStatus
{
    Todo,
    InProgress,
    Blocked,
    Done,
    Skipped
}

/// <summary>
/// Priority levels.
/// </summary>
public enum Priority
{
    Low,
    Normal,
    High
}

/// <summary>
/// Document categories.
/// </summary>
public enum DocumentCategory
{
    Formation,
    Compliance,
    Tax,
    Contract,
    Id,
    Other
}

/// <summary>
/// Organization member roles.
/// </summary>
public enum OrgMemberRole
{
    Owner,
    Admin,
    Manager,
    Viewer,
    Advisor,
    External
}

/// <summary>
/// Entity role types.
/// </summary>
public enum EntityRoleType
{
    Owner,
    Member,
    Manager,
    Director,
    Officer,
    Trustee,
    Beneficiary,
    Advisor,
    Attorney,
    Accountant,
    RegisteredAgentContact,
    Signatory
}

/// <summary>
/// Entity relationship types.
/// </summary>
public enum EntityRelationshipType
{
    Owns,
    Controls,
    SubsidiaryOf,
    GpOf,
    LpOf,
    TrusteeOf,
    BeneficiaryOf,
    ManagerOf,
    AdvisorTo,
    SpvFor
}

/// <summary>
/// Checklist template scope.
/// </summary>
public enum ChecklistScope
{
    Org,
    Entity,
    Person
}

/// <summary>
/// Source types for templates and tasks.
/// </summary>
public enum SourceType
{
    System,
    Ai,
    Custom
}

/// <summary>
/// Checklist instance status.
/// </summary>
public enum ChecklistStatus
{
    Draft,
    Active,
    Archived
}

/// <summary>
/// AI session channels.
/// </summary>
public enum AiChannel
{
    Chat,
    InlineAction,
    Api
}

/// <summary>
/// AI action types.
/// </summary>
public enum AiActionType
{
    CreateEntity,
    CreateTask,
    Summarize,
    Extract,
    Classify,
    Other
}

/// <summary>
/// Reminder channels.
/// </summary>
public enum ReminderChannel
{
    Email,
    InApp,
    Webhook
}

/// <summary>
/// Reminder status.
/// </summary>
public enum ReminderStatus
{
    Scheduled,
    Sent,
    Failed
}

/// <summary>
/// Feedback target types.
/// </summary>
public enum FeedbackTargetType
{
    Task,
    Checklist,
    Document,
    Extraction,
    Summary
}

/// <summary>
/// Audit action types.
/// </summary>
public enum AuditAction
{
    Create,
    Read,
    Update,
    Delete,
    Login,
    Logout,
    AiAction,
    Other
}
