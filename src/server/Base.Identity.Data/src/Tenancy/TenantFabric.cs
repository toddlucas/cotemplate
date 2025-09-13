using Metalama.Framework.Fabrics;

namespace Base.Data.Identity;

#if false
/// <summary>
/// Metalama fabric that automatically applies tenant aspects based on method naming conventions.
/// This eliminates the need to manually add [TenantRead] and [TenantWrite] attributes to every method.
/// </summary>
public sealed class TenantFabric : ProjectFabric
{
    /// <summary>
    /// Configures the project to automatically apply tenant aspects based on method naming patterns.
    /// </summary>
    public override void AmendProject(IProjectAmender amender)
    {
        // Apply TenantRead aspect to methods that start with "Get", "List", "Find", "Search", "Query"
        amender.Outbound
            .SelectMany(compilation => compilation.Types)
            .SelectMany(type => type.Methods.Where(method =>
                method.Accessibility == Metalama.Framework.Code.Accessibility.Public &&
                !method.IsStatic &&
                (method.Name.StartsWith("Get", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("List", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Find", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Search", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Query", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Read", StringComparison.OrdinalIgnoreCase))))
            .AddAspect(_ => new TenantReadAttribute());

        // Apply TenantWrite aspect to methods that start with "Create", "Update", "Delete", "Save", "Post", "Put", "Patch"
        amender.Outbound
            .SelectMany(compilation => compilation.Types)
            .SelectMany(type => type.Methods.Where(method =>
                method.Accessibility == Metalama.Framework.Code.Accessibility.Public &&
                !method.IsStatic &&
                (method.Name.StartsWith("Create", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Update", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Delete", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Save", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Post", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Put", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Patch", StringComparison.OrdinalIgnoreCase) ||
                 method.Name.StartsWith("Remove", StringComparison.OrdinalIgnoreCase))))
            .AddAspect(_ => new TenantWriteAttribute());
    }
}
#endif
