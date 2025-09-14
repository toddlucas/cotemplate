import { ChevronRight } from "lucide-react"
import { Link } from "react-router-dom"

import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "$/components/ui/collapsible"
import {
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubButton,
  SidebarMenuSubItem,
} from "$/components/ui/sidebar"
import { useAppSidebar } from "../contexts/sidebar-context"
import type { SidebarNavMainItem } from "./app-sidebar"
import { isExternalItem, isExpandOnlyItem } from "./app-sidebar"

export function NavMain({
  items,
}: {
  items: SidebarNavMainItem[]
}) {
  const sidebarHandle = useAppSidebar()
  return (
    <SidebarGroup>
      <SidebarGroupLabel>Platform</SidebarGroupLabel>
      <SidebarMenu>
        {items.map((item) => {
          const isExpanded = sidebarHandle.isItemExpanded(item.id)
          const isExpandOnly = isExpandOnlyItem(item)
          const isActive = isExpandOnly ? false : sidebarHandle.isItemActive(item.id)

          return (
            <Collapsible
              key={item.id}
              asChild
              open={isExpanded}
              onOpenChange={() => sidebarHandle.actions.onToggleExpanded(item.id)}
              className="group/collapsible"
            >
              <SidebarMenuItem>
                <CollapsibleTrigger asChild>
                  <SidebarMenuButton
                    tooltip={item.title}
                    onClick={() => {
                      if (!isExpandOnly) {
                        sidebarHandle.actions.onNavItemSelect(item.id)
                      }
                    }}
                    isActive={isActive}
                  >
                    {item.icon && <item.icon />}
                    <span>{item.title}</span>
                    <ChevronRight className="ml-auto transition-transform duration-200 group-data-[state=open]/collapsible:rotate-90" />
                  </SidebarMenuButton>
                </CollapsibleTrigger>
                <CollapsibleContent>
                  <SidebarMenuSub>
                    {item.items?.map((subItem) => {
                      const isSubItemActive = sidebarHandle.isItemActive(item.id, subItem.id)

                      return (
                        <SidebarMenuSubItem key={subItem.id}>
                          <SidebarMenuSubButton
                            asChild
                            isActive={isSubItemActive}
                          >
                            {isExternalItem(subItem) ? (
                              <a
                                href={subItem.url}
                                target={subItem.target || '_self'}
                                onClick={() => sidebarHandle.actions.onSubItemSelect(subItem.id, item.id)}
                              >
                                <span>{subItem.title}</span>
                              </a>
                            ) : (
                              <Link
                                to={subItem.path}
                                state={subItem.state}
                                onClick={() => sidebarHandle.actions.onSubItemSelect(subItem.id, item.id)}
                              >
                                <span>{subItem.title}</span>
                              </Link>
                            )}
                          </SidebarMenuSubButton>
                        </SidebarMenuSubItem>
                      )
                    })}
                  </SidebarMenuSub>
                </CollapsibleContent>
              </SidebarMenuItem>
            </Collapsible>
          )
        })}
      </SidebarMenu>
    </SidebarGroup>
  )
}
