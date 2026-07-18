import { createContext, useContext, useState } from 'react'
import type { ReactNode } from 'react'

interface WorkspaceContextType {
  currentRole: string | null
  setCurrentRole: (role: string | null) => void
}

const WorkspaceContext = createContext<WorkspaceContextType>({
  currentRole: null,
  setCurrentRole: () => {}
})

export function WorkspaceProvider({ children }: { children: ReactNode }) {
  const [currentRole, setCurrentRole] = useState<string | null>(null)

  return (
    <WorkspaceContext.Provider value={{ currentRole, setCurrentRole }}>
      {children}
    </WorkspaceContext.Provider>
  )
}

export function useWorkspace() {
  return useContext(WorkspaceContext)
}