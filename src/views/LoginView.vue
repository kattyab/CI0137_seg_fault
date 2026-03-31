<script setup lang="ts">
const handleSubmit = () => {
  window.alert('Formulario enviado.')
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
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label for="email">Correo Electrónico:</label>
            <input type="email" id="email" name="email" required />
          </div>
          <div class="form-group">
            <label for="password">Contraseña:</label>
            <input type="password" id="password" name="password" required />
          </div>
          <div style="display: flex; align-items: center; margin-bottom: 1.5rem">
            <input
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
            <RouterLink to="/registro" style="color: #ff6b35; text-decoration: none; font-weight: bold"
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
