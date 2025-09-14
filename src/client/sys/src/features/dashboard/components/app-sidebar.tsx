import type { ComponentProps } from "react"
import { NavMain } from "../components/nav-main"
import { NavProjects } from "../components/nav-projects"
import { NavUser } from "../components/nav-user"
import { TeamSwitcher } from "../components/team-switcher"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarRail,
} from "$/components/ui/sidebar"
import type { SidebarHandle } from "./sidebar-types"

interface AppSidebarProps {
  sidebarHandle: SidebarHandle
}

export function AppSidebar({ sidebarHandle, ...props }: AppSidebarProps & ComponentProps<typeof Sidebar>) {
  if (!sidebarHandle) {
    return null;
  }

  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader>
        <TeamSwitcher teams={sidebarHandle.data.teams} />
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={sidebarHandle.data.navMain} />
        <NavProjects projects={sidebarHandle.data.projects} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={sidebarHandle.data.user} />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  )
}
