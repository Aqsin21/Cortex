import { useState } from 'react'

function LoginPage() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  async function handleLogin() {
    setLoading(true)
    setError('')

    try {
      const response = await fetch('https://localhost:7180/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      })

      const data = await response.json()

      if (!response.ok) {
        setError(data.error || 'Login failed.')
        return
      }

      // Token'ı kaydet
      localStorage.setItem('token', data.token)
      alert('Login successful! Token: ' + data.token)

    } catch (err) {
      setError('Could not connect to server.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div style={{ maxWidth: 400, margin: '100px auto', padding: 24 }}>
      <h2>Login</h2>

      <div style={{ marginBottom: 16 }}>
        <label>Email</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          style={{ display: 'block', width: '100%', padding: 8, marginTop: 4 }}
        />
      </div>

      <div style={{ marginBottom: 16 }}>
        <label>Password</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          style={{ display: 'block', width: '100%', padding: 8, marginTop: 4 }}
        />
      </div>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      <button
        onClick={handleLogin}
        disabled={loading}
        style={{ width: '100%', padding: 10, background: '#4f46e5', color: 'white', border: 'none', borderRadius: 6, cursor: 'pointer' }}
      >
        {loading ? 'Logging in...' : 'Login'}
      </button>
    </div>
  )
}

export default LoginPage