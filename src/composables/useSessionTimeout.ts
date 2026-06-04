import { onMounted, onUnmounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'
import { STORAGE_KEYS } from '@/constants/storageKeys'

const INACTIVITY_TIMEOUT = 15 * 60 * 1000 // 15 minutos
const MAX_SESSION_DURATION = 60 * 60 * 1000 // 1 hora (60 * 60 * 1000)

// Estado global de los timers
let inactivityTimerId: number | null = null
let maxSessionTimerId: number | null = null
let eventListenersActive = false

// Guardamos la referencia al router fuera del intervalo para evitar llamarlo
// dentro de un contexto sin instancia Vue activa
let routerInstance: ReturnType<typeof useRouter> | null = null

// Helpers
const clearAllTimers = () => {
  if (inactivityTimerId) { clearTimeout(inactivityTimerId); inactivityTimerId = null }
  if (maxSessionTimerId) { clearTimeout(maxSessionTimerId); maxSessionTimerId = null }
}

const removeActivityListeners = () => {
  if (!eventListenersActive) return
  const events = ['mousedown', 'keydown', 'scroll', 'touchstart', 'click']
  events.forEach(e => document.removeEventListener(e, onUserActivity))
  eventListenersActive = false
}

// Expirar sesión
const expireSession = (message: string) => {
  clearAllTimers()
  removeActivityListeners()

  const auth = useAuthStore()
  auth.logout() // limpia localStorage + estado Pinia

  window.alert(message)
  routerInstance?.push('/login')
}

// Inactividad
const onUserActivity = () => {
  const auth = useAuthStore()
  if (!auth.user) return

  if (inactivityTimerId) clearTimeout(inactivityTimerId)

  inactivityTimerId = window.setTimeout(() => {
    expireSession('Tu sesión ha expirado por inactividad.')
  }, INACTIVITY_TIMEOUT)
}

const addActivityListeners = () => {
  if (eventListenersActive) return
  const events = ['mousedown', 'keydown', 'scroll', 'touchstart', 'click']
  events.forEach(e => document.addEventListener(e, onUserActivity))
  eventListenersActive = true
}

// Timer de duración máxima
const startMaxSessionTimer = () => {
  const now = Date.now()
  const stored = localStorage.getItem(STORAGE_KEYS.SESSION_START)

  // Si no existe, guardamos el inicio de sesión ahora
  if (!stored) {
    localStorage.setItem(STORAGE_KEYS.SESSION_START, String(now))
  }

  const sessionStart = Number(localStorage.getItem(STORAGE_KEYS.SESSION_START))
  const elapsed = now - sessionStart
  const remaining = MAX_SESSION_DURATION - elapsed

  console.log(`Sesión iniciada. Tiempo restante: ${remaining}ms`)

  // Ya expiró (ej: recargó la página después del límite)
  if (remaining <= 0) {
    expireSession('Tu sesión ha alcanzado el tiempo máximo.')
    return
  }

  if (maxSessionTimerId) clearTimeout(maxSessionTimerId)

  maxSessionTimerId = window.setTimeout(() => {
    expireSession('Tu sesión ha alcanzado el tiempo máximo.')
  }, remaining)
}

// API pública

/**
 * Llama esto justo después de auth.setUser() en LoginView
 * para registrar el inicio de sesión y arrancar todos los timers.
 */
export const initSession = (router: ReturnType<typeof useRouter>) => {
  routerInstance = router
  localStorage.setItem(STORAGE_KEYS.SESSION_START, String(Date.now()))
  startMaxSessionTimer()
  addActivityListeners()
  onUserActivity() // arranca el timer de inactividad de inmediato
  console.log('Sesión iniciada y timers activos')
}

/**
 * Llama esto en App.vue (onMounted) para reanudar
 * la vigilancia si el usuario ya tenía sesión activa (recargó la página).
 */
export const useSessionTimeout = () => {
  const auth = useAuthStore()
  const router = useRouter()

  onMounted(() => {
    if (!auth.user) return   // sin sesión → nada que vigilar

    console.log('Sesión existente detectada, reanudando timers...')
    routerInstance = router
    startMaxSessionTimer()
    addActivityListeners()
    onUserActivity()
  })

  onUnmounted(() => {
    // No limpiamos timers aquí: App.vue nunca se desmonta mientras la app corre.
    // Si quisieras usarlo en un componente hijo temporal, sí limpiarías aquí.
  })
}