import { useState, useCallback, useMemo } from "react"
import type {
  SidebarHandle,
  SidebarSelection,
  SidebarState,
  SidebarActions,
  SidebarData
} from "../components/sidebar-types"

interface useAppSidebarHandleProps {
  initialData: SidebarData
  initialSelection?: Partial<SidebarSelection>
  initialState?: Partial<SidebarState>
}

export function useAppSidebarHandle({
  initialData,
  initialSelection = {},
  initialState = {}
}: useAppSidebarHandleProps): SidebarHandle {
  // Selection state
  const [selection, setSelection] = useState<SidebarSelection>({
    activeTeamId: initialSelection.activeTeamId,
    activeNavItemId: initialSelection.activeNavItemId,
    activeSubItemId: initialSelection.activeSubItemId,
    activeProjectId: initialSelection.activeProjectId,
    expandedItems: initialSelection.expandedItems || [],
  })

  // UI state
  const [state, setState] = useState<SidebarState>({
    isCollapsed: initialState.isCollapsed || false,
    isMobile: initialState.isMobile || false,
    isOpen: initialState.isOpen || true,
  })

  // Actions - define each callback separately
  const onTeamSelect = useCallback((teamId: string) => {
    setSelection(prev => ({ ...prev, activeTeamId: teamId }))
  }, [])

  const onNavItemSelect = useCallback((itemId: string) => {
    setSelection(prev => ({
      ...prev,
      activeNavItemId: itemId,
      activeSubItemId: undefined // Clear sub-item when selecting main item
    }))
  }, [])

  const onSubItemSelect = useCallback((itemId: string, parentId: string) => {
    setSelection(prev => ({
      ...prev,
      activeNavItemId: parentId,
      activeSubItemId: itemId
    }))
  }, [])

  const onProjectSelect = useCallback((projectId: string) => {
    setSelection(prev => ({ ...prev, activeProjectId: projectId }))
  }, [])

  const onToggleExpanded = useCallback((itemId: string) => {
    setSelection(prev => ({
      ...prev,
      expandedItems: prev.expandedItems.includes(itemId)
        ? prev.expandedItems.filter(id => id !== itemId)
        : [...prev.expandedItems, itemId]
    }))
  }, [])

  const onToggleCollapse = useCallback(() => {
    setState(prev => ({ ...prev, isCollapsed: !prev.isCollapsed }))
  }, [])

  const onToggleMobile = useCallback(() => {
    setState(prev => ({ ...prev, isOpen: !prev.isOpen }))
  }, [])

  // Create actions object
  const actions: SidebarActions = useMemo(() => ({
    onTeamSelect,
    onNavItemSelect,
    onSubItemSelect,
    onProjectSelect,
    onToggleExpanded,
    onToggleCollapse,
    onToggleMobile,
  }), [
    onTeamSelect,
    onNavItemSelect,
    onSubItemSelect,
    onProjectSelect,
    onToggleExpanded,
    onToggleCollapse,
    onToggleMobile,
  ])

  // Utility methods
  const isItemActive = useCallback((itemId: string, subItemId?: string) => {
    if (subItemId) {
      return selection.activeNavItemId === itemId && selection.activeSubItemId === subItemId
    }
    return selection.activeNavItemId === itemId
  }, [selection])

  const isItemExpanded = useCallback((itemId: string) => {
    return selection.expandedItems.includes(itemId)
  }, [selection])

  const getActiveBreadcrumbs = useCallback(() => {
    const breadcrumbs: Array<{ title: string; url: string }> = []

    // Find active nav item by ID
    const activeNavItem = initialData.navMain.find(item => item.id === selection.activeNavItemId)
    if (activeNavItem) {
      // Use path for internal routes, url for external routes
      const itemUrl = activeNavItem.isExternal ? (activeNavItem as any).url : (activeNavItem as any).path
      breadcrumbs.push({ title: activeNavItem.title, url: itemUrl || '#' })

      // Find active sub-item by ID
      if (selection.activeSubItemId) {
        const activeSubItem = activeNavItem.items.find(item => item.id === selection.activeSubItemId)
        if (activeSubItem) {
          // Use path for internal routes, url for external routes
          const subItemUrl = activeSubItem.isExternal ? (activeSubItem as any).url : (activeSubItem as any).path
          breadcrumbs.push({ title: activeSubItem.title, url: subItemUrl || '#' })
        }
      }
    }

    // Find active project by ID
    if (selection.activeProjectId) {
      const activeProject = initialData.projects.find(project => project.id === selection.activeProjectId)
      if (activeProject) {
        // Use path for internal routes, url for external routes
        const projectUrl = activeProject.isExternal ? (activeProject as any).url : (activeProject as any).path
        breadcrumbs.push({ title: activeProject.name, url: projectUrl || '#' })
      }
    }

    return breadcrumbs
  }, [selection, initialData])

  return {
    selection,
    state,
    actions,
    data: initialData,
    isItemActive,
    isItemExpanded,
    getActiveBreadcrumbs,
  }
}
