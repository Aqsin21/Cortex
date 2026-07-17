import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { apiFetch } from '../services/api'

interface Workspace {
  id: string
  name: string
  roleName: string
}

function DashboardPage() {
  const navigate = useNavigate()
  const [workspaces, setWorkspaces] = useState<Workspace[]>([])
  const [loading, setLoading] = useState(true)

  const colors = [
    'bg-indigo-500', 'bg-blue-500', 'bg-green-500',
    'bg-yellow-500', 'bg-purple-500', 'bg-pink-500',
    'bg-red-500', 'bg-orange-500'
  ]

  useEffect(() => {
    async function fetchWorkspaces() {
      try {
        const response = await apiFetch('/WorkSpaces/GetAll')
        const data = await response.json()
        setWorkspaces(data)
      } catch {
        console.error('Failed to fetch workspaces')
      } finally {
        setLoading(false)
      }
    }
    fetchWorkspaces()
  }, [])

  if (loading) return (
    <div className="flex items-center justify-center h-full">
      <div className="text-gray-400">Loading...</div>
    </div>
  )

  return (
    <div>
      {/* Header */}
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-2xl font-bold text-gray-800">Home</h1>
          <p className="text-sm text-gray-500 mt-1">Welcome back! Here are your workspaces.</p>
        </div>
        <button
          onClick={() => navigate('/workspaces/new')}
          className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium px-4 py-2 rounded-lg transition-colors"
        >
          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
          </svg>
          New Workspace
        </button>
      </div>

      {/* Workspaces */}
      {workspaces.length === 0 ? (
        <div className="text-center py-24">
          <div className="w-16 h-16 bg-indigo-50 rounded-2xl flex items-center justify-center mx-auto mb-4">
            <svg className="w-8 h-8 text-indigo-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
            </svg>
          </div>
          <h3 className="text-gray-700 font-semibold mb-1">No workspaces yet</h3>
          <p className="text-sm text-gray-400 mb-4">Create your first workspace to get started.</p>
          <button
            onClick={() => navigate('/workspaces/new')}
            className="bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium px-4 py-2 rounded-lg transition-colors"
          >
            Create Workspace
          </button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {workspaces.map((ws, i) => (
            <div
              key={ws.id}
              onClick={() => navigate(`/workspaces/${ws.id}`)}
              className="bg-white rounded-xl border border-gray-200 p-5 hover:shadow-md transition-shadow cursor-pointer"
            >
              <div className="flex items-center gap-3 mb-4">
                <div className={`w-10 h-10 ${colors[i % colors.length]} rounded-xl flex items-center justify-center`}>
                  <span className="text-white font-bold text-sm">
                    {ws.name.charAt(0).toUpperCase()}
                  </span>
                </div>
                <div>
                  <h3 className="font-semibold text-gray-800 text-sm">{ws.name}</h3>
                  <span className="text-xs text-gray-400">{ws.roleName}</span>
                </div>
              </div>

              <div className="flex items-center justify-between">
                <span className="text-xs text-indigo-600 font-medium bg-indigo-50 px-2 py-1 rounded-full">
                  {ws.roleName === 'TeamLead' ? '👑 Team Lead' : `👤 ${ws.roleName}`}
                </span>
                <svg className="w-4 h-4 text-gray-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
                </svg>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default DashboardPage