import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { apiFetch } from '../services/api'

function RegisterPage() {
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  async function handleRegister() {
    setLoading(true)
    setError('')

    try {
      const response = await apiFetch('/auth/register', {
        method: 'POST',
        body: JSON.stringify({ firstName, lastName, email, password })
      })

      
      let data: any = null;
      try {
        data = await response.json()
      } catch (jsonErr) {
        
      }

      if (!response.ok) {
        if (!data) {
          setError('Register failed. No response from server.')
          return
        }

        let rawMessage = ''

        
        if (Array.isArray(data)) {
          rawMessage = data.join(' ')
        } 
        
        else if (data.errors) {
          rawMessage = Object.values(data.errors).flat().join(' ')
        } 

        else {
          rawMessage = data.error || data.message || 'Register failed.'
        }

       
        const cleanMessage = rawMessage.replace(/Username/g, 'Email').replace(/username/g, 'email')
        setError(cleanMessage)
        return
      }

      
      navigate('/login')

    } catch (err) {
      
      setError('Could not connect to server. Please check your connection.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center">
      <div className="bg-white p-8 rounded-xl shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold text-gray-800 mb-6">Register</h2>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">First Name</label>
          <input
            type="text"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Last Name</label>
          <input
            type="text"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">Password</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        {error && <p className="text-red-500 text-sm mb-4">{error}</p>}

        <button
          onClick={handleRegister}
          disabled={loading}
          className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-medium py-2 rounded-lg transition-colors disabled:opacity-50"
        >
          {loading ? 'Registering...' : 'Register'}
        </button>

        <p className="text-center text-sm text-gray-500 mt-4">
          Already have an account?{' '}
          <a href="/login" className="text-indigo-600 hover:underline">Login</a>
        </p>
      </div>
    </div>
  )
}

export default RegisterPage