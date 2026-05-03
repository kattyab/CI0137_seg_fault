<script setup lang="ts">
import { reactive } from 'vue'

const form = reactive({
  email: '',
  password: '',
  recordar: false,
})

const errors = reactive({
  email: '',
  password: '',
})

const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

const validateEmail = () => {
  if (!form.email.trim()) {
    errors.email = 'Ingresa tu correo electrónico.'
    return false
  }

  if (!emailPattern.test(form.email)) {
    errors.email = 'Ingresa un correo electrónico válido.'
    return false
  }

  errors.email = ''
  return true
}

const validatePassword = () => {
  if (!form.password) {
    errors.password = 'Ingresa tu contraseña.'
    return false
  }

  errors.password = ''
  return true
}

const handleSubmit = () => {
  const isEmailValid = validateEmail()
  const isPasswordValid = validatePassword()

  if (!isEmailValid || !isPasswordValid) {
    return
  }

  window.alert('Inicio de sesión validado correctamente.')
}
</script>

<template>
  <main>
    <section class="categorias">
      <h2>Iniciar Sesión</h2>
      <div
        style="
          max-width: 400px;
          margin: 0 auto;
          background: #fff;
          padding: 2rem;
          border-radius: 10px;
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        "
      >
        <form novalidate @submit.prevent="handleSubmit">
          <div class="form-group">
            <label for="email">Correo Electrónico:</label>
            <input
              id="email"
              v-model="form.email"
              :aria-invalid="Boolean(errors.email)"
              :class="{ 'is-invalid': errors.email }"
              autocomplete="email"
              name="email"
              type="email"
              @blur="validateEmail"
            />
            <p v-if="errors.email" class="field-error">{{ errors.email }}</p>
          </div>
          <div class="form-group">
            <label for="password">Contraseña:</label>
            <input
              id="password"
              v-model="form.password"
              :aria-invalid="Boolean(errors.password)"
              :class="{ 'is-invalid': errors.password }"
              autocomplete="current-password"
              name="password"
              type="password"
              @blur="validatePassword"
            />
            <p v-if="errors.password" class="field-error">{{ errors.password }}</p>
          </div>
          <div style="display: flex; align-items: center; margin-bottom: 1.5rem">
            <input
              v-model="form.recordar"
              type="checkbox"
              id="recordar"
              name="recordar"
              style="width: auto; margin-right: 0.5rem"
            />
            <label for="recordar" style="margin: 0">Recuérdame</label>
          </div>
          <button class="cta" type="submit" style="width: 100%">Iniciar Sesión</button>
        </form>

        <div
          style="
            margin-top: 2rem;
            text-align: center;
            border-top: 1px solid #ddd;
            padding-top: 1.5rem;
          "
        >
          <p style="margin-bottom: 1rem">
            ¿No tienes cuenta?
            <RouterLink
              to="/registro"
              style="color: #ff6b35; text-decoration: none; font-weight: bold"
              >Regístrate aquí</RouterLink
            >
          </p>
          <p>
            <RouterLink to="/recuperar-contrasena" style="color: #ff6b35; text-decoration: none"
              >¿Olvidaste tu contraseña?</RouterLink
            >
          </p>
        </div>
      </div>
    </section>
  </main>
</template>
