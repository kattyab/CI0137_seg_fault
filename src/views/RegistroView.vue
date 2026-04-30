<script setup lang="ts">
import { reactive } from 'vue'

const form = reactive({
  nombre: '',
  email: '',
  telefono: '',
  password: '',
  passwordConfirm: '',
  terminos: false,
})

const errors = reactive({
  nombre: '',
  email: '',
  telefono: '',
  password: '',
  passwordConfirm: '',
  terminos: '',
})

const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
const phonePattern = /^[0-9+\s-]{8,15}$/

const validateNombre = () => {
  if (!form.nombre.trim()) {
    errors.nombre = 'Ingresa tu nombre completo.'
    return false
  }

  if (form.nombre.trim().length < 3) {
    errors.nombre = 'El nombre debe tener al menos 3 caracteres.'
    return false
  }

  errors.nombre = ''
  return true
}

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

const validateTelefono = () => {
  if (!form.telefono.trim()) {
    errors.telefono = ''
    return true
  }

  if (!phonePattern.test(form.telefono.trim())) {
    errors.telefono = 'Ingresa un teléfono válido de 8 a 15 dígitos.'
    return false
  }

  errors.telefono = ''
  return true
}

const validatePassword = () => {
  if (!form.password) {
    errors.password = 'Ingresa una contraseña.'
    return false
  }

  if (form.password.length < 8) {
    errors.password = 'La contraseña debe tener al menos 8 caracteres.'
    return false
  }

  errors.password = ''
  return true
}

const validatePasswordConfirm = () => {
  if (!form.passwordConfirm) {
    errors.passwordConfirm = 'Confirma tu contraseña.'
    return false
  }

  if (form.passwordConfirm !== form.password) {
    errors.passwordConfirm = 'Las contraseñas no coinciden.'
    return false
  }

  errors.passwordConfirm = ''
  return true
}

const validateTerminos = () => {
  if (!form.terminos) {
    errors.terminos = 'Debes aceptar los términos y condiciones.'
    return false
  }

  errors.terminos = ''
  return true
}

const handleSubmit = () => {
  const validations = [
    validateNombre(),
    validateEmail(),
    validateTelefono(),
    validatePassword(),
    validatePasswordConfirm(),
    validateTerminos(),
  ]

  if (validations.some((isValid) => !isValid)) {
    return
  }

  window.alert('Cuenta creada correctamente.')
}
</script>

<template>
  <main>
    <section class="categorias">
      <h2>Crear Nueva Cuenta</h2>
      <div
        style="
          max-width: 500px;
          margin: 0 auto;
          background: #fff;
          padding: 2rem;
          border-radius: 10px;
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        "
      >
        <form novalidate @submit.prevent="handleSubmit">
          <div class="form-group">
            <label for="nombre">Nombre Completo:</label>
            <input
              id="nombre"
              v-model="form.nombre"
              :aria-invalid="Boolean(errors.nombre)"
              :class="{ 'is-invalid': errors.nombre }"
              autocomplete="name"
              name="nombre"
              type="text"
              @blur="validateNombre"
            />
            <p v-if="errors.nombre" class="field-error">{{ errors.nombre }}</p>
          </div>
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
            <label for="telefono">Teléfono:</label>
            <input
              id="telefono"
              v-model="form.telefono"
              :aria-invalid="Boolean(errors.telefono)"
              :class="{ 'is-invalid': errors.telefono }"
              autocomplete="tel"
              name="telefono"
              type="tel"
              @blur="validateTelefono"
            />
            <p v-if="errors.telefono" class="field-error">{{ errors.telefono }}</p>
          </div>
          <div class="form-group">
            <label for="password">Contraseña:</label>
            <input
              id="password"
              v-model="form.password"
              :aria-invalid="Boolean(errors.password)"
              :class="{ 'is-invalid': errors.password }"
              autocomplete="new-password"
              name="password"
              type="password"
              @blur="validatePassword"
            />
            <p v-if="errors.password" class="field-error">{{ errors.password }}</p>
          </div>
          <div class="form-group">
            <label for="password-confirm">Confirmar Contraseña:</label>
            <input
              id="password-confirm"
              v-model="form.passwordConfirm"
              :aria-invalid="Boolean(errors.passwordConfirm)"
              :class="{ 'is-invalid': errors.passwordConfirm }"
              autocomplete="new-password"
              name="password-confirm"
              type="password"
              @blur="validatePasswordConfirm"
            />
            <p v-if="errors.passwordConfirm" class="field-error">{{ errors.passwordConfirm }}</p>
          </div>
          <div style="display: flex; align-items: center; margin-bottom: 1.5rem">
            <input
              v-model="form.terminos"
              type="checkbox"
              id="terminos"
              name="terminos"
              style="width: auto; margin-right: 0.5rem"
            />
            <label for="terminos" style="margin: 0"
              >Aceptar
              <RouterLink to="/privacidad" style="color: #ff6b35"
                >términos y condiciones</RouterLink
              ></label
            >
          </div>
          <p v-if="errors.terminos" class="field-error" style="margin-top: -1rem; margin-bottom: 1rem">
            {{ errors.terminos }}
          </p>
          <button class="cta" type="submit" style="width: 100%">Crear Cuenta</button>
        </form>

        <div
          style="
            margin-top: 2rem;
            text-align: center;
            border-top: 1px solid #ddd;
            padding-top: 1.5rem;
          "
        >
          <p>
            ¿Ya tienes cuenta?
            <RouterLink to="/login" style="color: #ff6b35; text-decoration: none; font-weight: bold"
              >Inicia sesión aquí</RouterLink
            >
          </p>
        </div>
      </div>
    </section>
  </main>
</template>
