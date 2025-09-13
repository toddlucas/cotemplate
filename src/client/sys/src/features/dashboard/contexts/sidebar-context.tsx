import React, { createContext, useContext, ReactNode } from 'react'
import type { SidebarHandle } from '../components/app-sidebar'

// Create the context
const SidebarContext = createContext<SidebarHandle | null>(null)

// Provider component
interface SidebarProviderProps {
  children: ReactNode
  sidebarHandle: SidebarHandle
}

export function SidebarProvider({ children, sidebarHandle }: SidebarProviderProps) {
  return (
    <SidebarContext.Provider value={sidebarHandle}>
      {children}
    </SidebarContext.Provider>
  )
}

// Hook to use the sidebar context
export function useSidebar(): SidebarHandle {
  const context = useContext(SidebarContext)
  if (!context) {
    throw new Error('useSidebar must be used within a SidebarProvider')
  }
  return context
}

// Optional: Hook that returns null if no context (for optional usage)
export function useSidebarOptional(): SidebarHandle | null {
  return useContext(SidebarContext)
}
