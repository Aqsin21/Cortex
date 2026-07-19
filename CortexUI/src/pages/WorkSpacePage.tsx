import { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { apiFetch } from '../services/api'

interface Project {
  id: string
  name: string
  description: string
  endDate: string
}

interface Member {
  workSpaceMemberId: string
  fullName: string
  roleName: string
}

function WorkspacePage() {
  const { workspaceId } = useParams()
  const navigate = useNavigate()
  const [activeTab, setActiveTab] = useState<'projects' | 'members'>('projects')
  const [projects, setProjects] = useState<Project[]>([])
  const [members, setMembers] = useState<Member[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    fetchProjects()
    fetchMembers()
  }, [workspaceId])

  async function fetchProjects() {
    try {
      const response = await apiFetch(`/projects?workspaceId=${workspaceId}`)
      const data = await response.json()
      setProjects(data)
    } catch (err) {
      console.error('Failed to fetch projects:', err)
    } finally {
      setLoading(false)
    }
  }

  async function fetchMembers() {
    try {
      const response = await apiFetch(`/WorkSpaces/GetMembers?workspaceId=${workspaceId}`)
      const data = await response.json()
      setMembers(data)
    } catch (err) {
      console.error('Failed to fetch members:', err)
    }
  }

  async function handleRemoveMember(memberId: string) {
    if (!confirm('Are you sure you want to remove this member?')) return
    setError('')

    try {
      const response = await apiFetch(
        `/WorkSpaces/members/${memberId}?workspaceId=${workspaceId}`,
        { method: 'DELETE' }
      )

      if (!response.ok) {
        const data = await response.json()
        setError(data.error || 'Failed to remove member.')
        return
      }

      setMembers(prev => prev.filter(m => m.workSpaceMemberId !== memberId))
    } catch (err) {
      console.error('Failed to remove member:', err)
    }
  }
  async function handleDeleteProject(projectId: string) {
  if (!confirm('Are you sure you want to delete this project?')) return
  setError('')

  try {
    const response = await apiFetch(
      `/projects/${projectId}?workspaceId=${workspaceId}`,
      { method: 'DELETE' }
    )

    if (!response.ok) {
      const data = await response.json()
      setError(data.error || 'Failed to delete project.')
      return
    }

    setProjects(prev => prev.filter(p => p.id !== projectId))
  } catch (err) {
    console.error('Failed to delete project:', err)
  }
}

  const roleColors: Record<string, string> = {
    'TeamLead': 'bg-indigo-100 text-indigo-700',
    'Backend Developer': 'bg-blue-100 text-blue-700',
    'Frontend Developer': 'bg-cyan-100 text-cyan-700',
    'Full Stack Developer': 'bg-teal-100 text-teal-700',
    'QA': 'bg-green-100 text-green-700',
    'DevOps': 'bg-orange-100 text-orange-700',
    'UI/UX Designer': 'bg-pink-100 text-pink-700',
    'BA': 'bg-purple-100 text-purple-700',
  }

  if (loading) return (
    <div className="flex items-center justify-center h-full">
      <div className="text-gray-400">Loading...</div>
    </div>
  )

  return (
    <div>
      {/* Tabs */}
      <div className="flex items-center justify-between mb-6">
        <div className="flex gap-1 bg-gray-100 p-1 rounded-lg">
          <button
            onClick={() => setActiveTab('projects')}
            className={`px-4 py-2 text-sm font-medium rounded-md transition-colors ${
              activeTab === 'projects'
                ? 'bg-white text-gray-800 shadow-sm'
                : 'text-gray-500 hover:text-gray-700'
            }`}
          >
            Projects
          </button>
          <button
            onClick={() => setActiveTab('members')}
            className={`px-4 py-2 text-sm font-medium rounded-md transition-colors ${
              activeTab === 'members'
                ? 'bg-white text-gray-800 shadow-sm'
                : 'text-gray-500 hover:text-gray-700'
            }`}
          >
            Members
            <span className="ml-2 text-xs bg-gray-200 text-gray-600 px-1.5 py-0.5 rounded-full">
              {members.length}
            </span>
          </button> 
        </div>

        {activeTab === 'projects' && (
          <button
           onClick={() => navigate(`/workspaces/${workspaceId}/projects/new`)}
            className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium px-4 py-2 rounded-lg transition-colors"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
            </svg>
            New Project
          </button>
        )}

        {activeTab === 'members' && (
          <button
            onClick={() => navigate(`/workspaces/${workspaceId}/add-member`)}
            className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium px-4 py-2 rounded-lg transition-colors"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
            </svg>
            Add Member
          </button>
        )}
      </div>

      {error && (
        <div className="mb-4 px-4 py-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-600">
          {error}
        </div>
      )}

      {/* Projects Tab */}
      {activeTab === 'projects' && (
        projects.length === 0 ? (
          <div className="text-center py-16 text-gray-400">
            <svg className="w-12 h-12 mx-auto mb-3 text-gray-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
            </svg>
            <p className="text-sm">No projects yet</p>
            <p className="text-xs mt-1">Create your first project to get started</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {projects.map(project => (
              <div
                key={project.id}
                onClick={() => navigate(`/workspaces/${workspaceId}/projects/${project.id}`)}
                className="bg-white rounded-xl border border-gray-200 p-5 hover:shadow-md transition-shadow cursor-pointer"
              >
                <div className="flex items-start justify-between mb-3">
  <div className="w-9 h-9 bg-indigo-100 rounded-lg flex items-center justify-center">
    <svg className="w-5 h-5 text-indigo-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
    </svg>
  </div>
  <button
    onClick={(e) => {
      e.stopPropagation()  // kartın onClick'ini tetiklememesi için
      handleDeleteProject(project.id)
    }}
    className="text-gray-300 hover:text-red-500 transition-colors"
  >
    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
    </svg>
  </button>
</div>
                <h3 className="font-semibold text-gray-800 mb-1">{project.name}</h3>
                <p className="text-sm text-gray-500 mb-4 line-clamp-2">{project.description}</p>
                <div className="flex items-center gap-1 text-xs text-gray-400">
                  <svg className="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                  <span>Due {new Date(project.endDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' })}</span>
                </div>
              </div>
            ))}
          </div>
        )
      )}

      {/* Members Tab */}
      {activeTab === 'members' && (
        members.length === 0 ? (
          <div className="text-center py-16 text-gray-400">
            <p className="text-sm">No members yet</p>
          </div>
        ) : (
          <div className="space-y-3">
            {members.map(member => (
              <div
                key={member.workSpaceMemberId}
                className="bg-white rounded-xl border border-gray-200 px-5 py-4 flex items-center justify-between"
              >
                <div className="flex items-center gap-3">
                  <div className="w-9 h-9 bg-indigo-100 rounded-full flex items-center justify-center">
                    <span className="text-indigo-600 font-semibold text-sm">
                      {member.fullName.charAt(0).toUpperCase()}
                    </span>
                  </div>
                  <div>
                    <p className="text-sm font-medium text-gray-800">{member.fullName}</p>
                    <span className={`text-xs font-medium px-2 py-0.5 rounded-full ${roleColors[member.roleName] || 'bg-gray-100 text-gray-600'}`}>
                      {member.roleName}
                    </span>
                  </div>
                </div>

                {member.roleName !== 'TeamLead' && (
                  <button
                    onClick={() => handleRemoveMember(member.workSpaceMemberId)}
                    className="text-gray-300 hover:text-red-500 transition-colors"
                  >
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                    </svg>
                  </button>
                )}
              </div>
            ))}
          </div>
        )
      )}
    </div>
  )
}

export default WorkspacePage