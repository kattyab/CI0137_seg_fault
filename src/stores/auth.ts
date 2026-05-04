import { defineStore } from 'pinia'

export type AuthUser = {
  id: string
  nombre: string
  email: string
  telefono: string
}

const AUTH_USER_STORAGE_KEY = 'current_user'

const safeParse = (value: string | null): unknown => {
  if (!value) return null
  try {
    return JSON.parse(value)
  } catch {
    return null
  }
}

const isAuthUser = (value: unknown): value is AuthUser => {
  if (!value || typeof value !== 'object') return false
  const v = value as Record<string, unknown>
  return (
    typeof v.id === 'string' &&
    typeof v.nombre === 'string' &&
    typeof v.email === 'string' &&
    typeof v.telefono === 'string'
  )
}

export const useAuthStore = defineStore('auth', {
  state: () => {
    const parsed = safeParse(localStorage.getItem(AUTH_USER_STORAGE_KEY))
    return {
      user: isAuthUser(parsed) ? parsed : null,
    } as { user: AuthUser | null }
  },
  actions: {
    setUser(user: AuthUser) {
      this.user = user
      localStorage.setItem(AUTH_USER_STORAGE_KEY, JSON.stringify(user))
    },
    logout() {
      this.user = null
      localStorage.removeItem(AUTH_USER_STORAGE_KEY)
    },
  },
})
