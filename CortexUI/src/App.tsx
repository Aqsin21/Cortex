import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { WorkspaceProvider } from './context/WorkSpaceContext'  // ← ekle
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import DashboardPage from './pages/DashBoard'
import Layout from './components/Layout'
import WorkspacePage from './pages/WorkSpacePage'
import ProjectPage from './pages/ProjectPage'
import CreateWorkspacePage from './pages/CreateWorkspacePage'
import CreateProjectPage from './pages/CreateProjectPage'
import CreateIssuePage from './pages/CreateIssuePage'
import IssuesPage from './pages/IssuesPage'
import AddMemberPage from './pages/AddMemberPage'
const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const token = localStorage.getItem('token')
  if (!token) return <Navigate to="/login" replace />
  return <>{children}</>
}

function App() {
  return (
    <WorkspaceProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/dashboard" element={<ProtectedRoute><Layout><DashboardPage /></Layout></ProtectedRoute>} />
          <Route path="/issues" element={<ProtectedRoute><Layout><IssuesPage /></Layout></ProtectedRoute>} />

          {/* Önce spesifik route'lar */}
          <Route path="/workspaces/new" element={<ProtectedRoute><Layout><CreateWorkspacePage /></Layout></ProtectedRoute>} />
          <Route path="/workspaces/:workspaceId/add-member" element={<ProtectedRoute><Layout><AddMemberPage /></Layout></ProtectedRoute>} />
          <Route path="/workspaces/:workspaceId/projects/new" element={<ProtectedRoute><Layout><CreateProjectPage /></Layout></ProtectedRoute>} />
          <Route path="/workspaces/:workspaceId/projects/:projectId/issues/new" element={<ProtectedRoute><Layout><CreateIssuePage /></Layout></ProtectedRoute>} />
          <Route path="/workspaces/:workspaceId/projects/:projectId" element={<ProtectedRoute><Layout><ProjectPage /></Layout></ProtectedRoute>} />

          {/* En sonda parametreli route'lar */}
          <Route path="/workspaces/:workspaceId" element={<ProtectedRoute><Layout><WorkspacePage /></Layout></ProtectedRoute>} />

          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
      </BrowserRouter>
    </WorkspaceProvider>
  )
}

export default App