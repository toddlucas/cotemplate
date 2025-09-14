using Corp.Business;
using Corp.Workflow;

namespace Corp.Access;

using Record = Organization;
using Model = OrganizationModel;
using DetailModel = OrganizationDetailModel;

/// <summary>
/// Organization mapper.
/// </summary>
[Mapper(UseDeepCloning = true, PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
public static partial class OrganizationMapper
{
    /// <summary>
    /// Maps the entity to the model.
    /// </summary>
    [MapperIgnoreSource(nameof(Record.GroupId))]
    [MapperIgnoreSource(nameof(Record.TenantId))]
    [MapperIgnoreSource(nameof(Record.Members))]
    [MapperIgnoreSource(nameof(Record.ChildOrganizations))]
    [MapperIgnoreSource(nameof(Record.ParentOrganization))]
    [MapperIgnoreSource(nameof(Record.CreatedAt))]
    [MapperIgnoreSource(nameof(Record.UpdatedAt))]
    [MapperIgnoreSource(nameof(Record.DeletedAt))]
    public static partial Model ToModel(this Record source);

    /// <summary>
    /// Maps the entity to the detail model.
    /// </summary>
    [MapperIgnoreSource(nameof(Record.GroupId))]
    [MapperIgnoreSource(nameof(Record.TenantId))]
    [MapperIgnoreSource(nameof(Record.Members))]
    [MapperIgnoreSource(nameof(Record.ChildOrganizations))]
    [MapperIgnoreSource(nameof(Record.ParentOrganization))]
    [MapperIgnoreSource(nameof(Record.CreatedAt))]
    [MapperIgnoreSource(nameof(Record.UpdatedAt))]
    [MapperIgnoreSource(nameof(Record.DeletedAt))]
    [MapperIgnoreTarget(nameof(DetailModel.Entities))]
    [MapperIgnoreTarget(nameof(DetailModel.Members))]
    [MapperIgnoreTarget(nameof(DetailModel.Tasks))]
    [MapperIgnoreTarget(nameof(DetailModel.ChecklistInstances))]
    [MapperIgnoreTarget(nameof(DetailModel.CreatedAt))]
    [MapperIgnoreTarget(nameof(DetailModel.UpdatedAt))]
    [MapperIgnoreTarget(nameof(DetailModel.DeletedAt))]
    public static partial DetailModel ToDetailModel(this Record source);

    /// <summary>
    /// Maps entities to models.
    /// </summary>
    [MapperIgnoreSource(nameof(Record.GroupId))]
    [MapperIgnoreSource(nameof(Record.TenantId))]
    [MapperIgnoreSource(nameof(Record.Members))]
    [MapperIgnoreSource(nameof(Record.ChildOrganizations))]
    [MapperIgnoreSource(nameof(Record.ParentOrganization))]
    [MapperIgnoreSource(nameof(Record.CreatedAt))]
    [MapperIgnoreSource(nameof(Record.UpdatedAt))]
    [MapperIgnoreSource(nameof(Record.DeletedAt))]
    public static partial Model[] ToModels(this IEnumerable<Record> source);

    /// <summary>
    /// Maps entities to detail models.
    /// </summary>
    [MapperIgnoreSource(nameof(Record.GroupId))]
    [MapperIgnoreSource(nameof(Record.TenantId))]
    [MapperIgnoreSource(nameof(Record.Members))]
    [MapperIgnoreSource(nameof(Record.ChildOrganizations))]
    [MapperIgnoreSource(nameof(Record.ParentOrganization))]
    [MapperIgnoreSource(nameof(Record.CreatedAt))]
    [MapperIgnoreSource(nameof(Record.UpdatedAt))]
    [MapperIgnoreSource(nameof(Record.DeletedAt))]
    [MapperIgnoreTarget(nameof(DetailModel.Entities))]
    [MapperIgnoreTarget(nameof(DetailModel.Members))]
    [MapperIgnoreTarget(nameof(DetailModel.Tasks))]
    [MapperIgnoreTarget(nameof(DetailModel.ChecklistInstances))]
    [MapperIgnoreTarget(nameof(DetailModel.CreatedAt))]
    [MapperIgnoreTarget(nameof(DetailModel.UpdatedAt))]
    [MapperIgnoreTarget(nameof(DetailModel.DeletedAt))]
    public static partial DetailModel[] ToDetailModels(this IEnumerable<Record> source);

    /// <summary>
    /// Maps the model to the entity.
    /// </summary>
    [MapperIgnoreTarget(nameof(Record.GroupId))]
    [MapperIgnoreTarget(nameof(Record.TenantId))]
    [MapperIgnoreTarget(nameof(Record.Members))]
    [MapperIgnoreTarget(nameof(Record.ChildOrganizations))]
    [MapperIgnoreTarget(nameof(Record.ParentOrganization))]
    [MapperIgnoreTarget(nameof(Record.CreatedAt))]
    [MapperIgnoreTarget(nameof(Record.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Record.DeletedAt))]
    public static partial Record ToRecord(this Model source);

    /// <summary>
    /// Copy allowable fields from the model to the entity for update.
    /// </summary>
    public static void UpdateFrom(this Record record, Model model)
    {
        // GroupId and TenantId are not set from model (internal-only)
        record.Name = model.Name;
        record.Code = model.Code;
        record.ParentOrgId = model.ParentOrgId;
        record.Status = model.Status;
        record.Metadata = model.Metadata;
    }

    // Note: Related entities (Entities, Tasks, ChecklistInstances) in OrganizationDetailModel
    // are populated separately through queries, not through navigation properties.
    // The Organization entity's Members, ChildOrganizations, and ParentOrganization
    // navigation properties are ignored in the mapping as they serve different purposes.
}
