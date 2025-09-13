using System.Threading.Tasks;
using Metalama.Framework.Aspects;

namespace Base.Data.Identity;

/// <summary>
/// Metalama aspect that automatically ensures a read transaction is active before executing a method.
/// This aspect calls IRequestDbGuard.EnsureReadAsync() at the beginning of the method.
///
/// Usage:
/// [TenantRead]
/// public async Task<IActionResult> GetUsers() { ... }
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class TenantReadAttribute : OverrideMethodAspect
{
    /// <summary>
    /// Overrides the method to ensure a read transaction is active before execution.
    /// </summary>
    public override dynamic? OverrideMethod()
    {
        // Get the ambient guard and ensure read transaction is active
        var guard = AmbientRequestGuard.Current;
        guard.EnsureReadAsync().GetAwaiter().GetResult();

        // Execute the original method
        return meta.Proceed();
    }

    /// <summary>
    /// Overrides the method to ensure a read transaction is active before execution.
    /// </summary>
    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        // Get the ambient guard and ensure read transaction is active
        var guard = AmbientRequestGuard.Current;
        await guard.EnsureReadAsync();

        // Execute the original method
        return await meta.ProceedAsync();
    }
}
