namespace Corp;

/// <summary>
/// Entity types for business entities.
/// </summary>
public enum EntityType
{
    llc,
    c_corp,
    s_corp,
    lp,
    llp,
    trust,
    sole_prop,
    plc,
    non_profit,
    spv,
    other
}

/// <summary>
/// Ownership models for entities.
/// </summary>
public enum OwnershipModel
{
    member_managed,
    manager_managed,
    board_managed,
    trustee_managed
}

/// <summary>
/// Entity status values.
/// </summary>
public enum EntityStatus
{
    draft,
    active,
    dissolved,
    merged
}

/// <summary>
/// Task status values.
/// </summary>
public enum TaskStatus
{
    todo,
    in_progress,
    blocked,
    done,
    skipped
}

/// <summary>
/// Priority levels.
/// </summary>
public enum Priority
{
    low,
    normal,
    high
}

/// <summary>
/// Document categories.
/// </summary>
public enum DocumentCategory
{
    formation,
    compliance,
    tax,
    contract,
    id,
    other
}

/// <summary>
/// Organization member roles.
/// </summary>
public enum OrgMemberRole
{
    owner,
    admin,
    manager,
    viewer,
    advisor,
    external
}

/// <summary>
/// Entity role types.
/// </summary>
public enum EntityRoleType
{
    owner,
    member,
    manager,
    director,
    officer,
    trustee,
    beneficiary,
    advisor,
    attorney,
    accountant,
    registered_agent_contact,
    signatory
}

/// <summary>
/// Entity relationship types.
/// </summary>
public enum EntityRelationshipType
{
    owns,
    controls,
    subsidiary_of,
    gp_of,
    lp_of,
    trustee_of,
    beneficiary_of,
    manager_of,
    advisor_to,
    spv_for
}

/// <summary>
/// Checklist template scope.
/// </summary>
public enum ChecklistScope
{
    org,
    entity,
    person
}

/// <summary>
/// Source types for templates and tasks.
/// </summary>
public enum SourceType
{
    system,
    ai,
    custom
}

/// <summary>
/// Checklist instance status.
/// </summary>
public enum ChecklistStatus
{
    draft,
    active,
    archived
}

/// <summary>
/// AI session channels.
/// </summary>
public enum AiChannel
{
    chat,
    inline_action,
    api
}

/// <summary>
/// AI action types.
/// </summary>
public enum AiActionType
{
    create_entity,
    create_task,
    summarize,
    extract,
    classify,
    other
}

/// <summary>
/// Reminder channels.
/// </summary>
public enum ReminderChannel
{
    email,
    inapp,
    webhook
}

/// <summary>
/// Reminder status.
/// </summary>
public enum ReminderStatus
{
    scheduled,
    sent,
    failed
}

/// <summary>
/// Feedback target types.
/// </summary>
public enum FeedbackTargetType
{
    task,
    checklist,
    document,
    extraction,
    summary
}

/// <summary>
/// Audit action types.
/// </summary>
public enum AuditAction
{
    create,
    read,
    update,
    delete,
    login,
    logout,
    ai_action,
    other
}
