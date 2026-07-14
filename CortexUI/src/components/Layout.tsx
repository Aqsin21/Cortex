import { useState, useEffect } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { apiFetch } from '../services/api'

interface LayoutProps {
  children: React.ReactNode
}

function Layout({ children }: LayoutProps) {
  const navigate = useNavigate()
  const location = useLocation()

  function handleLogout() {
    localStorage.removeItem('token')
    navigate('/login')
  }

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar */}
      <aside className="w-60 bg-white flex flex-col border-r border-gray-200 py-4 px-3">

        {/* Logo */}
        <div className="flex items-center gap-2 px-2 mb-6">
          <div className="w-7 h-7 bg-indigo-600 rounded-lg flex items-center justify-center">
            <span className="text-white text-xs font-bold">C</span>
          </div>
          <span className="font-semibold text-gray-800 text-sm">Cortex</span>
        </div>

        {/* Main Nav */}
        <nav className="space-y-1 mb-6">
          {[
            { label: 'Home', path: '/dashboard', icon: (
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
              </svg>
            )},
            { label: 'My Issues', path: '/issues', icon: (
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
              </svg>
            )},
            { label: 'Search', path: '/search', icon: (
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            )},
          ].map(item => (
            <button
              key={item.path}
              onClick={() => navigate(item.path)}
              className={`w-full flex items-center gap-3 px-3 py-2 rounded-lg text-sm transition-colors
                ${location.pathname === item.path
                  ? 'bg-indigo-50 text-indigo-600 font-medium'
                  : 'text-gray-600 hover:bg-gray-100'
                }`}
            >
              {item.icon}
              {item.label}
            </button>
          ))}
        </nav>

        {/* Workspaces */}
        <div className="mb-4">
          <div className="flex items-center justify-between px-2 mb-2">
            <span className="text-xs font-semibold text-gray-400 uppercase tracking-wider">Workspaces</span>
            <button
              onClick={() => navigate('/workspaces/new')}
              className="text-gray-400 hover:text-indigo-600 transition-colors"
            >
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
              </svg>
            </button>
          </div>
          <WorkspaceList />
        </div>

        {/* Bottom */}
        <div className="mt-auto space-y-1">
          <button
            onClick={() => navigate('/members')}
            className="w-full flex items-center gap-3 px-3 py-2 rounded-lg text-sm text-gray-600 hover:bg-gray-100 transition-colors"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
            </svg>
            Add Member
          </button>

          <button
            onClick={handleLogout}
            className="w-full flex items-center gap-3 px-3 py-2 rounded-lg text-sm text-red-500 hover:bg-red-50 transition-colors"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
            </svg>
            Logout
          </button>
        </div>
      </aside>

      {/* Main */}
      <div className="flex-1 flex flex-col overflow-hidden">
        {/* Header */}
        <header className="bg-white border-b border-gray-200 px-6 py-3 flex items-center justify-between">
          <h1 className="text-base font-semibold text-gray-800">
            {location.pathname === '/dashboard' ? 'Home' :
             location.pathname === '/issues' ? 'My Issues' : 'Cortex'}
          </h1>
          <div className="flex items-center gap-3">
            <button className="w-8 h-8 bg-indigo-600 rounded-full flex items-center justify-center text-white text-sm font-bold">
              U
            </button>
          </div>
        </header>

        {/* Content */}
        <main className="flex-1 overflow-auto p-6">
          {children}
        </main>
      </div>
    </div>
  )
}

// Workspace listesini ayrı component olarak tutuyoruz
function WorkspaceList() {
  const navigate = useNavigate()
  const [workspaces, setWorkspaces] = useState<{ id: string; name: string; roleName: string }[]>([])

  const colors = [
    'bg-red-500', 'bg-blue-500', 'bg-green-500',
    'bg-yellow-500', 'bg-purple-500', 'bg-pink-500'
  ]

  useEffect(() => {
    async function fetchWorkspaces() {
      try {
        const response = await apiFetch('/WorkSpaces/GetAll')
        const data = await response.json()
        setWorkspaces(data)
      } catch (err) {
        console.error('Failed to fetch workspaces:', err)
      }
    }
    fetchWorkspaces()
  }, [])

  return (
    <div className="space-y-1">
      {workspaces.length === 0 && (
        <p className="text-xs text-gray-400 px-3 py-1">No workspaces yet</p>
      )}
      {workspaces.map((ws, i) => (
        <button
          key={ws.id}
          onClick={() => navigate(`/workspaces/${ws.id}`)}
          className="w-full flex items-center gap-3 px-3 py-2 rounded-lg text-sm text-gray-600 hover:bg-gray-100 transition-colors"
        >
          <div className={`w-3 h-3 rounded-sm ${colors[i % colors.length]}`} />
          <span className="truncate">{ws.name}</span>
        </button>
      ))}
    </div>
  )
}

export default Layout