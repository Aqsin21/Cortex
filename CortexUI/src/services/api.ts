const BASE_URL = 'https://localhost:7180/api'

function getToken() {
  return localStorage.getItem('token')
}

export async function apiFetch(endpoint: string, options: RequestInit = {}) {
  const response = await fetch(`${BASE_URL}${endpoint}`, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${getToken()}`,
      ...options.headers,
    },
  })

  if (response.status === 401) {
    localStorage.removeItem('token')
    window.location.href = '/login'
  }

  return response
}