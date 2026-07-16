import { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { apiFetch } from '../services/api'

interface Issue {
  id: string
  title: string
  description: string
  status: number
  priority: number
  dueDate: string | null
  assigneeId: string | null
}

const STATUS_COLUMNS = [
  { id: 1, label: 'To Do', color: 'bg-gray-100 text-gray-600' },
  { id: 2, label: 'In Progress', color: 'bg-blue-100 text-blue-600' },
  { id: 3, label: 'In Review', color: 'bg-yellow-100 text-yellow-600' },
  { id: 4, label: 'Done', color: 'bg-green-100 text-green-600' },
]

const PRIORITY_LABELS: Record<number, { label: string; color: string }> = {
  1: { label: 'Low', color: 'text-gray-500' },
  2: { label: 'Medium', color: 'text-blue-500' },
  3: { label: 'High', color: 'text-orange-500' },
  4: { label: 'Critical', color: 'text-red-500' },
}

function ProjectPage() {
  const { workspaceId, projectId } = useParams()
  const navigate = useNavigate()
  const [issues, setIssues] = useState<Issue[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    fetchIssues()
  }, [projectId, workspaceId])

  async function fetchIssues() {
    try {
      const response = await apiFetch(
        `/issues?projectId=${projectId}&workspaceId=${workspaceId}`
      )
      const data = await response.json()
      setIssues(data)
    } catch (err) {
      console.error('Failed to fetch issues:', err)
    } finally {
      setLoading(false)
    }
  }

  async function handleStatusChange(issueId: string, newStatus: number) {
    try {
      await apiFetch(`/issues/${issueId}/status`, {
        method: 'PATCH',
        body: JSON.stringify({
          workspaceId: workspaceId,
          newStatus: newStatus
        })
      })
      setIssues(prev =>
        prev.map(i => i.id === issueId ? { ...i, status: newStatus } : i)
      )
    } catch (err) {
      console.error('Failed to update status:', err)
    }
  }

  if (loading) return (
    <div className="flex items-center justify-center h-full">
      <div className="text-gray-400">Loading...</div>
    </div>
  )

  return (
    <div className="h-full flex flex-col">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-xl font-semibold text-gray-800">Issues</h2>
        <button
          onClick={() => navigate(`/workspaces/${workspaceId}/projects/${projectId}/issues/new`)}
          className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium px-4 py-2 rounded-lg transition-colors"
        >
          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
          </svg>
          New Issue
        </button>
      </div>

      <div className="flex gap-4 overflow-x-auto pb-4 flex-1">
        {STATUS_COLUMNS.map(column => {
          const columnIssues = issues.filter(i => i.status === column.id)

          return (
            <div key={column.id} className="flex-shrink-0 w-72">
              <div className="flex items-center justify-between mb-3">
                <div className="flex items-center gap-2">
                  <span className={`text-xs font-semibold px-2 py-1 rounded-full ${column.color}`}>
                    {column.label}
                  </span>
                  <span className="text-xs text-gray-400 font-medium">{columnIssues.length}</span>
                </div>
              </div>

              <div className="space-y-3">
                {columnIssues.map(issue => (
                  <div
                    key={issue.id}
                    className="bg-white rounded-xl border border-gray-200 p-4 hover:shadow-md transition-shadow"
                  >
                    <h4 className="text-sm font-medium text-gray-800 mb-2 line-clamp-2">
                      {issue.title}
                    </h4>

                    <p className="text-xs text-gray-400 mb-3 line-clamp-2">
                      {issue.description}
                    </p>

                    <div className="flex items-center justify-between">
                      <span className={`text-xs font-medium ${PRIORITY_LABELS[issue.priority]?.color}`}>
                        ● {PRIORITY_LABELS[issue.priority]?.label}
                      </span>

                      {issue.dueDate && (
                        <div className="flex items-center gap-1 text-xs text-gray-400">
                          <svg className="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                          </svg>
                          {new Date(issue.dueDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })}
                        </div>
                      )}
                    </div>

                    <div className="mt-3 pt-3 border-t border-gray-100">
                      <select
                        value={issue.status}
                        onChange={(e) => handleStatusChange(issue.id, Number(e.target.value))}
                        className="w-full text-xs border border-gray-200 rounded-lg px-2 py-1.5 text-gray-600 focus:outline-none focus:ring-2 focus:ring-indigo-500 bg-gray-50"
                      >
                        <option value={1}>To Do</option>
                        <option value={2}>In Progress</option>
                        <option value={3}>In Review</option>
                        <option value={4}>Done</option>
                      </select>
                    </div>
                  </div>
                ))}

                {column.id === 1 && (
                  <button
                    onClick={() => navigate(`/workspaces/${workspaceId}/projects/${projectId}/issues/new`)}
                    className="w-full flex items-center justify-center gap-2 py-2 rounded-lg border border-dashed border-gray-200 text-sm text-gray-400 hover:border-indigo-300 hover:text-indigo-500 transition-colors"
                  >
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                    </svg>
                    Add issue
                  </button>
                )}
              </div>
            </div>
          )
        })}
      </div>
    </div>
  )
}

export default ProjectPage