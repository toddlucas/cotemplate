import { useLocation } from 'react-router-dom'
import { useAppSidebarHandle } from '$/features/dashboard/hooks/use-sidebar-handle'
import type { SidebarData } from '../components/sidebar-types'

export interface BreadcrumbItem {
  title: string
  url?: string
  isExternal?: boolean
  target?: string
}

export interface RouteBreadcrumbHandle {
  breadcrumbs?: BreadcrumbItem[]
  breadcrumbTitle?: string
}

interface UseBreadcrumbsProps {
  data: SidebarData
}

export function useBreadcrumbs({ data }: UseBreadcrumbsProps) {
  const location = useLocation()
  const currentPathname = location.pathname

  // For dashboard routes, use the sidebar-based breadcrumbs
  const isDashboardRoute = currentPathname.startsWith('/dashboard') ||
    currentPathname.startsWith('/identity') ||
    currentPathname.startsWith('/models') ||
    currentPathname.startsWith('/playground') ||
    currentPathname.startsWith('/settings') ||
    currentPathname.startsWith('/tools')

  if (isDashboardRoute) {
    // Determine selection from path
    let activeNavItemId = "models"
    let activeSubItemId: string | undefined

    if (currentPathname.startsWith('/identity')) {
      activeNavItemId = "models"
      activeSubItemId = "users"
    } else if (currentPathname.startsWith('/playground')) {
      activeNavItemId = "playground"
    } else if (currentPathname.startsWith('/settings')) {
      activeNavItemId = "settings"
    } else if (currentPathname.startsWith('/tools')) {
      activeNavItemId = "tools"
    } else if (currentPathname.startsWith('/dashboard')) {
      activeNavItemId = "dashboard"
    }

    const sidebarHandle = useAppSidebarHandle({
      initialData: data,
      initialSelection: {
        activeTeamId: "acme-inc",
        activeNavItemId,
        activeSubItemId,
        expandedItems: [activeNavItemId]
      }
    })

    const sidebarBreadcrumbs = sidebarHandle.getActiveBreadcrumbs()

    // Convert sidebar breadcrumbs to our format
    return sidebarBreadcrumbs.map((bc: any) => ({
      title: bc.title,
      url: bc.url,
      isExternal: false
    }))
  }

  // For other routes, generate breadcrumbs from the path
  const pathSegments = currentPathname.split('/').filter(Boolean)

  const breadcrumbs: BreadcrumbItem[] = []
  let currentPath = ''

  pathSegments.forEach((segment) => {
    currentPath += `/${segment}`

    // Convert segment to title (capitalize and replace hyphens with spaces)
    const title = segment
      .split('-')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ')

    breadcrumbs.push({
      title,
      url: currentPath,
      isExternal: false
    })
  })

  return breadcrumbs
}
