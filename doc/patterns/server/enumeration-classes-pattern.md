# Enumeration Classes Pattern

## Overview

This document explains the enumeration classes pattern used in the codebase. This pattern provides a robust alternative to traditional C# enums, offering better type safety, extensibility, and database integration capabilities.

## Why Enumeration Classes?

Traditional C# enums have several limitations:
- **Type safety**: Enum values are just integers under the hood
- **Extensibility**: Cannot add behavior or properties to enum values
- **Database integration**: Limited control over how enums are stored and queried
- **Refactoring**: Changing enum values can break existing data

Enumeration classes solve these problems by:
- Providing strong typing with custom ID types
- Allowing additional properties and behavior
- Offering full control over database mapping
- Supporting ordered collections with custom sorting
- Enabling better debugging with meaningful display values

## Architecture

The enumeration system consists of three main base classes:

### 1. `Enumeration<TId>` - Base Foundation

```csharp
public abstract record Enumeration<TId>(TId Id, string Name) : IComparable<Enumeration<TId>>
    where TId : IComparable
```

**Key Features:**
- Generic ID type for flexibility (int, string, Guid, etc.)
- Implements `IComparable` for sorting
- Reflection-based `GetAll<T>()` method to discover static properties
- Immutable record type for value semantics

**Usage:**
```csharp
public record Status(int Id, string Name) : Enumeration<int>(Id, Name)
{
    public static Status Active => new(1, "Active");
    public static Status Inactive => new(2, "Inactive");
    
    public static IEnumerable<Status> GetAll() => GetAll<Status>();
}
```

### 2. `OrderedEnumeration<TId>` - Ordered Collections

```csharp
public abstract record OrderedEnumeration<TId>(TId Id, string Name, int Ordinal)
    : Enumeration<TId>(Id, Name)
    where TId : IComparable
```

**Key Features:**
- Adds `Ordinal` property for custom ordering
- Overrides `GetAll<T>()` to return items ordered by ordinal
- Includes `DebuggerDisplay` attribute for better debugging experience

**Usage:**
```csharp
public record Priority(int Id, string Name, int Ordinal) : OrderedEnumeration<int>(Id, Name, Ordinal)
{
    public static Priority Low => new(1, "Low", 10);
    public static Priority Medium => new(2, "Medium", 20);
    public static Priority High => new(3, "High", 30);
    
    public static IEnumerable<Priority> GetAll() => GetAll<Priority>();
}
```

### 3. `StringEnumeration` - String-Based Enumerations

```csharp
public abstract record StringEnumeration(string Id, string Name, int Ordinal)
    : OrderedEnumeration<string>(Id, Name, Ordinal)
```

**Key Features:**
- Specialized for string-based IDs
- Combines ordering with string identifiers
- Most commonly used pattern in the codebase

**Usage:**
```csharp
public record GenderEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 1;
    
    public static GenderEnum Male => new(10, "M", "Male");
    public static GenderEnum Female => new(20, "F", "Female");
    public static GenderEnum Other => new(30, "O", "Other");
    
    public static IEnumerable<GenderEnum> GetAll() => GetAll<GenderEnum>();
}
```

## Entity Framework Integration

### EnumerationBuilder Class

The `EnumerationBuilder` class provides Entity Framework integration:

```csharp
public static class EnumerationBuilder
{
    // Generic method for any enumeration type
    public static void OnCreating<T, TId>(this ModelBuilder modelBuilder, IEnumerable<T> items, string? tableName = null)
        where T : Enumeration<TId>
        where TId : IComparable
    
    // Specialized method for string enumerations
    public static void OnStringCreating<T>(this ModelBuilder modelBuilder, IEnumerable<T> items, int? keyLength = null, string? tableName = null) 
        where T : StringEnumeration
}
```

### Database Configuration

**In DbContext:**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Configure string enumerations
    EnumerationBuilder.OnStringCreating(modelBuilder, GenderEnum.GetAll(), GenderEnum.KeyLength, "gender_type");
    EnumerationBuilder.OnStringCreating(modelBuilder, Currency.GetAll(), Currency.KeyLength, "currency");
}
```

**Generated Database Schema:**
```sql
CREATE TABLE gender_type (
    id VARCHAR(20) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    ordinal INT NOT NULL
);

CREATE TABLE currency (
    id VARCHAR(3) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    ordinal INT NOT NULL
);
```

**Seed Data:**
```sql
INSERT INTO gender_type (id, name, ordinal) VALUES 
('Male', 'Male', 10),
('Female', 'Female', 20),
('Other', 'Other', 30);

INSERT INTO currency (id, name, ordinal) VALUES 
('usd', 'USD', 1);
```

## Implementation Patterns

### 1. Basic String Enumeration

```csharp
public record GenderEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    // Define maximum length for database column
    public const int KeyLength = 20;
    
    // Define enumeration values
    public static GenderEnum Male => new(10, nameof(GenderType.Male), "Male");
    public static GenderEnum Female => new(20, nameof(GenderType.Female), "Female");
    public static GenderEnum Other => new(30, nameof(GenderType.Other), "Other");
    
    // Provide GetAll method for EF integration
    public static IEnumerable<GenderEnum> GetAll() => GetAll<GenderEnum>();
}
```

### 2. Complex Enumeration with Additional Properties

```csharp
public record Currency(int Ordinal, string Id, string Name, int Scale)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 3;
    
    public static Currency Usd => new(1, "usd", "USD", 2);
    public static Currency Eur => new(2, "eur", "EUR", 2);
    public static Currency Jpy => new(3, "jpy", "JPY", 0); // No decimal places
    
    public static IEnumerable<Currency> GetAll() => GetAll<Currency>();
}
```

### 3. Integer-Based Enumeration

```csharp
public record Status(int Id, string Name) : Enumeration<int>(Id, Name)
{
    public static Status Draft => new(1, "Draft");
    public static Status Published => new(2, "Published");
    public static Status Archived => new(3, "Archived");
    
    public static IEnumerable<Status> GetAll() => GetAll<Status>();
}
```

### 4. String Enumeration tied to C# enum

```csharp
public enum GenderType
{
    Male = 1,
    Female,
    Other,
}

public record GenderEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    // Define maximum length for database column
    public const int KeyLength = 20;
    
    // Define enumeration values
    public static GenderEnum Male => new(10, nameof(GenderType.Male), "Male");
    public static GenderEnum Female => new(20, nameof(GenderType.Female), "Female");
    public static GenderEnum Other => new(30, nameof(GenderType.Other), "Other");
    
    // Provide GetAll method for EF integration
    public static IEnumerable<GenderEnum> GetAll() => GetAll<GenderEnum>();
}
```

## Exporting

1. Converting a C# string type key to a typed enum in TypeScript

```csharp
public class IdentityUserModel
{
    ...
    /// <summary>
    /// The gender ID.
    /// </summary>
    [Display(Name = "Gender ID")]
    public string? GenderId { get; set; }
}

public class AppGenerationSpec : GenerationSpec
{
    public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
    {
        ...
        AddEnum<GenderType>();
        
        AddInterface<IdentityUserModel>()
            .Member(x => nameof(x.GenderId)).Type(nameof(GenderType), "./gender-type");
    }
}
```

## Best Practices

### 1. Naming Conventions
- Use descriptive names: `GenderEnum`, `Currency`, `OrderStatus`
- End with `Enum` suffix for clarity
- Use PascalCase for all identifiers

### 2. Ordinal Values
- Use meaningful gaps (10, 20, 30) to allow future insertions
- Keep ordinals consistent across related enumerations
- Document the ordering logic

### 3. Key Length Constants
- Always define `KeyLength` constant for string enumerations
- Use appropriate lengths based on expected values
- Consider database performance implications

### 4. Type Aliases
- Use `using TEnumeration = YourEnum;` for cleaner code
- Reduces repetition in static property definitions

### 5. Database Integration
- Always call `GetAll()` in EF configuration
- Use descriptive table names
- Consider indexing strategies for frequently queried enumerations

## Usage Examples

### Querying Enumerations
```csharp
// Get all values
var allGenders = GenderEnum.GetAll();

// Find by ID
var male = allGenders.FirstOrDefault(g => g.Id == "Male");

// Filter by ordinal
var highPriority = Priority.GetAll().Where(p => p.Ordinal >= 20);
```

### Using in Entity Models
```csharp
public class User
{
    public string Id { get; set; }
    public string? GenderId { get; set; }
    public string? CurrencyId { get; set; }
    
    // Navigation properties (if needed)
    public GenderEnum? Gender => GenderEnum.GetAll().FirstOrDefault(g => g.Id == GenderId);
    public Currency? Currency => Currency.GetAll().FirstOrDefault(c => c.Id == CurrencyId);
}
```

### API Serialization
```csharp
[HttpGet("genders")]
public IActionResult GetGenders()
{
    var genders = GenderEnum.GetAll()
        .Select(g => new { g.Id, g.Name, g.Ordinal });
    
    return Ok(genders);
}
```

## Migration from Traditional Enums

### Before (Traditional Enum)
```csharp
public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}
```

### After (Enumeration Class)
```csharp
public record GenderEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 20;
    
    public static GenderEnum Male => new(10, "Male", "Male");
    public static GenderEnum Female => new(20, "Female", "Female");
    public static GenderEnum Other => new(30, "Other", "Other");
    
    public static IEnumerable<GenderEnum> GetAll() => GetAll<GenderEnum>();
}
```

## Benefits

1. **Type Safety**: Strong typing prevents invalid values
2. **Extensibility**: Easy to add properties and behavior
3. **Database Control**: Full control over storage and querying
4. **Ordering**: Built-in support for custom ordering
5. **Debugging**: Better debugging experience with meaningful display values
6. **Refactoring**: Safer refactoring with compile-time checks
7. **API Integration**: Better serialization and deserialization support

## Key selection

* May be based of the string representation of a C# enum member, allows tying a C# enum to the enumeration values
* May be a single character (upper or lowercase), which is common in some database designs
* Are represented as strings in C# but may be exported as TypeScript enums
* Casing can be an issue, especially through serialization transforms (e.g., Pascal to camel), so snake case may be useful

## References

- [Microsoft DDD Patterns - Enumeration Classes](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types)
- [String Enums in C# - Everything You Need to Know](https://josipmisko.com/posts/string-enums-in-c-sharp-everything-you-need-to-know)
