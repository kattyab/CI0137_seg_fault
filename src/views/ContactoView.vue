<script setup lang="ts">
import { reactive } from 'vue'

const form = reactive({
  nombre: '',
  email: '',
  telefono: '',
  asunto: '',
  mensaje: '',
})

const errors = reactive({
  nombre: '',
  email: '',
  telefono: '',
  asunto: '',
  mensaje: '',
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

const validateAsunto = () => {
  if (!form.asunto.trim()) {
    errors.asunto = 'Ingresa el asunto del mensaje.'
    return false
  }

  if (form.asunto.trim().length < 5) {
    errors.asunto = 'El asunto debe tener al menos 5 caracteres.'
    return false
  }

  errors.asunto = ''
  return true
}

const validateMensaje = () => {
  if (!form.mensaje.trim()) {
    errors.mensaje = 'Escribe el mensaje que quieres enviar.'
    return false
  }

  if (form.mensaje.trim().length < 10) {
    errors.mensaje = 'El mensaje debe tener al menos 10 caracteres.'
    return false
  }

  errors.mensaje = ''
  return true
}

const handleSubmit = () => {
  const validations = [
    validateNombre(),
    validateEmail(),
    validateTelefono(),
    validateAsunto(),
    validateMensaje(),
  ]

  if (validations.some((isValid) => !isValid)) {
    return
  }

  window.alert('Mensaje enviado correctamente.')
}
</script>

<template>
  <main>
    <section class="categorias">
      <h2>Contacto</h2>
      <div
        style="
          max-width: 600px;
          margin: 0 auto;
          background: #fff;
          padding: 2rem;
          border-radius: 10px;
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        "
      >
        <form novalidate style="margin-bottom: 2rem" @submit.prevent="handleSubmit">
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
            <label for="asunto">Asunto:</label>
            <input
              id="asunto"
              v-model="form.asunto"
              :aria-invalid="Boolean(errors.asunto)"
              :class="{ 'is-invalid': errors.asunto }"
              name="asunto"
              type="text"
              @blur="validateAsunto"
            />
            <p v-if="errors.asunto" class="field-error">{{ errors.asunto }}</p>
          </div>
          <div class="form-group">
            <label for="mensaje">Mensaje:</label>
            <textarea
              id="mensaje"
              v-model="form.mensaje"
              :aria-invalid="Boolean(errors.mensaje)"
              :class="{ 'is-invalid': errors.mensaje }"
              name="mensaje"
              @blur="validateMensaje"
            ></textarea>
            <p v-if="errors.mensaje" class="field-error">{{ errors.mensaje }}</p>
          </div>
          <button class="cta" type="submit" style="width: 100%">Enviar Mensaje</button>
        </form>

        <div style="border-top: 2px solid #ddd; padding-top: 2rem">
          <h3>Información de Contacto</h3>
          <p><strong>Teléfono:</strong> +506 1234-5678</p>
          <p><strong>Email:</strong> charliessneakers@cr.com</p>
          <p><strong>Dirección:</strong> Santo Domingo, Heredia, Costa Rica</p>
          <p><strong>Horario:</strong> Lunes a Viernes 9:00 AM - 6:00 PM</p>
          <p><strong>Sábados</strong> 10:00 AM - 4:00 PM</p>
          <p><strong>Domingos:</strong> Cerrado</p>
        </div>
      </div>
    </section>
  </main>
</template>
