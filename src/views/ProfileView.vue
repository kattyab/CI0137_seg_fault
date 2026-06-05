<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { userService } from '@/services/userService'

const auth = useAuthStore()

const user = computed(() => auth.user)

const fullUser = computed(() => {
  if (!auth.user) {
    return undefined
  }

  return userService.getUserByEmail(auth.user.email)
})

const createdAtText = computed(() => {
  if (!fullUser.value?.createdAt) {
    return 'No disponible'
  }

  return new Date(fullUser.value.createdAt).toLocaleDateString('es-CR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
})

</script>

<template>
  <main>
    <section class="profile-section">
      <div v-if="user" class="profile-card">
        <div class="profile-header">
          <div class="profile-avatar">
            {{ user.nombre.charAt(0).toUpperCase() }}
          </div>

          <div>
            <h2>Mi perfil</h2>
            <p class="profile-subtitle">
              Información de tu cuenta registrada.
            </p>
          </div>
        </div>

        <div class="profile-grid">
          <div class="profile-item">
            <span class="profile-label">Nombre completo</span>
            <span class="profile-value">{{ user.nombre }}</span>
          </div>

          <div class="profile-item">
            <span class="profile-label">Correo electrónico</span>
            <span class="profile-value">{{ user.email }}</span>
          </div>

          <div class="profile-item">
            <span class="profile-label">Teléfono</span>
            <span class="profile-value">
              {{ user.telefono || 'No registrado' }}
            </span>
          </div>

          <div class="profile-item">
            <span class="profile-label">Fecha de registro</span>
            <span class="profile-value">{{ createdAtText }}</span>
          </div>
        </div>
      </div>

      <div v-else class="profile-card profile-empty">
        <h2>No has iniciado sesión</h2>
        <p>Para ver tu perfil, primero debes ingresar a tu cuenta.</p>

        <RouterLink to="/login" class="profile-primary-link">
          Iniciar sesión
        </RouterLink>
      </div>
    </section>
  </main>
</template>
