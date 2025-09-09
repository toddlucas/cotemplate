using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

namespace Microsoft.AspNetCore.Mvc;

public static class ClaimsPrincipalExtensions
{
    //public static long? GetNameIdentifierOrDefault(this ClaimsPrincipal principal)
    //    => principal.FindFirstInt64Value(ClaimTypes.NameIdentifier);

    //public static long GetNameIdentifier(this ClaimsPrincipal principal)
    //    => principal.RequireFirstInt64Value(ClaimTypes.NameIdentifier);

    public static string? GetNameIdentifierOrDefault(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string GetNameIdentifier(this ClaimsPrincipal principal)
        => principal.RequireFirstValue(ClaimTypes.NameIdentifier);

    public static string? GetTenantIdOrDefault(this ClaimsPrincipal principal)
        => principal.FindFirstValue(CustomClaims.TenantId);

    public static string GetTenantId(this ClaimsPrincipal principal)
        => principal.RequireFirstValue(CustomClaims.TenantId);

    public static string RequireFirstValue(this ClaimsPrincipal principal, string claimType)
    {
        string? claimValue = principal.FindFirstValue(claimType);
        if (claimValue == null)
            throw new Exception("Claim value not found for {claimType}");

        return claimValue;
    }

    public static long RequireFirstInt64Value(this ClaimsPrincipal principal, string claimType)
    {
        string? claimValue = principal.RequireFirstValue(claimType);
        if (claimValue == null)
            throw new Exception("Claim value not found for {claimType}");

        if (!long.TryParse(claimValue, out long value))
            throw new Exception("Claim value is not Int64 for {claimType}");

        return value;
    }

    public static long? FindFirstInt64Value(this ClaimsPrincipal principal, string claimType)
    {
        string? claimValue = principal.FindFirstValue(claimType);
        if (claimValue == null)
            return null;

        if (!long.TryParse(claimValue, out long value))
            return null;

        return value;
    }
}
