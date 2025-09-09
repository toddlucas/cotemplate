using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Identity;

/// <summary>
/// Provides the APIs for user sign in.
/// </summary>
/// <typeparam name="TUser">The type encapsulating a user.</typeparam>
public class TenantSignInManager<TUser> : SignInManager<TUser>
    where TUser : TenantIdentityUser
{
    /// <summary>
    /// Creates a new instance of <see cref="SignInManager{TUser}"/>.
    /// </summary>
    /// <param name="userManager">An instance of <see cref="UserManager"/> used to retrieve users from and persist users.</param>
    /// <param name="contextAccessor">The accessor used to access the <see cref="HttpContext"/>.</param>
    /// <param name="claimsFactory">The factory to use to create claims principals for a user.</param>
    /// <param name="optionsAccessor">The accessor used to access the <see cref="IdentityOptions"/>.</param>
    /// <param name="logger">The logger used to log messages, warnings and errors.</param>
    /// <param name="schemes">The scheme provider that is used enumerate the authentication schemes.</param>
    /// <param name="confirmation">The <see cref="IUserConfirmation{TUser}"/> used check whether a user account is confirmed.</param>
    public TenantSignInManager(
        UserManager<TUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<TUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<TUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<TUser> confirmation)
        : base(
            userManager,
            contextAccessor,
            claimsFactory,
            optionsAccessor,
            logger,
            schemes,
            confirmation)
    {
    }

#if false
    /// <summary>
    /// Signs in the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to sign-in.</param>
    /// <param name="authenticationProperties">Properties applied to the login and authentication cookie.</param>
    /// <param name="additionalClaims">Additional claims that will be stored in the cookie.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public override Task SignInWithClaimsAsync(TUser user, AuthenticationProperties? authenticationProperties, IEnumerable<Claim> additionalClaims)
    {
        // NOTE: This mechanism only addresses sign in. When user credentials
        // are refreshed, the claim is lost. One option is to add a handler
        // for OnRefreshingPrincipal. Instead, we hook into a lower-level and
        // add the claim direclty to the principal after creation.
        // https://github.com/dotnet/aspnetcore/issues/49610
        // additionalClaims = additionalClaims.Append(new Claim(CustomClaims.TenantId, user.TenantId.ToString()));
        return base.SignInWithClaimsAsync(user, authenticationProperties, additionalClaims);
    }
#endif

    /// <summary>
    /// Creates a <see cref="ClaimsPrincipal"/> for the specified <paramref name="user"/>, as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to create a <see cref="ClaimsPrincipal"/> for.</param>
    /// <returns>The task object representing the asynchronous operation, containing the ClaimsPrincipal for the specified user.</returns>
    public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(TUser user)
    {
        var userPrincipal = await base.CreateUserPrincipalAsync(user);
        var tenantClaim = new Claim(CustomClaims.TenantId, user.TenantId.ToString());
        userPrincipal.Identities.First().AddClaim(tenantClaim);
        return userPrincipal;
    }
}
