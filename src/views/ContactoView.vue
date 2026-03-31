<script setup lang="ts">
const handleSubmit = () => {
  window.alert('Formulario enviado.')
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
        <form style="margin-bottom: 2rem" @submit.prevent="handleSubmit">
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
            <label for="asunto">Asunto:</label>
            <input type="text" id="asunto" name="asunto" required />
          </div>
          <div class="form-group">
            <label for="mensaje">Mensaje:</label>
            <textarea id="mensaje" name="mensaje" required></textarea>
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
