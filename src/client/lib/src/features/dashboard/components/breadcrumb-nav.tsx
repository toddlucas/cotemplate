import { Fragment } from "react"
import { Link } from "react-router-dom"
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "$/components/ui/breadcrumb"
import { useBreadcrumbs, type BreadcrumbItem as BreadcrumbItemType } from "../hooks/use-breadcrumbs"
import type { SidebarData, SidebarSelection } from "./sidebar-types"

interface BreadcrumbNavProps {
  data: SidebarData
  className?: string
  selection?: Partial<SidebarSelection>
}

export function BreadcrumbNav({ data, className, selection }: BreadcrumbNavProps) {
  const breadcrumbs = useBreadcrumbs({ data, selection })

  if (breadcrumbs.length === 0) {
    return null
  }

  return (
    <Breadcrumb className={className}>
      <BreadcrumbList>
        {breadcrumbs.map((breadcrumb: BreadcrumbItemType, index: number) => (
          <Fragment key={index}>
            {index > 0 && <BreadcrumbSeparator className="hidden md:block" />}
            <BreadcrumbItem className={index === 0 ? "hidden md:block" : ""}>
              {breadcrumb.url && index < breadcrumbs.length - 1 ? (
                breadcrumb.isExternal ? (
                  <BreadcrumbLink
                    href={breadcrumb.url}
                    target={breadcrumb.target}
                    rel="noopener noreferrer"
                  >
                    {breadcrumb.title}
                  </BreadcrumbLink>
                ) : (
                  <BreadcrumbLink asChild>
                    <Link to={breadcrumb.url}>
                      {breadcrumb.title}
                    </Link>
                  </BreadcrumbLink>
                )
              ) : (
                <BreadcrumbPage>{breadcrumb.title}</BreadcrumbPage>
              )}
            </BreadcrumbItem>
          </Fragment>
        ))}
      </BreadcrumbList>
    </Breadcrumb>
  )
}
