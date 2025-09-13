import type { SidebarHandle } from '../../dashboard/components/app-sidebar'

// Enhanced handle for UserList that includes sidebar integration
export const userListHandle = {
  label: 'Users',
  icon: 'users',
  path: '/identity/users',

  // Sidebar configuration for this route
  sidebar: {
    // When this route is active, set these sidebar states
    selection: {
      activeNavItemId: 'identity',
      activeSubItemId: 'users',
      expandedItems: ['identity']
    },

    // Actions to perform when entering this route
    onEnter: (sidebarHandle: SidebarHandle) => {
      sidebarHandle.actions.onNavItemSelect('identity')
      sidebarHandle.actions.onSubItemSelect('users', 'identity')
    },

    // Actions to perform when leaving this route
    onExit: (sidebarHandle: SidebarHandle) => {
      // Optional: clear selection or perform cleanup
    }
  }
} as const
