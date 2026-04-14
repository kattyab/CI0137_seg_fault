<script setup lang="ts">
const handleSubmit = () => {
  window.alert('Formulario enviado.')
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
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label for="nombre">Nombre Completo:</label>
            <input type="text" id="nombre" name="nombre" required />
          </div>
          <div class="form-group">
            <label for="email">Correo Electrónico:</label>
            <input type="email" id="email" name="email" required />
          </div>
          <div class="form-group">
            <label for="telefono">Teléfono:</label>
            <input type="tel" id="telefono" name="telefono" />
          </div>
          <div class="form-group">
            <label for="password">Contraseña:</label>
            <input type="password" id="password" name="password" required />
          </div>
          <div class="form-group">
            <label for="password-confirm">Confirmar Contraseña:</label>
            <input type="password" id="password-confirm" name="password-confirm" required />
          </div>
          <div style="display: flex; align-items: center; margin-bottom: 1.5rem">
            <input
              type="checkbox"
              id="terminos"
              name="terminos"
              style="width: auto; margin-right: 0.5rem"
              required
            />
            <label for="terminos" style="margin: 0"
              >Aceptar
              <RouterLink to="/privacidad" style="color: #ff6b35"
                >términos y condiciones</RouterLink
              ></label
            >
          </div>
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
