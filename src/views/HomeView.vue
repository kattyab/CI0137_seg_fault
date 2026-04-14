<script setup lang="ts">
import { RouterLink } from 'vue-router'

const handleSubmit = () => {
  window.alert('Formulario enviado.')
}
</script>

<template>
  <main>
    <section class="hero hero-home">
      <p class="hero-kicker">Lanzamientos 2026</p>
      <h1>
        Hablá con tu estilo.<br />
      </h1>
      <p class="hero-subtitle">
        <strong>Air Jordan originales</strong>
        <span class="hero-dot">•</span>
        <strong>Ediciones limitadas</strong>
        <span class="hero-dot">•</span>
        <strong>Envios rapidos en todo Costa Rica</strong>
      </p>
      <RouterLink class="cta hero-cta" to="/coleccion">Comprar Coleccion</RouterLink>
    </section>

    <section class="categorias">
      <h2 style="text-align: center; margin-bottom: 2rem">Categorías Destacadas</h2>
      <div class="grid">
        <div class="card">
          <RouterLink to="/low" class="card-link">
            <img src="@/assets/images/low/1lowG/gray.png" alt="Air Jordan Low" />
            <div class="card-content">
              <h3>Air Jordan Low</h3>
              <p>Desde ₡60,000</p>
              <span>Ver 4 Modelos</span>
            </div>
          </RouterLink>
        </div>
        <div class="card">
          <RouterLink to="/mid" class="card-link">
            <img
              src="@/assets/images/mid/1midSEEdge/pink-light brown-black.png"
              alt="Air Jordan Mid"
            />
            <div class="card-content">
              <h3>Air Jordan Mid</h3>
              <p>Desde ₡70,000</p>
              <span>Ver 3 Modelos</span>
            </div>
          </RouterLink>
        </div>
        <div class="card">
          <RouterLink to="/high" class="card-link">
            <img
              src="@/assets/images/high/1retroHighOG/light blue-light brown.png"
              alt="Air Jordan High"
            />
            <div class="card-content">
              <h3>Air Jordan High</h3>
              <p>Desde ₡80,000</p>
              <span>Ver 2 Modelos</span>
            </div>
          </RouterLink>
        </div>
        <div class="card">
          <RouterLink to="/coleccion" class="card-link">
            <img src="@/assets/images/collection/3retroSailandJadeAura/image.png" alt="Colección" />
            <div class="card-content">
              <h3>Colección Especial</h3>
              <p>Ediciones limitadas</p>
              <span>Ver 4 Modelos</span>
            </div>
          </RouterLink>
        </div>
      </div>
    </section>

    <section class="faq">
      <h2 class="section-title">Preguntas Frecuentes</h2>
      <div class="faq-list">
        <details>
          <summary>¿Cuánto tarda el envío en Costa Rica?</summary>
          <p>Los envíos tardan entre 2 y 4 días hábiles en el GAM y hasta 6 días en otras zonas.</p>
        </details>
        <details>
          <summary>¿Puedo cambiar la talla después de comprar?</summary>
          <p>
            Sí, tienes 7 días para cambios. El producto debe venir sin uso y con su caja original.
          </p>
        </details>
        <details>
          <summary>¿Qué métodos de pago aceptan?</summary>
          <p>Aceptamos tarjetas de crédito/débito y SINPE Móvil.</p>
        </details>
      </div>
    </section>

    <section class="forum">
      <div class="forum-header">
        <h2 class="section-title">Foro de la Comunidad</h2>
        <RouterLink class="cta" to="/foro">Ir al Foro</RouterLink>
      </div>
      <div class="forum-list">
        <article class="forum-card">
          <h3>Recomendaciones de limpieza</h3>
          <p>Comparte tips para mantener tus Air Jordan impecables.</p>
          <span class="pill">45 respuestas</span>
        </article>
        <article class="forum-card">
          <h3>¿Cuál fue tu mejor compra?</h3>
          <p>Cuenta tu experiencia y modelos favoritos.</p>
          <span class="pill">28 respuestas</span>
        </article>
        <article class="forum-card">
          <h3>Combinaciones de outfits</h3>
          <p>Ideas para combinar tus Jordan con estilo.</p>
          <span class="pill">19 respuestas</span>
        </article>
      </div>
    </section>

    <section class="comments">
      <h2 class="section-title">Comentarios de la Comunidad</h2>
      <div class="comments-grid">
        <div class="comment-card">
          <p>Excelente calidad, el envío llegó rápido y bien empacado.</p>
          <div class="comment-meta">Laura M. · Air Jordan Low</div>
        </div>
        <div class="comment-card">
          <p>La colección especial está brutal, volveré a comprar.</p>
          <div class="comment-meta">Carlos R. · Colección Especial</div>
        </div>
        <div class="comment-card">
          <p>Muy buena atención, resolvieron mis dudas en minutos.</p>
          <div class="comment-meta">Sofía P. · Soporte</div>
        </div>
        via the Vue CLI and is easily configurable in projects
      </div>
      <form class="comment-form" @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="comentario-nombre">Nombre</label>
          <input
            type="text"
            id="comentario-nombre"
            name="comentario-nombre"
            placeholder="Tu nombre"
            required
          />
        </div>
        <div class="form-group">
          <label for="comentario-texto">Comentario</label>
          <textarea
            id="comentario-texto"
            name="comentario-texto"
            placeholder="Escribe tu comentario"
            required
          ></textarea>
        </div>
        <button class="cta" type="submit">Publicar comentario</button>
      </form>
    </section>
  </main>
</template>
