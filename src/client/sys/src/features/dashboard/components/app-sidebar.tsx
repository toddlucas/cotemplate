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

export interface SidebarNavItem {
  id: string
  title: string
  url: string
}

export interface SidebarNavMainItem {
  id: string
  title: string
  url: string
  icon: LucideIcon
  isActive?: boolean
  items: SidebarNavItem[]
}

export interface SidebarProject {
  id: string
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
      url: "#",
      icon: SquareTerminal,
      //isActive: true,
      items: [
        {
          id: "history",
          title: "History",
          url: "#",
        },
        {
          id: "starred",
          title: "Starred",
          url: "#",
        },
        {
          id: "playground-settings",
          title: "Settings",
          url: "#",
        },
      ],
    },
    {
      id: "models",
      title: "Models",
      url: "#",
      icon: Bot,
      items: [
        {
          id: "genesis",
          title: "Genesis",
          url: "#",
        },
        {
          id: "users",
          title: "Users",
          url: "#",
        },
        {
          id: "explorer",
          title: "Explorer",
          url: "#",
        },
        {
          id: "quantum",
          title: "Quantum",
          url: "#",
        },
      ],
    },
    {
      id: "documentation",
      title: "Documentation",
      url: "#",
      icon: BookOpen,
      items: [
        {
          id: "introduction",
          title: "Introduction",
          url: "#",
        },
        {
          id: "get-started",
          title: "Get Started",
          url: "#",
        },
        {
          id: "tutorials",
          title: "Tutorials",
          url: "#",
        },
        {
          id: "changelog",
          title: "Changelog",
          url: "#",
        },
      ],
    },
    {
      id: "settings",
      title: "Settings",
      url: "#",
      icon: Settings2,
      items: [
        {
          id: "general",
          title: "General",
          url: "#",
        },
        {
          id: "team",
          title: "Team",
          url: "#",
        },
        {
          id: "billing",
          title: "Billing",
          url: "#",
        },
        {
          id: "limits",
          title: "Limits",
          url: "#",
        },
      ],
    },
  ],
  projects: [
    {
      id: "design-engineering",
      name: "Design Engineering",
      url: "#",
      icon: Frame,
    },
    {
      id: "sales-marketing",
      name: "Sales & Marketing",
      url: "#",
      icon: PieChart,
    },
    {
      id: "travel",
      name: "Travel",
      url: "#",
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

export { data }
