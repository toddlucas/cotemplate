import { Outlet } from "react-router-dom"
import { AppSidebar } from "$/features/dashboard/components/app-sidebar"
import { type SidebarHandle } from "$/features/dashboard/components/sidebar-types"
import { BreadcrumbNav } from "$/features/dashboard/components/breadcrumb-nav"
import { Separator } from "$/components/ui/separator"
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "$/components/ui/sidebar"
import { useAppSidebarHandle } from '$/features/dashboard/hooks/use-sidebar-handle'
import { AppSidebarProvider } from '$/features/dashboard/contexts/sidebar-context'
import { data } from '../constants/sidebar-data' // REVIEW: Why is this needed?

export type DashboardLayoutHandle = {
  sidebar: SidebarHandle;
}

export default function DashboardLayout() {
  // The layout creates the sidebar handle instance
  const sidebarHandle = useAppSidebarHandle({
    initialData: data,
    initialSelection: {
      activeTeamId: "acme-inc",
      activeNavItemId: "models",
      expandedItems: ["models"]
    }
  })

  return (
    <SidebarProvider>
      <AppSidebarProvider sidebarHandle={sidebarHandle}>
        <AppSidebar sidebarHandle={sidebarHandle} />
        <SidebarInset>
          <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
            <div className="flex items-center gap-2 px-4">
              <SidebarTrigger className="-ml-1" />
              <Separator
                orientation="vertical"
                className="mr-2 data-[orientation=vertical]:h-4"
              />
              <BreadcrumbNav
                data={data}
                selection={sidebarHandle.selection}
              />
            </div>
          </header>
          <div className="p-4 pt-0">
            <Outlet />
          </div>
        </SidebarInset>
      </AppSidebarProvider>
    </SidebarProvider>
  )
}
