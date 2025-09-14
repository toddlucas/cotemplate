# Organization Service and API Plan

**Date**: 2025-09-14
**Status**: Phase 1 Complete  
**Entity**: Organization  

## Overview

This document outlines the planned service and API methods for the Organization entity, including role-based access control, authorization policies, and implementation considerations.

## Service Layer Methods (`OrganizationService`)

### Standard CRUD Operations
- `ReadOrDefaultAsync(long id, CancellationToken cancellationToken)` - Get single organization
- `ReadDetailOrDefaultAsync(long id, CancellationToken cancellationToken)` - Get organization with related entities
- `ListAsync(CancellationToken cancellationToken)` - Get all organizations
- `CreateAsync(OrganizationModel model)` - Create new organization
- `UpdateAsync(OrganizationModel model)` - Update existing organization
- `DeleteAsync(long id)` - Delete organization

### Business Logic Methods
- `FindByCodeAsync(string code, CancellationToken cancellationToken)` - Find by code
- `FindByNameAsync(string name, CancellationToken cancellationToken)` - Search by name
- `FindByParentAsync(long parentOrgId, CancellationToken cancellationToken)` - Get child organizations
- `FindRootOrganizationsAsync(CancellationToken cancellationToken)` - Get root organizations
- `FindByStatusAsync(string status, CancellationToken cancellationToken)` - Filter by status
- `FindByNameAndStatusAsync(string name, string status, CancellationToken cancellationToken)` - Combined search

### Hierarchy Management Methods
- `GetOrganizationHierarchyAsync(long id, CancellationToken cancellationToken)` - Get full hierarchy (parent + children)
- `GetAncestorsAsync(long id, CancellationToken cancellationToken)` - Get all parent organizations up to root
- `GetDescendantsAsync(long id, CancellationToken cancellationToken)` - Get all child organizations recursively
- `ValidateHierarchyAsync(OrganizationModel model)` - Ensure no circular references

### Member Management Methods
- `GetMembersAsync(long orgId, CancellationToken cancellationToken)` - Get organization members
- `GetMemberCountAsync(long orgId, CancellationToken cancellationToken)` - Get member count
- `GetMembersByRoleAsync(long orgId, string role, CancellationToken cancellationToken)` - Get members by role

### Cross-Tenant Operations (Future)
- `ListAllTenantsAsync(CancellationToken cancellationToken)` - Get all tenants (Admin/Reseller only)
- `GetTenantOrganizationsAsync(Guid tenantId, CancellationToken cancellationToken)` - Get tenant organizations (Admin/Reseller only)
- `BulkOperations` - Bulk create/update/delete (Admin only)

### System Management (Future)
- `GetSystemStatsAsync(CancellationToken cancellationToken)` - System-wide statistics (Admin only)
- `GetTenantStatsAsync(Guid tenantId, CancellationToken cancellationToken)` - Tenant statistics (Admin/Reseller only)
- `GetOrganizationStatsAsync(long orgId, CancellationToken cancellationToken)` - Organization statistics (All roles)

## API Layer Methods (`OrganizationController`)

### Standard REST Endpoints
- `GET /api/access/organization/{id}` - Get single organization
- `GET /api/access/organization/{id}/detail` - Get organization with related entities
- `GET /api/access/organization` - List all organizations
- `POST /api/access/organization` - Create organization
- `PUT /api/access/organization` - Update organization
- `DELETE /api/access/organization/{id}` - Delete organization

### Search and Filter Endpoints
- `GET /api/access/organization/search?name={name}` - Search by name
- `GET /api/access/organization/search?code={code}` - Find by code
- `GET /api/access/organization/search?status={status}` - Filter by status
- `GET /api/access/organization/search?name={name}&status={status}` - Combined search
- `GET /api/access/organization/root` - Get root organizations

### Hierarchy Endpoints
- `GET /api/access/organization/{id}/hierarchy` - Get full hierarchy
- `GET /api/access/organization/{id}/ancestors` - Get parent organizations
- `GET /api/access/organization/{id}/children` - Get direct children
- `GET /api/access/organization/{id}/descendants` - Get all descendants

### Member Management Endpoints
- `GET /api/access/organization/{id}/members` - Get organization members
- `GET /api/access/organization/{id}/members/count` - Get member count
- `GET /api/access/organization/{id}/members/role/{role}` - Get members by role

### Cross-Tenant Endpoints (Future)
- `GET /api/access/organization/tenants` - Get all tenants (Reseller+ only)
- `GET /api/access/organization/tenant/{tenantId}` - Get tenant organizations (Reseller+ only)

### Bulk Operations (Future)
- `POST /api/access/organization/bulk` - Create multiple organizations (Admin only)
- `PUT /api/access/organization/bulk` - Update multiple organizations (Admin only)
- `DELETE /api/access/organization/bulk` - Delete multiple organizations (Admin only)

### System Management Endpoints (Future)
- `GET /api/access/organization/stats/system` - System statistics (Admin only)
- `GET /api/access/organization/stats/tenant` - Tenant statistics (Reseller+ only)
- `GET /api/access/organization/stats/organization` - Organization statistics (All roles)

## Role-Based Access Control

### Service Methods by Role

| Method Category | Admin | Reseller | Customer | Notes |
|---|---|---|---|---|
| **Basic CRUD Operations** | | | | |
| `ReadOrDefaultAsync` | ✅ | ✅ | ✅ | All roles can read their organizations |
| `ReadDetailOrDefaultAsync` | ✅ | ✅ | ✅ | Full details available to all |
| `ListAsync` | ✅ | ✅ | ✅ | All roles can list organizations |
| `CreateAsync` | ✅ | ✅ | ✅ | All roles can create organizations |
| `UpdateAsync` | ✅ | ✅ | ✅ | All roles can update their organizations |
| `DeleteAsync` | ✅ | ✅ | ❌ | Only Admin/Reseller can delete |
| **Search and Filter** | | | | |
| `FindByCodeAsync` | ✅ | ✅ | ✅ | All roles can search by code |
| `FindByNameAsync` | ✅ | ✅ | ✅ | All roles can search by name |
| `FindByStatusAsync` | ✅ | ✅ | ✅ | All roles can filter by status |
| `FindByNameAndStatusAsync` | ✅ | ✅ | ✅ | Combined search for all |
| **Hierarchy Management** | | | | |
| `FindByParentAsync` | ✅ | ✅ | ✅ | All roles can find children |
| `FindRootOrganizationsAsync` | ✅ | ✅ | ✅ | All roles can find roots |
| `GetOrganizationHierarchyAsync` | ✅ | ✅ | ✅ | Full hierarchy for all |
| `GetAncestorsAsync` | ✅ | ✅ | ✅ | Ancestor lookup for all |
| `GetDescendantsAsync` | ✅ | ✅ | ✅ | Descendant lookup for all |
| `ValidateHierarchyAsync` | ✅ | ✅ | ✅ | Validation for all |
| **Member Management** | | | | |
| `GetMembersAsync` | ✅ | ✅ | ✅ | All roles can see members |
| `GetMemberCountAsync` | ✅ | ✅ | ✅ | Member count for all |
| `GetMembersByRoleAsync` | ✅ | ✅ | ✅ | Role-based member lookup for all |
| **Cross-Tenant Operations** | | | | |
| `ListAllTenantsAsync` | ✅ | ✅ | ❌ | Admin/Reseller can see all tenants |
| `GetTenantOrganizationsAsync` | ✅ | ✅ | ❌ | Admin/Reseller can see tenant orgs |
| `BulkOperations` | ✅ | ❌ | ❌ | Only Admin can do bulk operations |
| **System Management** | | | | |
| `GetSystemStatsAsync` | ✅ | ❌ | ❌ | Only Admin gets system stats |
| `GetTenantStatsAsync` | ✅ | ✅ | ❌ | Admin/Reseller get tenant stats |
| `GetOrganizationStatsAsync` | ✅ | ✅ | ✅ | All roles get org stats |

### API Endpoints by Role

| Endpoint | Admin | Reseller | Customer | Authorization Policy |
|---|---|---|---|---|
| **Basic REST** | | | | |
| `GET /api/access/organization/{id}` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization/{id}/detail` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `POST /api/access/organization` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `PUT /api/access/organization` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `DELETE /api/access/organization/{id}` | ✅ | ✅ | ❌ | `RequireResellerRole` |
| **Search & Filter** | | | | |
| `GET /api/access/organization/search` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization/root` | ✅ | ✅ | ✅ | `RequireUserRole` |
| **Hierarchy** | | | | |
| `GET /api/access/organization/{id}/hierarchy` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization/{id}/ancestors` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization/{id}/children` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization/{id}/descendants` | ✅ | ✅ | ✅ | `RequireUserRole` |
| **Members** | | | | |
| `GET /api/access/organization/{id}/members` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization/{id}/members/count` | ✅ | ✅ | ✅ | `RequireUserRole` |
| `GET /api/access/organization/{id}/members/role/{role}` | ✅ | ✅ | ✅ | `RequireUserRole` |
| **Cross-Tenant** | | | | |
| `GET /api/access/organization/tenants` | ✅ | ✅ | ❌ | `RequireResellerRole` |
| `GET /api/access/organization/tenant/{tenantId}` | ✅ | ✅ | ❌ | `RequireResellerRole` |
| **Bulk Operations** | | | | |
| `POST /api/access/organization/bulk` | ✅ | ❌ | ❌ | `RequireAdminRole` |
| `PUT /api/access/organization/bulk` | ✅ | ❌ | ❌ | `RequireAdminRole` |
| `DELETE /api/access/organization/bulk` | ✅ | ❌ | ❌ | `RequireAdminRole` |
| **System Management** | | | | |
| `GET /api/access/organization/stats/system` | ✅ | ❌ | ❌ | `RequireAdminRole` |
| `GET /api/access/organization/stats/tenant` | ✅ | ✅ | ❌ | `RequireResellerRole` |
| `GET /api/access/organization/stats/organization` | ✅ | ✅ | ✅ | `RequireUserRole` |

## Security Principles

### Customer Role
- **Scope**: Only their own tenant's organizations
- **Operations**: Full CRUD except delete
- **Access**: Can see all organizations within their tenant
- **Restrictions**: No cross-tenant access, no bulk operations, no system stats

### Reseller Role
- **Scope**: All tenants they manage
- **Operations**: Full CRUD including delete
- **Access**: Can see organizations across their managed tenants
- **Restrictions**: No bulk operations, no system-wide stats

### Admin Role
- **Scope**: System-wide access
- **Operations**: Full CRUD including bulk operations
- **Access**: Can see all organizations across all tenants
- **Restrictions**: None (full system access)

## Implementation Considerations

### Authorization Policies Needed
- `RequireUserRole` - Basic user access
- `RequireResellerRole` - Reseller-level access
- `RequireAdminRole` - Admin-level access

### Data Filtering
- **Customer**: RLS automatically filters to their tenant
- **Reseller**: RLS filters to their managed tenants
- **Admin**: RLS can be bypassed or show all data

### Tenant Context Management
- All service methods will use `IRequestDbGuard` for tenant context
- Tenant filtering handled by RLS in PostgreSQL
- No manual tenant parameters needed in service methods

### Validation
- Input validation for required fields (Name, Code uniqueness)
- Business rule validation (no circular hierarchy references)
- Status validation (valid status values)

### Error Handling
- Standard HTTP status codes (200, 201, 400, 404, 500)
- Consistent error response format
- Proper logging for debugging

### Performance Considerations
- Pagination for list operations (future enhancement)
- Caching for frequently accessed data (future enhancement)
- Efficient hierarchy queries to avoid N+1 problems

### API Documentation
- OpenAPI/Swagger documentation for all endpoints
- Clear parameter descriptions and response schemas
- Example requests and responses

### Audit Logging
- All operations should be logged with role context
- Sensitive operations (delete, bulk) require additional audit trails
- Cross-tenant access should be prominently logged

## Implementation Phases

### Phase 1: Core CRUD (✅ Complete)
- ✅ Basic service methods (Read, Create, Update, Delete)
- ✅ Standard REST endpoints
- ✅ Basic authorization
- ✅ Search and filter endpoints
- ✅ Hierarchy endpoints

### Phase 2: Search and Hierarchy (Short-term)
- Search and filter methods
- Hierarchy management methods
- Member management methods

### Phase 3: Advanced Features (Medium-term)
- Cross-tenant operations
- Bulk operations
- System management features

### Phase 4: Optimization (Long-term)
- Performance optimizations
- Caching implementation
- Advanced audit logging

## Dependencies

### Required Components
- `OrganizationMapper` (✅ Complete)
- `OrganizationQuery` (✅ Complete)
- `IRequestDbGuard` (Available)
- Authorization policies (To be implemented)

### Related Entities
- `OrganizationMember` (For member management)
- `Entity` (For organization entities)
- `TaskRecord` (For organization tasks)
- `Checklist` (For organization checklists)

## Notes

- All methods follow established server architecture patterns
- Tenant context managed at service layer using request guard
- Query layer remains clean and focused on data access
- Authorization handled at controller level with appropriate policies
- Future enhancements clearly identified and prioritized
