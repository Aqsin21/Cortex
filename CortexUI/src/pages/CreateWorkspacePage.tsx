import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { apiFetch } from '../services/api'

interface MemberToAdd {
  userId: string
  fullName: string
  email: string
  roleName: string
}

function CreateWorkspacePage() {
  const navigate = useNavigate()
  const [name, setName] = useState('')
  const [ownerFullName, setOwnerFullName] = useState('')
  const [searchEmail, setSearchEmail] = useState('')
  const [searchResult, setSearchResult] = useState<{ userId: string; fullName: string } | null>(null)
  const [searchError, setSearchError] = useState('')
  const [selectedRole, setSelectedRole] = useState('Backend Developer')
  const [members, setMembers] = useState<MemberToAdd[]>([])
  const [loading, setLoading] = useState(false) 
  const [error, setError] = useState('')

 const [roles, setRoles] = useState<string[]>([])

    useEffect(() => {
  async function fetchRoles() {
    try {
      const response = await apiFetch('/roles')
      const data = await response.json()
      setRoles(data.map((r: { name: string }) => r.name))
    } catch {
      console.error('Failed to fetch roles')
    }
  }
  fetchRoles()
}, [])

  async function handleSearch() {
    setSearchError('')
    setSearchResult(null)

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

  function handleAddMember() {
    if (!searchResult) return

    const alreadyAdded = members.find(m => m.userId === searchResult.userId)
    if (alreadyAdded) {
      setSearchError('This user is already added.')
      return
    }

    setMembers(prev => [...prev, {
      userId: searchResult.userId,
      fullName: searchResult.fullName,
      email: searchEmail,
      roleName: selectedRole
    }])

    setSearchEmail('')
    setSearchResult(null)
    setSelectedRole('Backend Developer')
  }

  function handleRemoveMember(userId: string) {
    setMembers(prev => prev.filter(m => m.userId !== userId))
  }

  async function handleCreate() {
    if (!name.trim() || !ownerFullName.trim()) {
      setError('Workspace name and your full name are required.')
      return
    }

    setLoading(true)
    setError('')

    try {
      // 1. Workspace oluştur
        const wsResponse = await apiFetch('/WorkSpaces/CreateWorkSpace', {
       method: 'POST',
       body: JSON.stringify({ name, ownerFullName })
      })

      if (!wsResponse.ok) {
        setError('Failed to create workspace.')
        return
      }

      const wsData = await wsResponse.json()
      const workspaceId = wsData.workspaceId

      // 2. Üyeleri ekle
      for (const member of members) {
        await apiFetch('/workspaces/members', {
          method: 'POST',
          body: JSON.stringify({
            workspaceId,
            targetUserId: member.userId,
            targetFullName: member.fullName,
            roleName: member.roleName
          })
        })
      }

      navigate(`/workspaces/${workspaceId}`)

    } catch {
      setError('Something went wrong.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-2xl mx-auto">
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-gray-800">Create Workspace</h2>
        <p className="text-sm text-gray-500 mt-1">Set up your team's workspace and invite members.</p>
      </div>

      {/* Workspace Info */}
      <div className="bg-white rounded-xl border border-gray-200 p-6 mb-4">
        <h3 className="text-sm font-semibold text-gray-700 mb-4">Workspace Details</h3>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Workspace Name</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="e.g. Backend Team"
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Your Full Name</label>
          <input
            type="text"
            value={ownerFullName}
            onChange={(e) => setOwnerFullName(e.target.value)}
            placeholder="e.g. John Doe"
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>
      </div>

      {/* Add Members */}
      <div className="bg-white rounded-xl border border-gray-200 p-6 mb-4">
        <h3 className="text-sm font-semibold text-gray-700 mb-4">Invite Members (optional)</h3>

        {/* Search */}
        <div className="flex gap-2 mb-3">
          <input
            type="email"
            value={searchEmail}
            onChange={(e) => setSearchEmail(e.target.value)}
            placeholder="Search by email..."
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

        {searchError && <p className="text-red-500 text-xs mb-3">{searchError}</p>}

        {/* Search Result */}
        {searchResult && (
          <div className="flex items-center justify-between bg-indigo-50 border border-indigo-100 rounded-lg px-4 py-3 mb-3">
            <div>
              <p className="text-sm font-medium text-gray-800">{searchResult.fullName}</p>
              <p className="text-xs text-gray-500">{searchEmail}</p>
            </div>
            <div className="flex items-center gap-2">
              <select
                value={selectedRole}
                onChange={(e) => setSelectedRole(e.target.value)}
                className="text-xs border border-gray-200 rounded-lg px-2 py-1.5 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              >
                {roles.map(role => (
                  <option key={role} value={role}>{role}</option>
                ))}
              </select>
              <button
                onClick={handleAddMember}
                className="px-3 py-1.5 bg-indigo-600 hover:bg-indigo-700 text-white text-xs font-medium rounded-lg transition-colors"
              >
                Add
              </button>
            </div>
          </div>
        )}

        {/* Added Members List */}
        {members.length > 0 && (
          <div className="space-y-2">
            {members.map(member => (
              <div key={member.userId} className="flex items-center justify-between py-2 px-3 bg-gray-50 rounded-lg">
                <div>
                  <p className="text-sm font-medium text-gray-800">{member.fullName}</p>
                  <p className="text-xs text-gray-500">{member.email} · {member.roleName}</p>
                </div>
                <button
                  onClick={() => handleRemoveMember(member.userId)}
                  className="text-red-400 hover:text-red-600 transition-colors"
                >
                  <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                  </svg>
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      {error && <p className="text-red-500 text-sm mb-4">{error}</p>}

      {/* Actions */}
      <div className="flex gap-3">
        <button
          onClick={() => navigate('/dashboard')}
          className="flex-1 py-2.5 border border-gray-300 text-gray-700 text-sm font-medium rounded-lg hover:bg-gray-50 transition-colors"
        >
          Cancel
        </button>
        <button
          onClick={handleCreate}
          disabled={loading}
          className="flex-1 py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-lg transition-colors disabled:opacity-50"
        >
          {loading ? 'Creating...' : 'Create Workspace'}
        </button>
      </div>
    </div>
  )
}

export default CreateWorkspacePage