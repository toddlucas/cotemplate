"use client"

import type { ComponentProps } from "react"
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
// Base types for discriminated unions
//------------------------------

// Base interface for all sidebar items
export interface BaseSidebarItem {
  id: string
  isExternal?: boolean
}

// Internal route properties
export interface InternalRoute {
  isExternal?: false
  path: string
  state?: any
}

// External URL properties
export interface ExternalRoute {
  isExternal: true
  url: string
  target?: '_blank' | '_self'
}

//------------------------------
// Type definitions for the sidebar data structure
//------------------------------

export interface SidebarUser {
  name: string
  email: string
  avatar: string
}

export interface SidebarTeam {
  id: string
  name: string
  logo: LucideIcon
  plan: string
}

// Navigation item with base types
export type SidebarNavItem = BaseSidebarItem & {
  title: string
} & (InternalRoute | ExternalRoute)

// Main navigation item with base types
export type SidebarNavMainItem = BaseSidebarItem & {
  title: string
  icon: LucideIcon
  isActive?: boolean
  items: SidebarNavItem[]
} & (InternalRoute | ExternalRoute)

// Project with base types
export type SidebarProject = BaseSidebarItem & {
  name: string
  icon: LucideIcon
} & (InternalRoute | ExternalRoute)

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
      id: "acme-inc",
      name: "Acme Inc",
      logo: GalleryVerticalEnd,
      plan: "Enterprise",
    },
    {
      id: "acme-corp",
      name: "Acme Corp.",
      logo: AudioWaveform,
      plan: "Startup",
    },
    {
      id: "evil-corp",
      name: "Evil Corp.",
      logo: Command,
      plan: "Free",
    },
  ],
  navMain: [
    {
      id: "playground",
      title: "Playground",
      path: "/playground",
      icon: SquareTerminal,
      //isActive: true,
      items: [
        {
          id: "history",
          title: "History",
          path: "/playground/history",
        },
        {
          id: "starred",
          title: "Starred",
          path: "/playground/starred",
        },
        {
          id: "playground-settings",
          title: "Settings",
          path: "/playground/settings",
        },
      ],
    },
    {
      id: "models",
      title: "Models",
      path: "/models",
      icon: Bot,
      items: [
        {
          id: "genesis",
          title: "Genesis",
          path: "/models/genesis",
        },
        {
          id: "users",
          title: "Users",
          path: "/identity/users",
        },
        {
          id: "explorer",
          title: "Explorer",
          path: "/models/explorer",
        },
        {
          id: "quantum",
          title: "Quantum",
          path: "/models/quantum",
        },
      ],
    },
    {
      id: "documentation",
      title: "Documentation",
      isExternal: true,
      url: "https://docs.example.com",
      target: "_blank",
      icon: BookOpen,
      items: [
        {
          id: "introduction",
          title: "Introduction",
          isExternal: true,
          url: "https://docs.example.com/intro",
          target: "_blank",
        },
        {
          id: "get-started",
          title: "Get Started",
          isExternal: true,
          url: "https://docs.example.com/get-started",
          target: "_blank",
        },
        {
          id: "tutorials",
          title: "Tutorials",
          isExternal: true,
          url: "https://docs.example.com/tutorials",
          target: "_blank",
        },
        {
          id: "changelog",
          title: "Changelog",
          isExternal: true,
          url: "https://docs.example.com/changelog",
          target: "_blank",
        },
      ],
    },
    {
      id: "settings",
      title: "Settings",
      path: "/settings",
      icon: Settings2,
      items: [
        {
          id: "general",
          title: "General",
          path: "/settings/general",
        },
        {
          id: "team",
          title: "Team",
          path: "/settings/team",
        },
        {
          id: "billing",
          title: "Billing",
          path: "/settings/billing",
        },
        {
          id: "limits",
          title: "Limits",
          path: "/settings/limits",
        },
      ],
    },
  ],
  projects: [
    {
      id: "design-engineering",
      name: "Design Engineering",
      path: "/projects/design-engineering",
      icon: Frame,
    },
    {
      id: "sales-marketing",
      name: "Sales & Marketing",
      path: "/projects/sales-marketing",
      icon: PieChart,
    },
    {
      id: "travel",
      name: "Travel",
      isExternal: true,
      url: "https://travel.example.com",
      target: "_blank",
      icon: Map,
    },
  ],
}

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

//------------------------------
// Generic type guards that work with any sidebar item
//------------------------------

export function isExternalItem<T extends BaseSidebarItem>(item: T): item is T & ExternalRoute {
  return item.isExternal === true
}

export function isInternalItem<T extends BaseSidebarItem>(item: T): item is T & InternalRoute {
  return !item.isExternal
}

export { data }
