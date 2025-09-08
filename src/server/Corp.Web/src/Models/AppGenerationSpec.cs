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
        AddEnum<OrganizationMemberRole>();

        // Business enums
        AddEnum<EntityType>();
        AddEnum<EntityRelationshipType>();
        AddEnum<EntityRoleType>();
        AddEnum<EntityStatus>();
        AddEnum<OwnershipModel>();

        // Operations enums
        AddEnum<AiActionType>();
        AddEnum<AuditAction>();
        AddEnum<AiChannel>();
        AddEnum<FeedbackTargetType>();

        // Storage enums
        AddEnum<DocumentCategory>();

        // Workflow enums
        AddEnum<ReminderStatus>();
        AddEnum<SourceType>();
        AddEnum<ReminderChannel>();
        AddEnum<ChecklistStatus>();
        AddEnum<ChecklistScope>();
        AddEnum<Priority>();
        AddEnum<Workflow.TaskStatus>();
    }
}
