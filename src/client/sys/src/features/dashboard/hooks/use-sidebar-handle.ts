import { useState, useCallback, useMemo } from "react"
import type {
  SidebarHandle,
  SidebarSelection,
  SidebarState,
  SidebarActions,
  SidebarData
} from "../components/app-sidebar"

interface UseSidebarHandleProps {
  initialData: SidebarData
  initialSelection?: Partial<SidebarSelection>
  initialState?: Partial<SidebarState>
}

export function useSidebarHandle({
  initialData,
  initialSelection = {},
  initialState = {}
}: UseSidebarHandleProps): SidebarHandle {
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

    // Find active nav item
    const activeNavItem = initialData.navMain.find(item => item.title === selection.activeNavItemId)
    if (activeNavItem) {
      breadcrumbs.push({ title: activeNavItem.title, url: activeNavItem.url })

      // Find active sub-item
      if (selection.activeSubItemId) {
        const activeSubItem = activeNavItem.items.find(item => item.title === selection.activeSubItemId)
        if (activeSubItem) {
          breadcrumbs.push({ title: activeSubItem.title, url: activeSubItem.url })
        }
      }
    }

    // Find active project
    if (selection.activeProjectId) {
      const activeProject = initialData.projects.find(project => project.name === selection.activeProjectId)
      if (activeProject) {
        breadcrumbs.push({ title: activeProject.name, url: activeProject.url })
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
