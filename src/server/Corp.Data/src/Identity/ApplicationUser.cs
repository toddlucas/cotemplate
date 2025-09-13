namespace Microsoft.AspNetCore.Identity;

// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model
public class ApplicationUser : TenantIdentityUser<Guid> // IdentityUser
{
}
