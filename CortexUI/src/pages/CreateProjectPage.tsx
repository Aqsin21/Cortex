import { useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { apiFetch } from '../services/api'

function CreateProjectPage() {
  const navigate = useNavigate()
  const { workspaceId } = useParams()
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [endDate, setEndDate] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  async function handleCreate() {
    if (!name.trim() || !description.trim() || !endDate) {
      setError('All fields are required.')
      return
    }

    setLoading(true)
    setError('')

    try {
      const response = await apiFetch('/projects', {
        method: 'POST',
        body: JSON.stringify({
          workspaceId,
          name,
          description,
          endDate: new Date(endDate).toISOString()
        })
      })

      if (!response.ok) {
        const data = await response.json()
        setError(data.error || 'Failed to create project.')
        return
      }

      const data = await response.json()
      navigate(`/workspaces/${workspaceId}/projects/${data.projectId}`)

    } catch {
      setError('Something went wrong.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-2xl mx-auto">
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-gray-800">Create Project</h2>
        <p className="text-sm text-gray-500 mt-1">Add a new project to your workspace.</p>
      </div>

      <div className="bg-white rounded-xl border border-gray-200 p-6 mb-4">
        <h3 className="text-sm font-semibold text-gray-700 mb-4">Project Details</h3>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Project Name</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="e.g. Cortex Backend API"
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="What is this project about?"
            rows={3}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 resize-none"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">End Date</label>
          <input
            type="date"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>
      </div>

      {error && <p className="text-red-500 text-sm mb-4">{error}</p>}

      <div className="flex gap-3">
        <button
          onClick={() => navigate(`/workspaces/${workspaceId}`)}
          className="flex-1 py-2.5 border border-gray-300 text-gray-700 text-sm font-medium rounded-lg hover:bg-gray-50 transition-colors"
        >
          Cancel
        </button>
        <button
          onClick={handleCreate}
          disabled={loading}
          className="flex-1 py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-lg transition-colors disabled:opacity-50"
        >
          {loading ? 'Creating...' : 'Create Project'}
        </button>
      </div>
    </div>
  )
}

export default CreateProjectPage