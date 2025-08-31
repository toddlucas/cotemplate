using TypeGen.Core.SpecGeneration;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;

using Corp.Http;

namespace Corp.Models;

public class AppGenerationSpec : GenerationSpec
{
    public AppGenerationSpec()
    {
        // /api/auth
        AddInterface<RegisterRequest>();
        AddInterface<LoginRequest>();
        AddInterface<RefreshRequest>();
        AddInterface<AccessTokenResponse>();
        AddInterface<ResendConfirmationEmailRequest>();
        AddInterface<ForgotPasswordRequest>();
        AddInterface<ResetPasswordRequest>();
        AddInterface<TwoFactorRequest>();
        AddInterface<TwoFactorResponse>();
        AddInterface<InfoRequest>();
        AddInterface<InfoResponse>();

        // Identity
        AddInterface<IdentityUserModel>();
        AddInterface(typeof(IdentityUserModel<>));

        AddInterface<ProblemDetailsModel>();
        AddInterface<ValidationProblemDetailsModel>();
    }
}
