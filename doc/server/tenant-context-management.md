# Tenant Context Management

This document explains how tenant context is managed for database connections in the application, enabling Row Level Security (RLS) for multi-tenant data isolation.

## Overview

The application uses a **Read Guard Pattern** combined with **PostgreSQL Row Level Security** to ensure that all database operations are automatically scoped to the current tenant. This provides transparent multi-tenancy without requiring manual tenant filtering in every query.

## Architecture

### Core Components

1. **`IRequestDbGuard`** - Interface for managing database transactions per request
2. **`RequestDbGuard<TDb>`** - PostgreSQL implementation with tenant context setting
3. **`PassthroughRequestDbGuard`** - No-op implementation for SQLite/other providers
4. **`IRequestDbGuardFactory`** - Factory for creating appropriate guard instances
5. **`WriteGuardInterceptor`** - EF Core interceptor for automatic write transaction promotion

### Database Provider Support

- **PostgreSQL**: Full tenant context with RLS support
- **SQLite**: Passthrough implementation (no tenant isolation)

## How It Works

### 1. Request Lifecycle

```
HTTP Request
    ↓
TenantContextMiddleware (sets tenant from claims/route)
    ↓
Controller Action
    ↓
IRequestDbGuard.EnsureReadAsync()
    ↓
RequestDbGuard creates transaction + sets app.current_tenant
    ↓
Query executes with RLS filtering
    ↓
SaveChanges (if needed)
    ↓
WriteGuardInterceptor promotes to write transaction
    ↓
Request ends → Guard disposed → Transaction committed
```

### 2. Tenant Context Setting

For PostgreSQL, the tenant context is set using PostgreSQL's session variables:

```sql
SELECT set_config('app.current_tenant', @tenant_id, true);
```

The `true` parameter makes the setting transaction-scoped, so it automatically resets when the transaction ends.

### 3. Row Level Security Integration

PostgreSQL RLS policies can access the tenant context:

```sql
-- Example RLS policy
CREATE POLICY tenant_isolation ON users
    FOR ALL
    TO application_role
    USING (tenant_id = current_setting('app.current_tenant')::uuid);
```

## Implementation Details

### RequestDbGuard Class

The `RequestDbGuard<TDb>` class manages the transaction lifecycle:

```csharp
public sealed class RequestDbGuard<TDb> : IRequestDbGuard where TDb : DbContext
{
    private readonly TDb _db;
    private readonly TenantContext<string> _tenantContext;
    private IDbContextTransaction? _transaction;
    private bool _isReadOnly;
    
    // Ensures read-only transaction with tenant context
    public async Task EnsureReadAsync(CancellationToken cancellationToken = default)
    
    // Promotes to write transaction when needed
    public async Task EnsureWriteAsync(CancellationToken cancellationToken = default)
    
    // Sets tenant context for RLS
    private async Task SetTenantContextAsync(CancellationToken cancellationToken)
}
```

### Key Features

1. **Lazy Transaction Creation**: Transactions are only created when first needed
2. **Read-First Approach**: Starts with read-only transactions for better performance
3. **Automatic Promotion**: Write transactions are created automatically when `SaveChanges` is called
4. **Transaction Scoping**: Each request gets its own transaction that's disposed at request end
5. **Tenant Context**: Automatically sets `app.current_tenant` for RLS policies

### Factory Pattern

The `IRequestDbGuardFactory` breaks circular dependencies:

```csharp
public interface IRequestDbGuardFactory
{
    IRequestDbGuard CreateGuard();
}
```

The factory chooses the appropriate implementation based on:
- Database provider (PostgreSQL vs SQLite)
- Configuration setting (`UseTenantInterceptor`)

## Usage in Controllers

### Read Operations

```csharp
[HttpGet]
public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
{
    await _guard.EnsureReadAsync(cancellationToken);
    
    var users = await _dbContext.Users
        .Where(u => u.IsActive)
        .ToListAsync(cancellationToken);
        
    return Ok(users);
}
```

### Write Operations

```csharp
[HttpPost]
public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
{
    await _guard.EnsureReadAsync(cancellationToken);
    
    var user = new User { Name = request.Name };
    _dbContext.Users.Add(user);
    
    // WriteGuardInterceptor automatically promotes to write transaction
    await _dbContext.SaveChangesAsync(cancellationToken);
    
    return Ok(user);
}
```

### Mixed Operations

```csharp
[HttpPut("{id}")]
public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request, CancellationToken cancellationToken)
{
    await _guard.EnsureReadAsync(cancellationToken);
    
    var user = await _dbContext.Users.FindAsync(id);
    if (user == null) return NotFound();
    
    if (NeedsUpdate(user, request))
    {
        await _guard.EnsureWriteAsync(cancellationToken);
        user.Name = request.Name;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    return Ok(user);
}
```

## Configuration

### Service Registration

```csharp
// In Program.cs or Startup.cs
services.AddCorpDbConfiguration(configuration);
```

This registers:
- `IRequestDbGuardFactory` → `RequestDbGuardFactory`
- `IRequestDbGuard` → Factory-created instance
- `WriteGuardInterceptor` (PostgreSQL only)
- `CorpDbContext` with appropriate interceptors

### Configuration Settings

```json
{
  "UseTenantInterceptor": true,
  "DatabaseProvider": "Npgsql"
}
```

- `UseTenantInterceptor`: Enables tenant context management
- `DatabaseProvider`: Determines which implementation to use

## Database Setup

### PostgreSQL RLS Policies

Enable RLS on tables that need tenant isolation:

```sql
-- Enable RLS
ALTER TABLE users ENABLE ROW LEVEL SECURITY;

-- Create tenant isolation policy
CREATE POLICY tenant_isolation ON users
    FOR ALL
    TO application_role
    USING (tenant_id = current_setting('app.current_tenant')::uuid);
```

### Required Columns

Tables must have a `tenant_id` column:

```sql
ALTER TABLE users ADD COLUMN tenant_id UUID NOT NULL;
CREATE INDEX idx_users_tenant_id ON users(tenant_id);
```

## Benefits

1. **Transparent Multi-Tenancy**: No manual tenant filtering required in queries
2. **Performance**: Read-only transactions for read operations
3. **Security**: Database-level tenant isolation via RLS
4. **Flexibility**: Works with any EF Core query
5. **Provider Agnostic**: Graceful degradation for non-PostgreSQL providers

## Limitations

1. **PostgreSQL Only**: Full tenant isolation requires PostgreSQL RLS
2. **Transaction Scoping**: Each request must use its own transaction
3. **Connection Pooling**: Transactions are held for the entire request duration

## Troubleshooting

### Common Issues

1. **Missing Tenant Context**: Ensure `TenantContextMiddleware` is registered and running
2. **RLS Not Working**: Verify RLS policies are created and `app.current_tenant` is set
3. **Circular Dependencies**: Use the factory pattern for service registration
4. **Transaction Errors**: Ensure proper disposal of guards and transactions

### Debugging

Enable logging to see tenant context setting:

```csharp
// In RequestDbGuard.SetTenantContextAsync()
_logger.LogDebug("Setting tenant context: {TenantId}", tenantId);
```

## Future Enhancements

1. **AOP Integration**: Use Metalama for declarative tenant context
2. **Batch Operations**: Optimize for bulk operations
3. **Connection Pooling**: Improve connection management
4. **Metrics**: Add performance monitoring
5. **Testing**: Enhanced test utilities for multi-tenant scenarios
