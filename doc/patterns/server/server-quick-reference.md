# Server Component Quick Reference

## 🏗️ File Structure
```
Corp.Contracts/src/{Namespace}/
├── {Entity}Model.cs           # Basic DTO
└── {Entity}DetailModel.cs     # DTO with relations + ITemporal

Corp.Data/src/{Namespace}/
├── {Entity}.cs                # EF Entity + OnModelCreating
├── {Entity}Query.cs           # Query methods
└── {Entity}Mapper.cs          # Mapperly mappings

Corp.Services/src/{Namespace}/
└── {Entity}Service.cs         # Business logic + CRUD

Corp.Web/src/Controllers/{Namespace}/
└── {Entity}Controller.cs      # HTTP endpoints
```

## 📝 Naming Conventions

| Component | Pattern | Example |
|-----------|---------|---------|
| **Entity** | `{Entity}` | `Author`, `Book` |
| **Model** | `{Entity}Model` | `AuthorModel` |
| **Detail Model** | `{Entity}DetailModel` | `AuthorDetailModel` |
| **Service** | `{Entity}Service` | `AuthorService` |
| **Controller** | `{Entity}Controller` | `AuthorController` |
| **Query** | `{Entity}Query` | `AuthorQuery` |
| **Mapper** | `{Entity}Mapper` | `AuthorMapper` |
| **Table** | `{entity}` (snake_case) | `author`, `book` |
| **Column** | `{property}` (snake_case) | `id`, `author_id`, `created_at` |
| **DbSet** | `{Entity}s` (plural) | `Authors`, `Books` |

## 🔧 Type Aliases (Standard)
```csharp
using Record = {Entity};
using Model = {Entity}Model;
using DetailModel = {Entity}DetailModel;
using TRecord = {Entity};  // For OnModelCreating
```

## 🎯 TypeScript Generation (TypeGen)
```csharp
// Add to AppGenerationSpec.cs
AddInterface<{Entity}Model>();
AddInterface<{Entity}DetailModel>();
```
**Output**: `src/client/lib/src/models/{entity}-model.ts`

## 📋 Standard Operations

### Service Methods
```csharp
Task<Model?> ReadOrDefaultAsync(long id, CancellationToken cancellationToken)
Task<DetailModel?> ReadDetailOrDefaultAsync(long id, CancellationToken cancellationToken)
Task<Model[]> ListAsync(CancellationToken cancellationToken)
Task<Model> CreateAsync(Model model)
Task<Model?> UpdateAsync(Model model)
Task<bool> DeleteAsync(long id)
```

### Query Methods
```csharp
Task<Record?> SingleOrDefaultAsync(long id, CancellationToken cancellationToken = default)
Task<Record?> TrackOrDefaultAsync(long id, CancellationToken cancellationToken = default)
Task<Record?> SingleDetailOrDefaultAsync(long id, CancellationToken cancellationToken = default)
Task<Record?> TrackDetailOrDefaultAsync(long id, CancellationToken cancellationToken = default)
Task<Record[]> ListAsync(CancellationToken cancellationToken = default)
```

### HTTP Endpoints
| Method | Route | Action | Returns |
|--------|-------|--------|---------|
| `GET` | `/{id:long}` | Get single | `DetailModel` |
| `GET` | `/` | List all | `Model[]` |
| `POST` | `/` | Create | `Model` |
| `PUT` | `/` | Update | `Model` |
| `DELETE` | `/{id:long}` | Delete | `NoContent` |

## 🚀 Quick Start Templates

### 1. Basic Model
```csharp
namespace Corp.{Namespace};

public class {Entity}Model
{
    [Display(Name = "ID")]
    public long Id { get; set; }

    [Display(Name = "{Property}")]
    public string {Property} { get; set; } = null!;
}

public class {Entity}DetailModel : {Entity}Model, ITemporal
{
    public {RelatedEntity}Model[] {RelatedEntities} { get; set; } = [];
    
    #region ITemporal
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    #endregion
}
```

### 2. Entity
```csharp
namespace Corp.{Namespace};
using TRecord = {Entity};

public class {Entity} : {Entity}Model, ITemporal
{
    public string? InternalId { get; set; }
    public {RelatedEntity}[] {RelatedEntities} { get; set; } = [];
    
    #region ITemporal
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    #endregion

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TRecord>().ToTable(nameof({Entity}));
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<TRecord>().Property(x => x.{Property}).HasColumnName("{property}");
        modelBuilder.Entity<TRecord>().HasIndex(b => b.{Property});
    }
}
```

### 3. Query
```csharp
namespace Corp.{Namespace};
using Record = {Entity};

public record {Entity}Query(DbSet<Record> DbSet, ILogger logger)
{
    public Task<Record?> SingleOrDefaultAsync(long id, CancellationToken cancellationToken = default) => 
        DbSet.Where(e => e.Id == id).SingleOrDefaultAsync(cancellationToken);

    public Task<Record?> TrackOrDefaultAsync(long id, CancellationToken cancellationToken = default) => 
        DbSet.AsTracking().Where(e => e.Id == id).SingleOrDefaultAsync(cancellationToken);

    public Task<Record[]> ListAsync(CancellationToken cancellationToken = default) => 
        DbSet.ToArrayAsync(cancellationToken);
}
```

### 4. Mapper
```csharp
namespace Corp.{Namespace};
using Record = {Entity}; using Model = {Entity}Model; using DetailModel = {Entity}DetailModel;

[Mapper(UseDeepCloning = true, PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
public static partial class {Entity}Mapper
{
    [MapperIgnoreSource(nameof(Record.InternalId), nameof(Record.{RelatedEntities}), 
                       nameof(Record.CreatedAt), nameof(Record.UpdatedAt), nameof(Record.DeletedAt))]
    public static partial Model ToModel(this Record source);

    [MapperIgnoreSource(nameof(Record.InternalId))]
    public static partial DetailModel ToDetailModel(this Record source);

    public static partial Model[] ToModels(this IEnumerable<Record> source);
    public static partial Record ToRecord(this Model source);

    public static void UpdateFrom(this Record record, Model model)
    {
        record.{Property} = model.{Property};
    }
}
```

### 5. Service
```csharp
namespace Corp.{Namespace};
using Record = {Entity}; using Model = {Entity}Model; using DetailModel = {Entity}DetailModel;

public class {Entity}Service(CorpDbContext dbContext, ILogger<{Entity}Service> logger)
{
    private readonly ILogger _logger = logger;
    private readonly CorpDbContext _dbContext = dbContext;
    private readonly DbSet<Record> _dbSet = dbContext.{Entity}s;
    private readonly {Entity}Query _query = new(dbContext.{Entity}s, logger);

    public async Task<Model?> ReadOrDefaultAsync(long id, CancellationToken cancellationToken)
    {
        Record? record = await _query.SingleOrDefaultAsync(id, cancellationToken);
        return record?.ToModel();
    }

    public async Task<Model> CreateAsync(Model model)
    {
        Record record = model.ToRecord();
        record.CreatedAt = record.UpdatedAt = DateTimeOffset.UtcNow;
        _dbSet.Add(record);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Created {entity} {EntityId}.", record.Id);
        return record.ToModel();
    }
}
```

### 6. Controller
```csharp
namespace Corp.Controllers.{Namespace};

[Route("api/[area]/[controller]")]
[Area(nameof({Namespace}))]
[Tags(nameof({Namespace}))]
[Authorize(Policy = AppPolicy.RequireUserRole)]
[ApiController]
public class {Entity}Controller(ILogger<{Entity}Controller> logger, {Entity}Service {entity}Service) : ControllerBase
{
    private readonly {Entity}Service _{entity}Service = {entity}Service;

    [HttpGet("{id:long}")]
    [Produces(typeof({Entity}DetailModel))]
    public async Task<ActionResult> Get(long id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest();
        var result = await _{entity}Service.ReadDetailOrDefaultAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Post({Entity}Model model)
    {
        if (model is null) return BadRequest();
        var result = await _{entity}Service.CreateAsync(model);
        return Ok(result);
    }
}
```

## ⚡ Essential Attributes

### Models
```csharp
[Display(Name = "Friendly Name")]
[Description("RFC 3339 timestamp description")]
```

### Mappers
```csharp
[Mapper(UseDeepCloning = true, PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
[MapperIgnoreSource(nameof(Record.PropertyName))]
[MapperIgnoreTarget(nameof(Record.PropertyName))]
```

### Controllers
```csharp
[Route("api/[area]/[controller]")]
[Area(nameof(Namespace))]
[Tags(nameof(Namespace))]
[Authorize(Policy = AppPolicy.RequireUserRole)]
[ApiController]
[HttpGet("{id:long}")]
[Produces(typeof(Model))]
[EndpointDescription("Description text")]
```

## 🔗 Relationships

### One-to-Many Setup
```csharp
// Parent Entity
public ChildEntity[] ChildEntities { get; set; } = [];

// Child Entity  
public long ParentEntityId { get; set; }
public ParentEntity ParentEntity { get; set; } = null!;

// EF Configuration
modelBuilder.Entity<ChildEntity>()
    .HasOne(x => x.ParentEntity)
    .WithMany(y => y.ChildEntities)
    .HasForeignKey(x => x.ParentEntityId)
    .IsRequired();
```

## 🛠️ Registration Checklist

### DbContext
```csharp
// Add DbSet
public DbSet<{Entity}> {Entity}s { get; set; } = null!;

// Add to OnModelCreating
{Entity}.OnModelCreating(modelBuilder);
```

### Services
```csharp
// Add to IServiceCollectionExtensions
serviceCollection.AddScoped<{Entity}Service>();
```

### TypeScript Generation
```csharp
// Add to AppGenerationSpec.cs
AddInterface<{Entity}Model>();
AddInterface<{Entity}DetailModel>();
```

## 🚨 Common Patterns

### Error Handling
```csharp
// Service Layer
return record?.ToModel();  // null for not found
return false;              // for failed operations

// Controller Layer
if (id <= 0) return BadRequest();
if (model is null) return BadRequest();
return result is null ? NotFound() : Ok(result);
return NoContent();  // for successful deletes
```

### Timestamps
```csharp
// Create
record.CreatedAt = record.UpdatedAt = DateTimeOffset.UtcNow;

// Update  
record.UpdatedAt = DateTimeOffset.UtcNow;
```

### Logging
```csharp
_logger.LogInformation("Created {entityName} {EntityId}.", record.Id);
_logger.LogInformation("Updated {entityName} {EntityId}.", record.Id);
_logger.LogInformation("Deleted {entityName} {EntityId}.", id);
```

## 🔧 Migration Commands
```bash
cd src/server/Corp.Data.Npgsql
dotnet ef migrations add Add{Entity}Entity
dotnet ef database update
```

## Configuration

We use the .NET Core Options pattern for configuration.

## 🐛 Quick Troubleshooting

| Issue | Check |
|-------|-------|
| **Migration fails** | Entity configuration, relationships |
| **Mapping errors** | Mapperly attributes, property names |
| **DI errors** | Service registration in IServiceCollectionExtensions |
| **Auth fails** | Policy configuration, controller attributes |
| **Performance** | Indexes, Include statements, AsTracking usage |

## 📚 Reference Links
- **Full Architecture Guide**: `doc/patterns/server-architecture-patterns.md`
- **Implementation Template**: `doc/patterns/server-component-template.md` 