import { onMounted, onUnmounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'

const INACTIVITY_TIMEOUT = 15 * 60 * 1000 // 15 minutos en milisegundos

export const useSessionTimeout = () => {
  const auth = useAuthStore()
  const router = useRouter()
  let timeoutId: number | null = null

  const resetTimer = () => {
    // Limpiar el timeout anterior si existe
    if (timeoutId) {
      clearTimeout(timeoutId)
    }

    // Solo iniciar el timer si el usuario está autenticado
    if (auth.user) {
      timeoutId = window.setTimeout(() => {
        // Cerrar sesión por inactividad
        auth.logout()
        window.alert('Tu sesión ha expirado por inactividad.')
        router.push('/login')
      }, INACTIVITY_TIMEOUT)
    }
  }

  const setupListeners = () => {
    const events = ['mousedown', 'keydown', 'scroll', 'touchstart', 'click']

    events.forEach((event) => {
      document.addEventListener(event, resetTimer)
    })

    return () => {
      events.forEach((event) => {
        document.removeEventListener(event, resetTimer)
      })
    }
  }

  onMounted(() => {
    // Inicializar el timer cuando se monta el componente
    resetTimer()

    // Configurar listeners para detectar actividad
    const cleanup = setupListeners()

    onUnmounted(() => {
      cleanup()
      if (timeoutId) {
        clearTimeout(timeoutId)
      }
    })
  })
}
