import { useState, useEffect } from 'react'
import { apiFetch } from '../services/api'

interface UserProfile {
  userId: string
  email: string
  firstName: string
  lastName: string
}

function ProfilePage() {
  const [profile, setProfile] = useState<UserProfile | null>(null)
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [currentPassword, setCurrentPassword] = useState('')
  const [newPassword, setNewPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [profileSuccess, setProfileSuccess] = useState('')
  const [profileError, setProfileError] = useState('')
  const [passwordSuccess, setPasswordSuccess] = useState('')
  const [passwordError, setPasswordError] = useState('')
  const [profileLoading, setProfileLoading] = useState(false)
  const [passwordLoading, setPasswordLoading] = useState(false)

  useEffect(() => {
    async function fetchProfile() {
      try {
        const response = await apiFetch('/auth/me')
        const data = await response.json()
        setProfile(data)
        setFirstName(data.firstName)
        setLastName(data.lastName)
      } catch {
        console.error('Failed to fetch profile')
      }
    }
    fetchProfile()
  }, [])

  async function handleUpdateProfile() {
    if (!firstName.trim() || !lastName.trim()) {
      setProfileError('First name and last name are required.')
      return
    }

    setProfileLoading(true)
    setProfileError('')
    setProfileSuccess('')

    try {
      const response = await apiFetch('/auth/profile', {
        method: 'PUT',
        body: JSON.stringify({ firstName, lastName })
      })

      if (!response.ok) {
        const data = await response.json()
        setProfileError(data.error || 'Failed to update profile.')
        return
      }

      setProfile(prev => prev ? { ...prev, firstName, lastName } : prev)
      setProfileSuccess('Profile updated successfully!')
    } catch {
      setProfileError('Something went wrong.')
    } finally {
      setProfileLoading(false)
    }
  }

  async function handleChangePassword() {
    if (!currentPassword || !newPassword || !confirmPassword) {
      setPasswordError('All fields are required.')
      return
    }

    if (newPassword !== confirmPassword) {
      setPasswordError('New passwords do not match.')
      return
    }

    if (newPassword.length < 6) {
      setPasswordError('New password must be at least 6 characters.')
      return
    }

    setPasswordLoading(true)
    setPasswordError('')
    setPasswordSuccess('')

    try {
      const response = await apiFetch('/auth/change-password', {
        method: 'PUT',
        body: JSON.stringify({ currentPassword, newPassword })
      })

      if (!response.ok) {
        const data = await response.json()
        setPasswordError(data.error || 'Failed to change password.')
        return
      }

      setPasswordSuccess('Password changed successfully!')
      setCurrentPassword('')
      setNewPassword('')
      setConfirmPassword('')
    } catch {
      setPasswordError('Something went wrong.')
    } finally {
      setPasswordLoading(false)
    }
  }

  return (
    <div className="max-w-2xl mx-auto">
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-gray-800">Profile Settings</h2>
        <p className="text-sm text-gray-500 mt-1">Manage your personal information and password.</p>
      </div>

      {/* Profile Card */}
      {profile && (
        <div className="bg-white rounded-xl border border-gray-200 p-6 mb-4 flex items-center gap-4">
          <div className="w-14 h-14 bg-indigo-600 rounded-full flex items-center justify-center text-white text-xl font-bold">
            {profile.firstName.charAt(0).toUpperCase()}
          </div>
          <div>
            <p className="font-semibold text-gray-800">{profile.firstName} {profile.lastName}</p>
            <p className="text-sm text-gray-500">{profile.email}</p>
          </div>
        </div>
      )}

      {/* Update Profile */}
      <div className="bg-white rounded-xl border border-gray-200 p-6 mb-4">
        <h3 className="text-sm font-semibold text-gray-700 mb-4">Personal Information</h3>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">First Name</label>
          <input
            type="text"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Last Name</label>
          <input
            type="text"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        {profileError && (
          <div className="mb-4 px-4 py-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-600">
            {profileError}
          </div>
        )}

        {profileSuccess && (
          <div className="mb-4 px-4 py-3 bg-green-50 border border-green-200 rounded-lg text-sm text-green-600">
            ✓ {profileSuccess}
          </div>
        )}

        <button
          onClick={handleUpdateProfile}
          disabled={profileLoading}
          className="w-full py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-lg transition-colors disabled:opacity-50"
        >
          {profileLoading ? 'Updating...' : 'Update Profile'}
        </button>
      </div>

      {/* Change Password */}
      <div className="bg-white rounded-xl border border-gray-200 p-6">
        <h3 className="text-sm font-semibold text-gray-700 mb-4">Change Password</h3>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Current Password</label>
          <input
            type="password"
            value={currentPassword}
            onChange={(e) => setCurrentPassword(e.target.value)}
            placeholder="Enter current password"
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">New Password</label>
          <input
            type="password"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            placeholder="Enter new password"
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Confirm New Password</label>
          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            placeholder="Confirm new password"
            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        {passwordError && (
          <div className="mb-4 px-4 py-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-600">
            {passwordError}
          </div>
        )}

        {passwordSuccess && (
          <div className="mb-4 px-4 py-3 bg-green-50 border border-green-200 rounded-lg text-sm text-green-600">
            ✓ {passwordSuccess}
          </div>
        )}

        <button
          onClick={handleChangePassword}
          disabled={passwordLoading}
          className="w-full py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-lg transition-colors disabled:opacity-50"
        >
          {passwordLoading ? 'Changing...' : 'Change Password'}
        </button>
      </div>
    </div>
  )
}

export default ProfilePage