# Sidebar Data Flow Architecture

This document explains how the sidebar handle system works across the application, providing a centralized way to manage sidebar state, selection, and interactions.

## Overview

The sidebar handle system provides a unified interface for managing sidebar state across all components. It includes selection state, UI state, actions, and utility methods that can be used throughout the application.

## Architecture

```
┌─────────────────┐       ┌──────────────────┐    ┌─────────────────┐
│   Layout        │       │   Sidebar        │    │   Page          │
│   (Provider)    │──────▶│   Component      │    │   Component     │
│                 │       │                  │    │   (UserList)    │
└─────────────────┘       └──────────────────┘    └─────────────────┘
         │                         │                      │
         ▼                         ▼                      ▼
┌────────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│ useAppSidebarHandle│    │ Receives handle  │    │ useSidebar()    │
│ Creates state      │    │ as props         │    │ Gets handle     │
│ & actions          │    │ Uses for UI      │    │ from context    │
└────────────────────┘    └──────────────────┘    └─────────────────┘
```

## Core Types

### SidebarHandle Interface

```typescript
export interface SidebarHandle {
  // Selection state
  selection: SidebarSelection
  
  // Current state
  state: SidebarState
  
  // Actions
  actions: SidebarActions
  
  // Data
  data: SidebarData
  
  // Utility methods
  isItemActive: (itemId: string, subItemId?: string) => boolean
  isItemExpanded: (itemId: string) => boolean
  getActiveBreadcrumbs: () => Array<{ title: string; url: string }>
}
```

### Selection State

```typescript
export interface SidebarSelection {
  activeTeamId?: string
  activeNavItemId?: string
  activeSubItemId?: string
  activeProjectId?: string
  expandedItems: string[]
}
```

### UI State

```typescript
export interface SidebarState {
  isCollapsed: boolean
  isMobile: boolean
  isOpen: boolean
}
```

### Actions

```typescript
export interface SidebarActions {
  onTeamSelect: (teamId: string) => void
  onNavItemSelect: (itemId: string) => void
  onSubItemSelect: (itemId: string, parentId: string) => void
  onProjectSelect: (projectId: string) => void
  onToggleExpanded: (itemId: string) => void
  onToggleCollapse: () => void
  onToggleMobile: () => void
}
```

## Integration Patterns

### Pattern 1: Context Provider (Recommended)

This is the recommended approach for most applications as it provides clean separation and easy access throughout the component tree.

#### Layout Level

```typescript
// DashboardLayout.tsx
import { useAppSidebarHandle } from '../features/dashboard/hooks/use-sidebar-handle'
import { SidebarProvider } from '../features/dashboard/contexts/sidebar-context'

export default function DashboardLayout() {
  const sidebarHandle = useAppSidebarHandle({
    initialData: data,
    initialSelection: {
      activeTeamId: "acme-inc",
      activeNavItemId: "identity",
      expandedItems: ["identity"]
    }
  })

  return (
    <SidebarProvider sidebarHandle={sidebarHandle}>
      <AppSidebar />
      <Outlet />
    </SidebarProvider>
  )
}
```

#### Sidebar Component

```typescript
// AppSidebar.tsx
import { useAppSidebar } from '../contexts/sidebar-context'

export function AppSidebar() {
  const sidebarHandle = useAppSidebar()
  
  return (
    <aside>
      {sidebarHandle.data.navMain.map(item => (
        <button
          key={item.title}
          onClick={() => sidebarHandle.actions.onNavItemSelect(item.title)}
          className={sidebarHandle.isItemActive(item.title) ? 'active' : ''}
        >
          {item.title}
        </button>
      ))}
    </aside>
  )
}
```

#### Page Component

```typescript
// UserList.tsx
import { useAppSidebar } from '../../dashboard/contexts/sidebar-context'

export default function UserList() {
  const sidebarHandle = useAppSidebar()
  
  // Update sidebar when component mounts
  useEffect(() => {
    sidebarHandle.actions.onNavItemSelect('identity')
    sidebarHandle.actions.onSubItemSelect('users', 'identity')
  }, [])
  
  // Use sidebar state
  const currentTeam = sidebarHandle.selection.activeTeamId
  const breadcrumbs = sidebarHandle.getActiveBreadcrumbs()
  
  return (
    <div>
      <h1>Users for {currentTeam}</h1>
      <nav>{breadcrumbs.map(b => b.title).join(' > ')}</nav>
      {/* Component content */}
    </div>
  )
}
```

### Pattern 2: Props Passing (Direct)

Use this pattern when you need explicit control over data flow or when context is not available.

```typescript
// Layout creates handle and passes to components
const sidebarHandle = useAppSidebarHandle({...})

return (
  <div>
    <AppSidebar sidebarHandle={sidebarHandle} />
    <UserList sidebarHandle={sidebarHandle} />
  </div>
)
```

### Pattern 3: Route Handles (React Router)

Use this pattern to define sidebar behavior at the route level.

```typescript
// Route configuration
const route = {
  path: '/identity/users',
  element: <UserList />,
  handle: {
    sidebar: {
      selection: { 
        activeNavItemId: 'identity',
        activeSubItemId: 'users'
      },
      onEnter: (handle) => {
        handle.actions.onNavItemSelect('identity')
        handle.actions.onSubItemSelect('users', 'identity')
      }
    }
  }
}
```

## Data Flow Examples

### Example 1: User Selects Navigation Item

1. User clicks "Identity" in sidebar
2. `sidebarHandle.actions.onNavItemSelect('identity')` is called
3. Selection state updates: `activeNavItemId: 'identity'`
4. Sidebar re-renders with active styling
5. Page components can read new state via `useAppSidebar()`

### Example 2: Page Updates Sidebar

1. UserList component mounts
2. `useEffect` calls `sidebarHandle.actions.onNavItemSelect('identity')`
3. Sidebar automatically updates to show "Identity" as active
4. Breadcrumbs are generated based on current selection

### Example 3: Team Switching

1. User selects different team from team switcher
2. `sidebarHandle.actions.onTeamSelect('new-team')` is called
3. All team-related state updates
4. Page components can react to team change
5. Breadcrumbs and navigation context update

## Utility Methods

### isItemActive(itemId, subItemId?)

Checks if a navigation item is currently active.

```typescript
const isActive = sidebarHandle.isItemActive('identity', 'users')
// Returns true if 'identity' is active and 'users' is the active sub-item
```

### isItemExpanded(itemId)

Checks if a navigation item is expanded.

```typescript
const isExpanded = sidebarHandle.isItemExpanded('identity')
// Returns true if 'identity' section is expanded
```

### getActiveBreadcrumbs()

Generates breadcrumb navigation based on current selection.

```typescript
const breadcrumbs = sidebarHandle.getActiveBreadcrumbs()
// Returns: [{ title: 'Identity', url: '/identity' }, { title: 'Users', url: '/identity/users' }]
```

## State Management

### Selection State

The selection state tracks what's currently selected in the sidebar:

- `activeTeamId`: Currently selected team/organization
- `activeNavItemId`: Main navigation item
- `activeSubItemId`: Sub-navigation item
- `activeProjectId`: Selected project
- `expandedItems`: Array of expanded items

### UI State

The UI state manages the visual state of the sidebar:

- `isCollapsed`: Whether sidebar is collapsed
- `isMobile`: Mobile view state
- `isOpen`: Open/closed state

### Actions

All state changes go through the actions interface:

- `onTeamSelect`: Switch teams
- `onNavItemSelect`: Select main nav items
- `onSubItemSelect`: Select sub-items
- `onProjectSelect`: Select projects
- `onToggleExpanded`: Expand/collapse items
- `onToggleCollapse`: Toggle sidebar collapse
- `onToggleMobile`: Toggle mobile view

## Best Practices

### 1. Use Context Provider

The context provider pattern is recommended for most use cases as it provides clean separation and easy access throughout the component tree.

### 2. Update Sidebar on Route Changes

Always update the sidebar state when navigating to new routes to maintain consistency.

```typescript
useEffect(() => {
  sidebarHandle.actions.onNavItemSelect('current-section')
}, [location.pathname])
```

### 3. Use Utility Methods

Leverage the utility methods for common operations rather than manually checking state.

```typescript
// Good
const isActive = sidebarHandle.isItemActive('identity')

// Avoid
const isActive = sidebarHandle.selection.activeNavItemId === 'identity'
```

### 4. Handle Loading States

Consider the sidebar state when showing loading states or error conditions.

```typescript
if (isLoading) {
  sidebarHandle.actions.onNavItemSelect('loading')
}
```

### 5. Type Safety

Always use the provided TypeScript types for better development experience and error prevention.

```typescript
import type { SidebarHandle } from '../features/dashboard/components/app-sidebar'
```

## Troubleshooting

### Common Issues

1. **Sidebar not updating**: Ensure you're using the same handle instance across components
2. **Context not available**: Make sure components are wrapped in `SidebarProvider`
3. **State not persisting**: Consider adding localStorage/sessionStorage to the hook
4. **Performance issues**: Use `useCallback` for action functions to prevent unnecessary re-renders

### Debug Tips

1. Log the current sidebar state to understand what's happening
2. Use React DevTools to inspect the context value
3. Check that actions are being called with correct parameters
4. Verify that components are re-rendering when state changes

## Future Enhancements

Potential improvements to consider:

1. **Persistence**: Add localStorage/sessionStorage support
2. **Animation**: Add transition states for smooth animations
3. **Accessibility**: Enhanced ARIA support and keyboard navigation
4. **Performance**: Memoization and optimization for large datasets
5. **Testing**: Utilities for testing sidebar interactions
