# Server Component Implementation Template

## Overview

This document provides a step-by-step template for implementing new server components following the established patterns. Use this as a checklist when creating new entities and their associated components.

## Implementation Checklist

### Phase 1: Planning and Design

- [ ] **Define Entity Requirements**
  - [ ] Identify entity name (e.g., `Product`, `Category`, `Order`)
  - [ ] Define properties and data types
  - [ ] Identify relationships with other entities
  - [ ] Determine if temporal tracking is needed (`ITemporal`)
  - [ ] Plan any special business rules or validation

- [ ] **Plan API Endpoints**
  - [ ] Determine which CRUD operations are needed
  - [ ] Identify any custom endpoints beyond standard CRUD
  - [ ] Plan authorization requirements
  - [ ] Consider pagination needs for list operations

### Phase 2: Contracts Layer

#### Step 1: Create Basic Model
**File**: `src/server/Corp.Contracts/src/{Namespace}/{EntityName}Model.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Corp.{Namespace};

public class {EntityName}Model
{
    /// <summary>
    /// The {entity} ID.
    /// </summary>
    [Display(Name = "ID")]
    public long Id { get; set; }

    /// <summary>
    /// The {entity} {property}.
    /// </summary>
    [Display(Name = "{Property}")]
    public string {Property} { get; set; } = null!;

    // Add other properties as needed
}
```

- [ ] Create `{EntityName}Model.cs`
- [ ] Add all basic properties with proper data annotations
- [ ] Include XML documentation for all properties
- [ ] Add `[Display]` attributes for UI-friendly names

#### Step 2: Create Detail Model (if needed)
**File**: Same as above, add to existing file

```csharp
public class {EntityName}DetailModel : {EntityName}Model, ITemporal
{
    // Add related entities as arrays - use base models, NOT detail models
    // This prevents endless nesting and circular references
    public {RelatedEntity}Model[] {RelatedEntities} { get; set; } = [];

    #region ITemporal

    /// <summary>
    /// The created timestamp.
    /// </summary>
    [Display(Name = "Created at")]
    [Description("The date and time this record was created, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The updated timestamp.
    /// </summary>
    [Display(Name = "Updated at")]
    [Description("The date and time this record was last updated, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// The deleted timestamp.
    /// </summary>
    [Display(Name = "Deleted at")]
    [Description("The date and time this record was deleted, or null, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset? DeletedAt { get; set; }

    #endregion ITemporal
}
```

- [ ] Create `{EntityName}DetailModel` if relationships exist
- [ ] Inherit from base model and `ITemporal`
- [ ] **Add related entity collections using base models (e.g., `{RelatedEntity}Model[]`), NOT detail models**
- [ ] **This prevents endless nesting and circular references in API responses**
- [ ] Include temporal properties with proper documentation

### Phase 3: Data Layer

#### Step 3: Create Entity
**File**: `src/server/Corp.Data/src/{Namespace}/{EntityName}.cs`

```csharp
namespace Corp.{Namespace};

using TRecord = {EntityName};

public class {EntityName} : {EntityName}Model, ITemporal
{
    #region Internal properties

    // Add any internal-only properties here (not exposed via API)
    // Example: public string? InternalId { get; set; }

    // For user-scoped entities, include UserId as internal-only field
    // /// <summary>
    // /// The user ID who owns this {entity}.
    // /// </summary>
    // [Display(Name = "User ID")]
    // [Required]
    // public string UserId { get; set; } = null!;

    #endregion Internal properties

    #region Navigation properties

    // Add navigation properties for relationships
    public {RelatedEntity}[] {RelatedEntities} { get; set; } = [];

    #endregion Navigation properties

    #region ITemporal

    /// <summary>
    /// The created timestamp.
    /// </summary>
    [Display(Name = "Created at")]
    [Description("The date and time this record was created, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The updated timestamp.
    /// </summary>
    [Display(Name = "Updated at")]
    [Description("The date and time this record was last updated, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// The deleted timestamp.
    /// </summary>
    [Display(Name = "Deleted at")]
    [Description("The date and time this record was deleted, or null, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTimeOffset? DeletedAt { get; set; }

    #endregion ITemporal

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table
        modelBuilder.Entity<TRecord>().ToTable(nameof({EntityName}));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<TRecord>().Property(x => x.{Property}).HasColumnName("{property_name}");
        // For user-scoped entities, add UserId mapping
        // modelBuilder.Entity<TRecord>().Property(x => x.UserId).HasColumnName("user_id");
        // Add other column mappings

        // Relations
        // Example: One-to-many relationship
        // modelBuilder.Entity<TRecord>()
        //     .HasOne(x => x.{RelatedEntity})
        //     .WithMany(y => y.{EntityName}s)
        //     .HasForeignKey(x => x.{RelatedEntity}Id)
        //     .IsRequired();

        // Indexes
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.{Property});
        // For user-scoped entities, add UserId index
        // modelBuilder.Entity<TRecord>()
        //     .HasIndex(b => b.UserId);

        // Seed data (optional)
        var createdAt = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<TRecord>().HasData(new TRecord 
        { 
            Id = 1, 
            {Property} = "Sample {Entity}", 
            // UserId = "sample-user-id", // For user-scoped entities
            CreatedAt = createdAt, 
            UpdatedAt = createdAt 
        });
    }
}
```

- [ ] Create entity class inheriting from model and `ITemporal`
- [ ] Add internal properties section
- [ ] **For user-scoped entities**: Add `UserId` as internal-only field with `[Required]` and `[Display]` attributes
- [ ] Add navigation properties for relationships
- [ ] Implement `OnModelCreating` with table configuration
- [ ] Map all properties to snake_case column names
- [ ] **For user-scoped entities**: Map `UserId` to `user_id` column and add index
- [ ] Configure relationships and foreign keys
- [ ] Add appropriate indexes
- [ ] Add seed data if needed

#### Step 4: Create Query Class
**File**: `src/server/Corp.Data/src/{Namespace}/{EntityName}Query.cs`

**For user-scoped entities**: Include userId as the first parameter to queries that require it

```csharp
namespace Corp.{Namespace};

using Record = {EntityName};

public record {EntityName}Query(DbSet<Record> DbSet, ILogger logger)
{
    // For user-scoped entities, use userId parameter in all methods
    public Task<Record?> SingleByUserIdOrDefaultAsync(string userId, long id, CancellationToken cancellationToken = default) => DbSet
        .Where(e => e.Id == id && e.UserId == userId)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record?> TrackByUserIdOrDefaultAsync(string userId, long id, CancellationToken cancellationToken = default) => DbSet
        .AsTracking()
        .Where(e => e.Id == id && e.UserId == userId)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record?> SingleDetailByUserIdOrDefaultAsync(string userId, long id, CancellationToken cancellationToken = default) => DbSet
        .Include(e => e.{RelatedEntities})
        .Where(e => e.Id == id && e.UserId == userId)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record?> TrackDetailByUserIdOrDefaultAsync(string userId, long id, CancellationToken cancellationToken = default) => DbSet
        .Include(e => e.{RelatedEntities})
        .AsTracking()
        .Where(e => e.Id == id && e.UserId == userId)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record[]> ListByUserIdAsync(string userId, CancellationToken cancellationToken = default) => DbSet
        .Where(e => e.UserId == userId)
        .ToArrayAsync(cancellationToken);

    // Add custom query methods as needed
    // public Task<Record[]> FindBy{Property}ByUserIdAsync(string userId, string {property}, CancellationToken cancellationToken = default) => DbSet
    //     .Where(e => e.{Property}.Contains({property}) && e.UserId == userId)
    //     .ToArrayAsync(cancellationToken);
}
```

**For non-user-scoped entities**: Write queries as normal

```csharp
namespace Corp.{Namespace};

using Record = {EntityName};

public record {EntityName}Query(DbSet<Record> DbSet, ILogger logger)
{
    public Task<Record?> SingleOrDefaultAsync(long id, CancellationToken cancellationToken = default) => DbSet
        .Where(e => e.Id == id)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record?> TrackOrDefaultAsync(long id, CancellationToken cancellationToken = default) => DbSet
        .AsTracking()
        .Where(e => e.Id == id)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record?> SingleDetailOrDefaultAsync(long id, CancellationToken cancellationToken = default) => DbSet
        .Include(e => e.{RelatedEntities})
        .Where(e => e.Id == id)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record?> TrackDetailOrDefaultAsync(long id, CancellationToken cancellationToken = default) => DbSet
        .Include(e => e.{RelatedEntities})
        .AsTracking()
        .Where(e => e.Id == id)
        .SingleOrDefaultAsync(cancellationToken);

    public Task<Record[]> ListAsync(CancellationToken cancellationToken = default) => DbSet
        .ToArrayAsync(cancellationToken);

    // Add custom query methods as needed
    // public Task<Record[]> FindBy{Property}Async(string {property}, CancellationToken cancellationToken = default) => DbSet
    //     .Where(e => e.{Property}.Contains({property}))
    //     .ToArrayAsync(cancellationToken);
}
```

- [ ] Create query record with DbSet and ILogger parameters
- [ ] **For user-scoped entities**: Use `userId` as first parameter in all methods and filter by `e.UserId == userId`
- [ ] Implement standard query methods
- [ ] Add `Include()` statements for detail queries
- [ ] Add custom query methods as needed
- [ ] Ensure proper cancellation token support

#### Step 5: Create Mapper Class
**File**: `src/server/Corp.Data/src/{Namespace}/{EntityName}Mapper.cs`

```csharp
namespace Corp.{Namespace};

using Record = {EntityName};
using Model = {EntityName}Model;
using DetailModel = {EntityName}DetailModel;

/// <summary>
/// {EntityName} mapper.
/// </summary>
[Mapper(UseDeepCloning = true, PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
public static partial class {EntityName}Mapper
{
    /// <summary>
    /// Maps the entity to the model.
    /// </summary>
    // [MapperIgnoreSource(nameof(Record.InternalId))] // Uncomment if using internal properties
    [MapperIgnoreSource(nameof(Record.UserId))] // For user-scoped entities
    [MapperIgnoreSource(nameof(Record.{RelatedEntities}))]
    [MapperIgnoreSource(nameof(Record.CreatedAt))]
    [MapperIgnoreSource(nameof(Record.UpdatedAt))]
    [MapperIgnoreSource(nameof(Record.DeletedAt))]
    public static partial Model ToModel(this Record source);

    /// <summary>
    /// Maps the entity to the detail model.
    /// </summary>
    // [MapperIgnoreSource(nameof(Record.InternalId))] // Uncomment if using internal properties
    [MapperIgnoreSource(nameof(Record.UserId))] // For user-scoped entities
    public static partial DetailModel ToDetailModel(this Record source);

    /// <summary>
    /// Maps entities to models.
    /// </summary>
    public static partial Model[] ToModels(this IEnumerable<Record> source);

    /// <summary>
    /// Maps entities to detail models.
    /// </summary>
    public static partial DetailModel[] ToDetailModels(this IEnumerable<Record> source);

    /// <summary>
    /// Maps the model to the entity.
    /// </summary>
    // [MapperIgnoreTarget(nameof(Record.InternalId))] // Uncomment if using internal properties
    [MapperIgnoreTarget(nameof(Record.UserId))] // For user-scoped entities
    [MapperIgnoreTarget(nameof(Record.{RelatedEntities}))]
    [MapperIgnoreTarget(nameof(Record.CreatedAt))]
    [MapperIgnoreTarget(nameof(Record.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Record.DeletedAt))]
    public static partial Record ToRecord(this Model source);

    /// <summary>
    /// Copy allowable fields from the model to the entity for update.
    /// </summary>
    public static void UpdateFrom(this Record record, Model model)
    {
        // record.UserId is not set from model (internal-only for user-scoped entities)
        record.{Property} = model.{Property};
        // Add other updateable properties
    }

    // Private mappers for related entities
    [MapperIgnoreSource(nameof({RelatedEntity}.{NavigationProperty}))]
    [MapperIgnoreSource(nameof({RelatedEntity}.CreatedAt))]
    [MapperIgnoreSource(nameof({RelatedEntity}.UpdatedAt))]
    [MapperIgnoreSource(nameof({RelatedEntity}.DeletedAt))]
    private static partial {RelatedEntity}Model MapTo{RelatedEntity}Model(this {RelatedEntity} source);
}
```

- [ ] Create mapper class with Mapperly attributes
- [ ] **For user-scoped entities**: Add `[MapperIgnoreSource(nameof(Record.UserId))]` and `[MapperIgnoreTarget(nameof(Record.UserId))]` attributes
- [ ] Implement all standard mapping methods
- [ ] Add appropriate `MapperIgnoreSource` and `MapperIgnoreTarget` attributes
- [ ] Implement `UpdateFrom` method with updateable properties only
- [ ] Add private mappers for related entities
- [ ] Include proper XML documentation

#### Step 6: Update DbContext
**File**: `src/server/Corp.Data/src/CorpDbContext.cs`

```csharp
// Add to the appropriate region
public DbSet<{EntityName}> {EntityName}s { get; set; } = null!;

// Add to OnModelCreating method
{EntityName}.OnModelCreating(modelBuilder);
```

- [ ] Add DbSet property for the new entity
- [ ] Add OnModelCreating call in the appropriate section
- [ ] Ensure proper naming (plural for DbSet)

### Phase 4: Services Layer

#### Step 7: Create Service Class
**File**: `src/server/Corp.Services/src/{Namespace}/{EntityName}Service.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Corp.Data;

namespace Corp.{Namespace};

using Record = {EntityName};
using Model = {EntityName}Model;
using DetailModel = {EntityName}DetailModel;

public class {EntityName}Service(CorpDbContext dbContext, ILogger<{EntityName}Service> logger)
{
    private readonly ILogger _logger = logger;
    private readonly CorpDbContext _dbContext = dbContext;
    private readonly DbSet<Record> _dbSet = dbContext.{EntityName}s;
    private readonly {EntityName}Query _query = new(dbContext.{EntityName}s, logger);

    public async Task<Model?> ReadOrDefaultAsync(string userId, long id, CancellationToken cancellationToken)
    {
        Record? record = await _query.SingleByUserIdOrDefaultAsync(userId, id, cancellationToken);
        return record?.ToModel();
    }

    public async Task<DetailModel?> ReadDetailOrDefaultAsync(string userId, long id, CancellationToken cancellationToken)
    {
        Record? record = await _query.SingleDetailByUserIdOrDefaultAsync(userId, id, cancellationToken);
        return record?.ToDetailModel();
    }

    public async Task<Model[]> ListAsync(string userId, CancellationToken cancellationToken)
    {
        Record[] records = await _query.ListByUserIdAsync(userId, cancellationToken);
        return records.ToModels();
    }

    public async Task<Model> CreateAsync(string userId, Model model)
    {
        Record record = model.ToRecord();
        record.UserId = userId; // For user-scoped entities
        record.CreatedAt = record.UpdatedAt = DateTimeOffset.UtcNow;
        _dbSet.Add(record);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Created {entityName} {EntityId}.", record.Id);
        return record.ToModel();
    }

    public async Task<Model?> UpdateAsync(string userId, Model model)
    {
        Record? record = await _query.TrackByUserIdOrDefaultAsync(userId, model.Id);
        if (record is null)
            return null;

        record.UpdateFrom(model);
        record.UpdatedAt = DateTimeOffset.UtcNow;

        _dbSet.Update(record);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Updated {entityName} {EntityId}.", record.Id);
        return record.ToModel();
    }

    public async Task<bool> DeleteAsync(string userId, long id)
    {
        Record? record = await _query.TrackByUserIdOrDefaultAsync(userId, id);
        if (record == null)
            return false;

        _dbSet.Remove(record);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Deleted {entityName} {EntityId}.", id);
        return true;
    }

    // Add custom business logic methods as needed
    // public async Task<Model[]> FindBy{Property}Async(string userId, string {property}, CancellationToken cancellationToken)
    // {
    //     Record[] records = await _query.FindBy{Property}ByUserIdAsync(userId, {property}, cancellationToken);
    //     return records.ToModels();
    // }
}
```

- [ ] Create service class with proper constructor injection
- [ ] Initialize all required fields (logger, dbContext, dbSet, query)
- [ ] **For user-scoped entities**: Use `userId` as first parameter in all methods
- [ ] **For user-scoped entities**: Set `record.UserId = userId` in CreateAsync
- [ ] Implement all standard CRUD operations
- [ ] Add proper timestamp management in Create and Update
- [ ] Add appropriate logging statements
- [ ] Add custom business logic methods as needed
- [ ] Ensure proper error handling and null checks

#### Step 8: Register Service
**File**: `src/server/Corp.Services/src/Extensions/IServiceCollectionExtensions.cs`

```csharp
// Add to AddServices method
serviceCollection.AddScoped<{EntityName}Service>();
```

- [ ] Add service registration to DI container
- [ ] Use `AddScoped` for entity services

#### Step 8a: Add TypeScript Generation (Optional)
**File**: `src/server/Corp.Web/src/Models/AppGenerationSpec.cs`

```csharp
// Add to constructor
// {EntityName}
AddInterface<{EntityName}Model>();
AddInterface<{EntityName}DetailModel>();
```

- [ ] Add model interfaces to TypeScript generation
- [ ] This generates TypeScript types for client-side use
- [ ] Types are generated to `src/client/lib/src/models/`

### Phase 5: Web Layer

#### Step 9: Create Controller
**File**: `src/server/Corp.Web/src/Controllers/{Namespace}/{EntityName}Controller.cs`

```csharp
using Microsoft.AspNetCore.Authorization;

using Corp.{Namespace};

namespace Corp.Controllers.{Namespace};

[Route("api/[area]/[controller]")]
[Area(nameof({Namespace}))]
[Tags(nameof({Namespace}))]
[Authorize(Policy = AppPolicy.RequireUserRole)]
[ApiController]
public class {EntityName}Controller(
    ILogger<{EntityName}Controller> logger,
    {EntityName}Service {entityName}Service) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly {EntityName}Service _{entityName}Service = {entityName}Service;

    /// <summary>
    /// Returns a {entity}.
    /// </summary>
    /// <param name="id">The ID of the {entity}.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A {entity} details object.</returns>
    [HttpGet("{id:long}")]
    [Produces(typeof({EntityName}DetailModel))]
    [EndpointDescription("Returns a {entity} details object.")]
    public async Task<ActionResult> Get(long id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        string userId = User.GetNameIdentifier(); // For user-scoped entities
        {EntityName}DetailModel? result = await _{entityName}Service.ReadDetailOrDefaultAsync(userId, id, cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Returns a list of {entities}.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of {entities}.</returns>
    [HttpGet]
    [Produces("application/json", Type = typeof({EntityName}Model[]))]
    [EndpointDescription("Returns a list of {entities}.")]
    public async Task<ActionResult> List(CancellationToken cancellationToken)
    {
        string userId = User.GetNameIdentifier(); // For user-scoped entities
        {EntityName}Model[] results = await _{entityName}Service.ListAsync(userId, cancellationToken);
        return Ok(results);
    }

    /// <summary>
    /// Creates a {entity}.
    /// </summary>
    /// <param name="model">The {entity} object.</param>
    /// <returns>The {entity} object.</returns>
    [HttpPost]
    [Produces(typeof({EntityName}Model))]
    public async Task<ActionResult> Post({EntityName}Model model)
    {
        if (model is null)
            return BadRequest();

        string userId = User.GetNameIdentifier(); // For user-scoped entities
        {EntityName}Model result = await _{entityName}Service.CreateAsync(userId, model);
        return Ok(result);
    }

    /// <summary>
    /// Updates a {entity}.
    /// </summary>
    /// <param name="model">The {entity} object.</param>
    /// <returns>The {entity} object.</returns>
    [HttpPut]
    [Produces(typeof({EntityName}Model))]
    public async Task<ActionResult> Put({EntityName}Model model)
    {
        if (model is null)
            return BadRequest();

        string userId = User.GetNameIdentifier(); // For user-scoped entities
        {EntityName}Model? result = await _{entityName}Service.UpdateAsync(userId, model);
        if (result is null)
            return BadRequest();

        return Ok(result);
    }

    /// <summary>
    /// Deletes a {entity}.
    /// </summary>
    /// <param name="id">The ID of the {entity}.</param>
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> Delete(long id)
    {
        if (id <= 0)
            return BadRequest();

        string userId = User.GetNameIdentifier(); // For user-scoped entities
        bool succeeded = await _{entityName}Service.DeleteAsync(userId, id);
        if (!succeeded)
            return NotFound();

        return NoContent();
    }

    // Add custom endpoints as needed
    // /// <summary>
    // /// Finds {entities} by {property}.
    // /// </summary>
    // /// <param name="{property}">The {property} to search for.</param>
    // /// <param name="cancellationToken">A cancellation token.</param>
    // /// <returns>A list of matching {entities}.</returns>
    // [HttpGet("search")]
    // [Produces("application/json", Type = typeof({EntityName}Model[]))]
    // public async Task<ActionResult> Search(string {property}, CancellationToken cancellationToken)
    // {
    //     if (string.IsNullOrWhiteSpace({property}))
    //         return BadRequest();
    //
    //     string userId = User.GetNameIdentifier(); // For user-scoped entities
    //     {EntityName}Model[] results = await _{entityName}Service.FindBy{Property}Async(userId, {property}, cancellationToken);
    //     return Ok(results);
    // }
}
```

- [ ] Create controller with proper attributes and routing
- [ ] Set appropriate area and authorization policy
- [ ] **For user-scoped entities**: Read `userId` from current user using `User.GetNameIdentifier()` in all endpoints
- [ ] **For user-scoped entities**: Pass `userId` as first parameter to all service methods
- [ ] Implement all standard CRUD endpoints
- [ ] Add proper input validation
- [ ] Include comprehensive XML documentation
- [ ] Add `[Produces]` attributes for OpenAPI documentation
- [ ] Add `[EndpointDescription]` for better API docs
- [ ] Add custom endpoints as needed
- [ ] Ensure consistent error handling and response patterns

### Phase 6: Database Migration

#### Step 10: Create Migration
**Commands to run**:

```bash
# Navigate to the appropriate data project
cd src/server/Corp.Data.Npgsql  # or Corp.Data.Sqlite

# Create migration
dotnet ef migrations add Add{EntityName}Entity

# Update database
dotnet ef database update
```

- [ ] Create EF Core migration for the new entity
- [ ] Review generated migration for correctness
- [ ] Apply migration to development database
- [ ] Verify database schema matches expectations

### Phase 7: Testing (Optional but Recommended)

#### Step 11: Create Service Tests
**File**: `tests/Corp.Services.Tests/{Namespace}/{EntityName}ServiceTests.cs`

```csharp
// TODO: Add comprehensive service tests
// - Test all CRUD operations
// - Test error scenarios
// - Mock dependencies appropriately
```

#### Step 12: Create Controller Tests
**File**: `tests/Corp.Web.Tests/Controllers/{Namespace}/{EntityName}ControllerTests.cs`

```csharp
// TODO: Add comprehensive controller tests
// - Test all endpoints
// - Test authorization
// - Test input validation
// - Mock service dependencies
```

- [ ] Create service unit tests
- [ ] Create controller integration tests
- [ ] Test error scenarios and edge cases
- [ ] Verify authorization requirements

## TypeScript Generation with TypeGen

The project uses **TypeGen** to automatically generate TypeScript interfaces from C# models, ensuring type safety between server and client.

### How It Works
1. **C# Models** → **TypeScript Interfaces** via `AppGenerationSpec.cs`
2. Generated files go to `src/client/lib/src/models/`
3. Client projects import these types for API calls

### Adding New Types
```csharp
// In AppGenerationSpec.cs constructor
AddInterface<YourModel>();
AddInterface<YourDetailModel>();
```

### Generated Output Example
```typescript
// Generated from ProjectModel
export interface ProjectModel {
    id: number;
    name: string;
    description?: string;
    userId: string;
    autoSync: boolean;
    versionControl: boolean;
    accessControl: boolean;
    defaultStorageGroup?: string;
}
```

### Benefits
- **Type Safety**: Compile-time checking of API contracts
- **Auto-Sync**: Changes to C# models automatically update TypeScript
- **Consistency**: Single source of truth for data structures
- **Developer Experience**: IntelliSense and auto-completion

## Validation Checklist

Before considering the implementation complete, verify:

### Code Quality
- [ ] All files follow established naming conventions
- [ ] XML documentation is complete and accurate
- [ ] Type aliases are used consistently
- [ ] Proper using statements and namespaces

### Functionality
- [ ] All CRUD operations work correctly
- [ ] Relationships are properly configured
- [ ] Timestamps are managed automatically
- [ ] Error handling follows established patterns

### API Design
- [ ] Endpoints follow RESTful conventions
- [ ] Authorization is properly configured
- [ ] Input validation is comprehensive
- [ ] Response types are documented

### Database
- [ ] Migration applies successfully
- [ ] Indexes are appropriate for expected queries
- [ ] Foreign key constraints are correct
- [ ] Seed data is appropriate (if any)

### Integration
- [ ] Service is registered in DI container
- [ ] DbSet is added to DbContext
- [ ] OnModelCreating is called
- [ ] No compilation errors

## Common Customizations

### Adding Custom Business Logic
- Add methods to the Service class
- Create corresponding endpoints in the Controller
- Add custom query methods to the Query class

### Adding Validation
- Use data annotations on Model properties
- Add custom validation in Service methods
- Consider FluentValidation for complex scenarios

### Adding Caching
- Implement caching in Service methods
- Use `IMemoryCache` or distributed caching
- Consider cache invalidation strategies

### Adding Pagination
- Modify List methods to accept pagination parameters
- Return `PagedResult<T>` instead of arrays
- Add appropriate query methods

### Adding Soft Delete
- Implement soft delete in Service.DeleteAsync
- Filter deleted records in Query methods
- Add restore functionality if needed

### Adding Internal Properties
Internal properties are database-only fields that are never exposed via the API:

```csharp
// In Entity class
#region Internal properties
public string? InternalId { get; set; }        // Example: External system ID
public string? ProcessingStatus { get; set; }  // Example: Background job status
public byte[]? RowVersion { get; set; }        // Example: Concurrency token
#endregion
```

**Key Points:**
- Internal properties exist only in the Entity, not in Model classes
- Use `[MapperIgnoreSource]` and `[MapperIgnoreTarget]` to exclude from API
- Map to snake_case columns in `OnModelCreating`
- Useful for: external IDs, processing flags, concurrency tokens, audit fields

### Adding User Scoping
User-scoped entities belong to a specific user and require user isolation in all operations:

```csharp
// In Entity class
#region Internal properties
/// <summary>
/// The user ID who owns this entity.
/// </summary>
[Display(Name = "User ID")]
[Required]
public string UserId { get; set; } = null!;
#endregion
```

**Key Points:**
- Add `UserId` as internal-only field (not in Model classes)
- Use `[MapperIgnoreSource]` and `[MapperIgnoreTarget]` for `UserId`
- Map to `user_id` column and add index in `OnModelCreating`
- Query methods require `userId` parameter and filter by `e.UserId == userId`
- Service methods require `userId` parameter and set `record.UserId = userId` on create
- Controller reads `userId` from `User.GetNameIdentifier()` and passes to service
- Ensures complete user isolation and security

**When to use:**
- User-specific data (projects, volumes, personal settings)
- Multi-tenant applications
- Any entity that should be isolated per user

**When NOT to use:**
- System-wide configuration
- Shared reference data
- Global entities accessible to all users

## Troubleshooting

### Common Issues
1. **Migration fails**: Check entity configuration and relationships
2. **Mapping errors**: Verify Mapperly attributes and property names
3. **DI errors**: Ensure service is registered in IServiceCollectionExtensions
4. **Authorization fails**: Check policy configuration and controller attributes
5. **Query performance**: Add appropriate indexes and optimize Include statements 