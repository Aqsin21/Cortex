import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import DashboardPage from './pages/DashBoard'
import Layout from './components/Layout'
import WorkspacePage from './pages/WorkSpacePage'
import ProjectPage from './pages/ProjectPage'

const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const token = localStorage.getItem('token')
  if (!token) return <Navigate to="/login" replace />
  return <>{children}</>
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Layout>
                <DashboardPage />
              </Layout>
            </ProtectedRoute>
          }
        />

        <Route
          path="/workspaces/:workspaceId"
          element={
            <ProtectedRoute>
              <Layout>
                <WorkspacePage />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
           path="/workspaces/:workspaceId/projects/:projectId"
            element={
           <ProtectedRoute>
            <Layout>
             <ProjectPage />
              </Layout>
              </ProtectedRoute>
  }
/>

        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App