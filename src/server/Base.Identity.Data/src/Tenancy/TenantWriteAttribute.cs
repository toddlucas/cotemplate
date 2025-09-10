using Metalama.Framework.Aspects;

namespace Corp.Data.Identity;

/// <summary>
/// Metalama aspect that automatically ensures a write transaction is active before executing a method.
/// This aspect calls IRequestDbGuard.EnsureWriteAsync() at the beginning of the method.
///
/// Usage:
/// [TenantWrite]
/// public async Task<IActionResult> CreateUser() { ... }
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class TenantWriteAttribute : OverrideMethodAspect
{
    /// <summary>
    /// Overrides the method to ensure a write transaction is active before execution.
    /// </summary>
    public override dynamic? OverrideMethod()
    {
        // Get the ambient guard and ensure write transaction is active
        var guard = AmbientRequestGuard.Current;
        guard.EnsureWriteAsync().GetAwaiter().GetResult();

        // Execute the original method
        return meta.Proceed();
    }

    /// <summary>
    /// Overrides the method to ensure a write transaction is active before execution.
    /// </summary>
    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        // Get the ambient guard and ensure write transaction is active
        var guard = AmbientRequestGuard.Current;
        await guard.EnsureWriteAsync();

        // Execute the original method
        return await meta.ProceedAsync();
    }
}
