import { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { apiFetch } from '../services/api'

function AddMemberPage() {
  const navigate = useNavigate()
  const { workspaceId } = useParams()
  const [searchEmail, setSearchEmail] = useState('')
  const [searchResult, setSearchResult] = useState<{ userId: string; fullName: string } | null>(null)
  const [searchError, setSearchError] = useState('')
  const [selectedRole, setSelectedRole] = useState('')
  const [roles, setRoles] = useState<string[]>([])
  const [loading, setLoading] = useState(false)
  const [success, setSuccess] = useState('')
  const [error, setError] = useState('')

  useEffect(() => {
    async function fetchRoles() {
      try {
        const response = await apiFetch('/roles')
        const data = await response.json()
        const filtered = data
          .filter((r: { name: string }) => r.name !== 'TeamLead')
          .map((r: { name: string }) => r.name)
        setRoles(filtered)
        if (filtered.length > 0) setSelectedRole(filtered[0])
      } catch {
        console.error('Failed to fetch roles')
      }
    }
    fetchRoles()
  }, [])

  async function handleSearch() {
    setSearchError('')
    setSearchResult(null)
    setSuccess('')

    if (!searchEmail.trim()) return

    try {
      const response = await apiFetch(`/auth/users/search?email=${searchEmail}`)
      if (response.status === 404) {
        setSearchError('User not found.')
        return
      }
      const data = await response.json()
      setSearchResult(data)
    } catch {
      setSearchError('Search failed.')
    }
  }

  async function handleAddMember() {
    if (!searchResult) return
    setLoading(true)
    setError('')
    setSuccess('')

    try {
      const response = await apiFetch('/WorkSpaces/members', {
        method: 'POST',
        body: JSON.stringify({
          workspaceId,
          targetUserId: searchResult.userId,
          targetFullName: searchResult.fullName,
          roleName: selectedRole
        })
      })

      if (!response.ok) {
        const data = await response.json()
        setError(data.error || 'Failed to add member.')
        return
      }

      setSuccess(`${searchResult.fullName} added successfully as ${selectedRole}!`)
      setSearchEmail('')
      setSearchResult(null)
    } catch {
      setError('Something went wrong.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-2xl mx-auto">
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-gray-800">Add Member</h2>
        <p className="text-sm text-gray-500 mt-1">Search for a user by email and add them to the workspace.</p>
      </div>

      <div className="bg-white rounded-xl border border-gray-200 p-6 mb-4">
        {/* Search */}
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Search by Email</label>
          <div className="flex gap-2">
            <input
              type="email"
              value={searchEmail}
              onChange={(e) => setSearchEmail(e.target.value)}
              placeholder="e.g. developer@example.com"
              className="flex-1 border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
              onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
            />
            <button
              onClick={handleSearch}
              className="px-4 py-2 bg-gray-100 hover:bg-gray-200 text-gray-700 text-sm font-medium rounded-lg transition-colors"
            >
              Search
            </button>
          </div>
          {searchError && <p className="text-red-500 text-xs mt-2">{searchError}</p>}
        </div>

        {/* Search Result */}
        {searchResult && (
          <div className="bg-indigo-50 border border-indigo-100 rounded-lg px-4 py-3 mb-4">
            <div className="flex items-center justify-between mb-3">
              <div className="flex items-center gap-3">
                <div className="w-9 h-9 bg-indigo-200 rounded-full flex items-center justify-center">
                  <span className="text-indigo-700 font-semibold text-sm">
                    {searchResult.fullName.charAt(0).toUpperCase()}
                  </span>
                </div>
                <div>
                  <p className="text-sm font-medium text-gray-800">{searchResult.fullName}</p>
                  <p className="text-xs text-gray-500">{searchEmail}</p>
                </div>
              </div>
            </div>

            <div className="flex items-center gap-2">
              <select
                value={selectedRole}
                onChange={(e) => setSelectedRole(e.target.value)}
                className="flex-1 text-sm border border-gray-200 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              >
                {roles.map(role => (
                  <option key={role} value={role}>{role}</option>
                ))}
              </select>
              <button
                onClick={handleAddMember}
                disabled={loading}
                className="px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-lg transition-colors disabled:opacity-50"
              >
                {loading ? 'Adding...' : 'Add'}
              </button>
            </div>
          </div>
        )}

        {success && (
          <div className="px-4 py-3 bg-green-50 border border-green-200 rounded-lg text-sm text-green-600">
            ✓ {success}
          </div>
        )}

        {error && (
          <div className="px-4 py-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-600">
            {error}
          </div>
        )}
      </div>

      <button
        onClick={() => navigate(`/workspaces/${workspaceId}`)}
        className="w-full py-2.5 border border-gray-300 text-gray-700 text-sm font-medium rounded-lg hover:bg-gray-50 transition-colors"
      >
        Back to Workspace
      </button>
    </div>
  )
}

export default AddMemberPage