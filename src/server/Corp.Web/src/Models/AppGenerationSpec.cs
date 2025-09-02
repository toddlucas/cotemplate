using TypeGen.Core.SpecGeneration;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;

using Corp.Http;

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
    }
}
