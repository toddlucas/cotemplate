using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;

using Corp.Http;
using Corp.Pagination;
using Corp.Access;
using Corp.Business;
using Corp.Operations;
using Corp.Storage;
using Corp.Workflow;

namespace Corp.Models;

public class AppGenerationSpec : BaseGenerationSpec
{
    public AppGenerationSpec()
    {
        // /api/auth
        const string authPath = "auth";

        AddInterface<RegisterRequest>(authPath);
        AddInterface<LoginRequest>(authPath);
        AddInterface<RefreshRequest>(authPath);
        AddInterface<AccessTokenResponse>(authPath);
        AddInterface<ResendConfirmationEmailRequest>(authPath);
        AddInterface<ForgotPasswordRequest>(authPath);
        AddInterface<ResetPasswordRequest>(authPath);
        AddInterface<TwoFactorRequest>(authPath);
        AddInterface<TwoFactorResponse>(authPath);
        AddInterface<InfoRequest>(authPath);
        AddInterface<InfoResponse>(authPath);

        // Identity
        AddInterface<IdentityUserModel>();
        AddInterface(typeof(IdentityUserModel<>));

        AddInterface<ProblemDetailsModel>();
        AddInterface<ValidationProblemDetailsModel>();

        AddInterface<PagedQuery>();
        AddInterface(typeof(PagedResult<>));

        // Access enums
        const string accessPath = "access";
        AddEnum<OrganizationMemberRole>(accessPath);
        AddInterface<OrganizationMemberModel>(accessPath)
            .Member(x => nameof(x.RoleId)).Type(nameof(OrganizationMemberRole), "./organization-member-role");

        // Business enums
        const string businessPath = "business";
        AddEnum<EntityType>(businessPath);
        AddEnum<EntityRelationshipType>(businessPath);
        AddEnum<EntityRoleType>(businessPath);
        AddEnum<EntityStatus>(businessPath);
        AddEnum<OwnershipModel>(businessPath);
        AddInterface<EntityModel>(businessPath)
            .Member(x => nameof(x.EntityTypeId)).Type(nameof(EntityType), "./entity-type")
            .Member(x => nameof(x.OwnershipModelId)).Type(nameof(OwnershipModel), "./ownership-model")
            .Member(x => nameof(x.StatusId)).Type(nameof(EntityStatus), "./entity-status");
        AddInterface<EntityRoleModel>(businessPath)
            .Member(x => nameof(x.RoleId)).Type(nameof(EntityRoleType), "./entity-role-type");
        AddInterface<EntityRelationshipModel>(businessPath)
            .Member(x => nameof(x.RelationshipTypeId)).Type(nameof(EntityRelationshipType), "./entity-relationship-type");

        // Operations enums
        const string operationsPath = "operations";
        AddEnum<AiActionType>(operationsPath);
        AddEnum<AuditAction>(operationsPath);
        AddEnum<AiChannel>(operationsPath);
        AddEnum<FeedbackTargetType>(operationsPath);

        // Storage enums
        const string storagePath = "storage";
        AddEnum<DocumentCategory>(storagePath);
        AddInterface<DocumentModel>(storagePath)
            .Member(x => nameof(x.CategoryId)).Type(nameof(DocumentCategory), "./document-category");

        // Workflow enums
        const string workflowPath = "workflow";
        AddEnum<ReminderStatus>(workflowPath);
        AddEnum<SourceType>(workflowPath);
        AddEnum<ReminderChannel>(workflowPath);
        AddEnum<ChecklistStatus>(workflowPath);
        AddEnum<ChecklistScope>(workflowPath);
        AddEnum<Priority>(workflowPath);
        AddEnum<Workflow.TaskStatus>(workflowPath);
        AddInterface<TaskModel>(workflowPath)
            .Member(x => nameof(x.StatusId)).Type(nameof(Workflow.TaskStatus), "./task-status")
            .Member(x => nameof(x.PriorityId)).Type(nameof(Priority), "./priority");
        AddInterface<TaskTemplateModel>(workflowPath)
            .Member(x => nameof(x.PriorityId)).Type(nameof(Priority), "./priority");
        AddInterface<ChecklistTemplateModel>(workflowPath)
            .Member(x => nameof(x.ScopeId)).Type(nameof(ChecklistScope), "./checklist-scope")
            .Member(x => nameof(x.SourceTypeId)).Type(nameof(SourceType), "./source-type");
        AddInterface<ChecklistModel>(workflowPath)
            .Member(x => nameof(x.StatusId)).Type(nameof(ChecklistStatus), "./checklist-status")
            .Member(x => nameof(x.CreatedFromId)).Type(nameof(SourceType), "./source-type");
    }
}
