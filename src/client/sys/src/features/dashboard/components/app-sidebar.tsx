"use client"

import * as React from "react"
import {
  AudioWaveform,
  BookOpen,
  Bot,
  Command,
  Frame,
  GalleryVerticalEnd,
  Map,
  PieChart,
  Settings2,
  SquareTerminal,
} from "lucide-react"

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
import type { LucideIcon } from "lucide-react"

//------------------------------
// Type definitions for the sidebar data structure
//------------------------------

export interface SidebarUser {
  name: string
  email: string
  avatar: string
}

export interface SidebarTeam {
  name: string
  logo: LucideIcon
  plan: string
}

export interface SidebarNavItem {
  title: string
  url: string
}

export interface SidebarNavMainItem {
  title: string
  url: string
  icon: LucideIcon
  isActive?: boolean
  items: SidebarNavItem[]
}

export interface SidebarProject {
  name: string
  url: string
  icon: LucideIcon
}

export interface SidebarData {
  user: SidebarUser
  teams: SidebarTeam[]
  navMain: SidebarNavMainItem[]
  projects: SidebarProject[]
}


//------------------------------
// Sidebar selection state types
//------------------------------

export interface SidebarSelection {
  activeTeamId?: string
  activeNavItemId?: string
  activeSubItemId?: string
  activeProjectId?: string
  expandedItems: string[]
}

// Sidebar state management
export interface SidebarState {
  isCollapsed: boolean
  isMobile: boolean
  isOpen: boolean
}

// Sidebar actions/callbacks
export interface SidebarActions {
  onTeamSelect: (teamId: string) => void
  onNavItemSelect: (itemId: string) => void
  onSubItemSelect: (itemId: string, parentId: string) => void
  onProjectSelect: (projectId: string) => void
  onToggleExpanded: (itemId: string) => void
  onToggleCollapse: () => void
  onToggleMobile: () => void
}

// Main sidebar handle type for React Router
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

// This is sample data.
const data: SidebarData = {
  user: {
    name: "shadcn",
    email: "m@example.com",
    avatar: "/avatars/shadcn.jpg",
  },
  teams: [
    {
      name: "Acme Inc",
      logo: GalleryVerticalEnd,
      plan: "Enterprise",
    },
    {
      name: "Acme Corp.",
      logo: AudioWaveform,
      plan: "Startup",
    },
    {
      name: "Evil Corp.",
      logo: Command,
      plan: "Free",
    },
  ],
  navMain: [
    {
      title: "Playground",
      url: "#",
      icon: SquareTerminal,
      isActive: true,
      items: [
        {
          title: "History",
          url: "#",
        },
        {
          title: "Starred",
          url: "#",
        },
        {
          title: "Settings",
          url: "#",
        },
      ],
    },
    {
      title: "Models",
      url: "#",
      icon: Bot,
      items: [
        {
          title: "Genesis",
          url: "#",
        },
        {
          title: "Explorer",
          url: "#",
        },
        {
          title: "Quantum",
          url: "#",
        },
      ],
    },
    {
      title: "Documentation",
      url: "#",
      icon: BookOpen,
      items: [
        {
          title: "Introduction",
          url: "#",
        },
        {
          title: "Get Started",
          url: "#",
        },
        {
          title: "Tutorials",
          url: "#",
        },
        {
          title: "Changelog",
          url: "#",
        },
      ],
    },
    {
      title: "Settings",
      url: "#",
      icon: Settings2,
      items: [
        {
          title: "General",
          url: "#",
        },
        {
          title: "Team",
          url: "#",
        },
        {
          title: "Billing",
          url: "#",
        },
        {
          title: "Limits",
          url: "#",
        },
      ],
    },
  ],
  projects: [
    {
      name: "Design Engineering",
      url: "#",
      icon: Frame,
    },
    {
      name: "Sales & Marketing",
      url: "#",
      icon: PieChart,
    },
    {
      name: "Travel",
      url: "#",
      icon: Map,
    },
  ],
}

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader>
        <TeamSwitcher teams={data.teams} />
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={data.navMain} />
        <NavProjects projects={data.projects} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={data.user} />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  )
}

export { data }
