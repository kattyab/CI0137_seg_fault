<script setup lang="ts">
const handleSubmit = () => {
  window.alert('Formulario enviado.')
}
</script>

<template>
  <main>
    <section class="categorias">
      <h2>Recuperar Contraseña</h2>
      <div
        style="
          max-width: 450px;
          margin: 0 auto;
          background: #fff;
          padding: 2rem;
          border-radius: 10px;
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        "
      >
        <p style="margin-bottom: 1.5rem; color: #666">
          Ingresa tu correo electrónico y te enviaremos un enlace para recuperar tu contraseña.
        </p>

        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label for="email">Correo Electrónico:</label>
            <input type="email" id="email" name="email" required />
          </div>
          <button class="cta" type="submit" style="width: 100%">
            Enviar Enlace de Recuperación
          </button>
        </form>

        <div
          style="
            margin-top: 2rem;
            text-align: center;
            border-top: 1px solid #ddd;
            padding-top: 1.5rem;
          "
        >
          <p style="margin-bottom: 0.5rem">
            ¿No tienes cuenta?
            <RouterLink
              to="/registro"
              style="color: #ff6b35; text-decoration: none; font-weight: bold"
              >Regístrate aquí</RouterLink
            >
          </p>
          <p>
            <RouterLink to="/login" style="color: #ff6b35; text-decoration: none"
              >Volver a Iniciar Sesión</RouterLink
            >
          </p>
        </div>
      </div>
    </section>
  </main>
</template>
