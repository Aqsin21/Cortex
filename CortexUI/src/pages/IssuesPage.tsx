import { useState, useEffect } from 'react'
import { apiFetch } from '../services/api'

interface Issue {
  id: string
  title: string
  description: string
  status: number
  priority: number
  dueDate: string | null
  workspaceId: string
}

const STATUS_LABELS: Record<number, { label: string; color: string }> = {
  1: { label: 'To Do', color: 'bg-gray-100 text-gray-600' },
  2: { label: 'In Progress', color: 'bg-blue-100 text-blue-600' },
  3: { label: 'In Review', color: 'bg-yellow-100 text-yellow-600' },
  4: { label: 'Done', color: 'bg-green-100 text-green-600' },
}

const PRIORITY_LABELS: Record<number, { label: string; color: string }> = {
  1: { label: 'Low', color: 'text-gray-500' },
  2: { label: 'Medium', color: 'text-blue-500' },
  3: { label: 'High', color: 'text-orange-500' },
  4: { label: 'Critical', color: 'text-red-500' },
}

function IssuesPage() {
  const [issues, setIssues] = useState<Issue[]>([])
  const [loading, setLoading] = useState(true)
  const [selectedIssue, setSelectedIssue] = useState<Issue | null>(null)
  const [statusError, setStatusError] = useState('')

  useEffect(() => {
    async function fetchAllIssues() {
      try {
        const wsResponse = await apiFetch('/WorkSpaces/GetAll')
        const workspaces = await wsResponse.json()

        const allIssues: Issue[] = []

        for (const ws of workspaces) {
          const projResponse = await apiFetch(`/projects?workspaceId=${ws.id}`)
          const projects = await projResponse.json()

          for (const proj of projects) {
            const issueResponse = await apiFetch(
              `/issues?projectId=${proj.id}&workspaceId=${ws.id}`
            )
            const data = await issueResponse.json()
            allIssues.push(...data.map((i: Issue) => ({ ...i, workspaceId: ws.id })))
          }
        }

        setIssues(allIssues)
      } catch (err) {
        console.error('Failed to fetch issues:', err)
      } finally {
        setLoading(false)
      }
    }
    fetchAllIssues()
  }, [])

  async function handleStatusChange(issueId: string, workspaceId: string, newStatus: number) {
    setStatusError('')
    try {
      const response = await apiFetch(`/issues/${issueId}/status`, {
        method: 'PATCH',
        body: JSON.stringify({ workspaceId, newStatus })
      })

      if (!response.ok) {
        const data = await response.json()
        setStatusError(data.error || 'You are not authorized to make this status change.')
        return
      }

      setIssues(prev =>
        prev.map(i => i.id === issueId ? { ...i, status: newStatus } : i)
      )
      setSelectedIssue(prev =>
        prev?.id === issueId ? { ...prev, status: newStatus } : prev
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
    <div className="relative">
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-gray-800">My Issues</h2>
        <p className="text-sm text-gray-500 mt-1">All issues assigned to you across workspaces.</p>
      </div>

      {issues.length === 0 ? (
        <div className="text-center py-24">
          <div className="w-16 h-16 bg-indigo-50 rounded-2xl flex items-center justify-center mx-auto mb-4">
            <svg className="w-8 h-8 text-indigo-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg>
          </div>
          <h3 className="text-gray-700 font-semibold mb-1">No issues assigned</h3>
          <p className="text-sm text-gray-400">You don't have any issues assigned to you yet.</p>
        </div>
      ) : (
        <div className="space-y-3">
          {issues.map(issue => (
            <div
              key={issue.id}
              onClick={() => { setSelectedIssue(issue); setStatusError('') }}
              className="bg-white rounded-xl border border-gray-200 p-4 hover:shadow-md transition-shadow cursor-pointer hover:border-indigo-200"
            >
              <div className="flex items-center justify-between">
                <h4 className="text-sm font-medium text-gray-800">{issue.title}</h4>
                <span className={`text-xs font-semibold px-2 py-1 rounded-full ${STATUS_LABELS[issue.status]?.color}`}>
                  {STATUS_LABELS[issue.status]?.label}
                </span>
              </div>
              <p className="text-xs text-gray-400 mt-1 line-clamp-1">{issue.description}</p>
              <div className="flex items-center gap-3 mt-3">
                <span className={`text-xs font-medium ${PRIORITY_LABELS[issue.priority]?.color}`}>
                  ● {PRIORITY_LABELS[issue.priority]?.label}
                </span>
                {issue.dueDate && (
                  <span className="text-xs text-gray-400">
                    Due {new Date(issue.dueDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })}
                  </span>
                )}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Drawer overlay */}
      {selectedIssue && (
        <>
          {/* Backdrop */}
          <div
            className="fixed inset-0 bg-black/20 z-40"
            onClick={() => setSelectedIssue(null)}
          />

          {/* Drawer */}
          <div className="fixed right-0 top-0 h-full w-full max-w-md bg-white shadow-2xl z-50 flex flex-col">
            {/* Drawer Header */}
            <div className="flex items-center justify-between px-6 py-4 border-b border-gray-200">
              <h3 className="font-semibold text-gray-800">Issue Detail</h3>
              <button
                onClick={() => setSelectedIssue(null)}
                className="text-gray-400 hover:text-gray-600 transition-colors"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>

            {/* Drawer Content */}
            <div className="flex-1 overflow-auto p-6">
              <h2 className="text-lg font-semibold text-gray-800 mb-2">
                {selectedIssue.title}
              </h2>

              <p className="text-sm text-gray-500 mb-6">
                {selectedIssue.description}
              </p>

              <div className="space-y-4">
                {/* Priority */}
                <div className="flex items-center justify-between py-3 border-b border-gray-100">
                  <span className="text-sm text-gray-500">Priority</span>
                  <span className={`text-sm font-medium ${PRIORITY_LABELS[selectedIssue.priority]?.color}`}>
                    ● {PRIORITY_LABELS[selectedIssue.priority]?.label}
                  </span>
                </div>

                {/* Due Date */}
                <div className="flex items-center justify-between py-3 border-b border-gray-100">
                  <span className="text-sm text-gray-500">Due Date</span>
                  <span className="text-sm text-gray-700">
                    {selectedIssue.dueDate
                      ? new Date(selectedIssue.dueDate).toLocaleDateString('en-US', { month: 'long', day: 'numeric', year: 'numeric' })
                      : 'No due date'}
                  </span>
                </div>

                {/* Status */}
                <div className="flex items-center justify-between py-3 border-b border-gray-100">
                  <span className="text-sm text-gray-500">Status</span>
                  <select
                    value={selectedIssue.status}
                    onChange={(e) => handleStatusChange(selectedIssue.id, selectedIssue.workspaceId, Number(e.target.value))}
                    className="text-sm border border-gray-200 rounded-lg px-3 py-1.5 text-gray-600 focus:outline-none focus:ring-2 focus:ring-indigo-500 bg-gray-50"
                  >
                    <option value={1}>To Do</option>
                    <option value={2}>In Progress</option>
                    <option value={3}>In Review</option>
                    <option value={4}>Done</option>
                  </select>
                </div>
              </div>

              {statusError && (
                <div className="mt-4 px-4 py-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-600">
                  {statusError}
                </div>
              )}
            </div>
          </div>
        </>
      )}
    </div>
  )
}

export default IssuesPage