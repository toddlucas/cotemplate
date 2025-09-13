import { Outlet, useMatches } from "react-router-dom"
import { AppSidebar, type SidebarHandle } from "../features/dashboard/components/app-sidebar"
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "$/components/ui/breadcrumb"
import { Separator } from "$/components/ui/separator"
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "$/components/ui/sidebar"
import { useSidebarHandle } from '../features/dashboard/hooks/use-sidebar-handle'
import { data } from '../features/dashboard/components/app-sidebar' // REVIEW: Why is this needed?

export type DashboardLayoutHandle = {
  sidebar: SidebarHandle;
}

export default function DashboardLayout() {
  // const matches = useMatches();
  // // Get the active sidebar handle from the current route
  // // This can be used to customize sidebar behavior per route
  // const activeSidebar = [...matches].reverse().find(m =>
  //   m.handle && typeof m.handle === 'object' && 'sidebar' in m.handle
  // )?.handle as DashboardLayoutHandle | undefined;

  // TODO: Use activeSidebar to customize sidebar behavior
  // For example: activeSidebar?.sidebar.actions.onNavItemSelect(...)

  // The layout creates the sidebar handle instance
  const sidebarHandle = useSidebarHandle({
    initialData: data,
    initialSelection: {
      activeTeamId: "acme-inc",
      activeNavItemId: "identity", // This would be set based on current route
      expandedItems: ["identity"]
    }
  })

  return (
    <SidebarProvider>
      <AppSidebar />
      <SidebarInset>
        <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
          <div className="flex items-center gap-2 px-4">
            <SidebarTrigger className="-ml-1" />
            <Separator
              orientation="vertical"
              className="mr-2 data-[orientation=vertical]:h-4"
            />
            <Breadcrumb>
              <BreadcrumbList>
                <BreadcrumbItem className="hidden md:block">
                  <BreadcrumbLink href="#">
                    Building Your Application
                  </BreadcrumbLink>
                </BreadcrumbItem>
                <BreadcrumbSeparator className="hidden md:block" />
                <BreadcrumbItem>
                  <BreadcrumbPage>Data Fetching</BreadcrumbPage>
                </BreadcrumbItem>
              </BreadcrumbList>
            </Breadcrumb>
          </div>
        </header>
        <div className="p-4 pt-0">
          <Outlet />
        </div>
      </SidebarInset>
    </SidebarProvider>
  )
}
