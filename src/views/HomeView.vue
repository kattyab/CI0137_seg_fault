<script setup lang="ts">
import { reactive } from 'vue'
import { RouterLink } from 'vue-router'
import { Carousel, Slide, Navigation, Pagination } from 'vue3-carousel'
import 'vue3-carousel/dist/carousel.css'

import lowImage from '@/assets/images/low/1lowG/mediumGray-tintBlue-white/right.png'
import midImage from '@/assets/images/mid/1midSEEdge/paleIvory-black-muslin-carreraPink/right.png'
import highImage from '@/assets/images/high/1retroHighOG/coolGrey-sail-gameRoyal-black/right.png'
import collectionImage from '@/assets/images/collection/airJordan4RetroSEWeAreEternal/right.png'

const commentForm = reactive({
  nombre: '',
  comentario: '',
})

const commentErrors = reactive({
  nombre: '',
  comentario: '',
})

const validateCommentName = () => {
  if (!commentForm.nombre.trim()) {
    commentErrors.nombre = 'Ingresa tu nombre.'
    return false
  }

  if (commentForm.nombre.trim().length < 2) {
    commentErrors.nombre = 'El nombre debe tener al menos 2 caracteres.'
    return false
  }

  commentErrors.nombre = ''
  return true
}

const validateCommentText = () => {
  if (!commentForm.comentario.trim()) {
    commentErrors.comentario = 'Escribe un comentario.'
    return false
  }

  if (commentForm.comentario.trim().length < 10) {
    commentErrors.comentario = 'El comentario debe tener al menos 10 caracteres.'
    return false
  }

  commentErrors.comentario = ''
  return true
}

const handleSubmit = () => {
  const isNameValid = validateCommentName()
  const isCommentValid = validateCommentText()

  if (!isNameValid || !isCommentValid) {
    return
  }

  globalThis.alert('Comentario validado correctamente.')
}

const carouselItems = [
  {
    to: '/low',
    image: lowImage,
    alt: 'Air Jordan Low',
    title: 'Air Jordan Low',
    subtitle: 'Ligero y versatil para uso diario.',
    price: 'Desde ₡65,000',
    models: '4 modelos disponibles',
  },
  {
    to: '/mid',
    image: midImage,
    alt: 'Air Jordan Mid',
    title: 'Air Jordan Mid',
    subtitle: 'Balance ideal entre soporte y estilo urbano.',
    price: 'Desde ₡70,000',
    models: '3 modelos disponibles',
  },
  {
    to: '/high',
    image: highImage,
    alt: 'Air Jordan High',
    title: 'Air Jordan High',
    subtitle: 'Silueta clásica con presencia premium.',
    price: 'Desde ₡80,000',
    models: '2 modelos disponibles',
  },
  {
    to: '/coleccion',
    image: collectionImage,
    alt: 'Colección',
    title: 'Colección Especial',
    subtitle: 'Lanzamientos limitados para coleccionistas.',
    price: 'Ediciones limitadas',
    models: '4 modelos disponibles',
  },
]
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

    <section class="carousel carousel-home">
      <h2>Lanzamientos Recientes</h2>
      <div class="carousel-container">
        <Carousel :autoplay="4000" :wrap-around="true">
          <Slide v-for="item in carouselItems" :key="item.to">
            <RouterLink
              :to="item.to"
              class="carousel-item"
            >
              <div class="carousel-media">
                <img :src="item.image" :alt="item.alt" />
              </div>
              <div class="carousel-content">
                <p class="carousel-kicker">Lanzamiento destacado</p>
                <h3>{{ item.title }}</h3>
                <p class="carousel-subtitle">{{ item.subtitle }}</p>
                <p class="carousel-price">{{ item.price }}</p>
                <p class="carousel-models">{{ item.models }}</p>
                <span class="carousel-cta">Ver detalles</span>
              </div>
            </RouterLink>
          </Slide>

          <template #addons>
            <Navigation />
            <Pagination />
          </template>
        </Carousel>
      </div>
    </section>

    <section class="categorias">
      <h2 style="text-align: center; margin-bottom: 2rem">Categorías Destacadas</h2>
      <div class="grid">
        <div class="card">
          <RouterLink to="/low" class="card-link">
            <img src="@/assets/images/low/1lowG/mediumGray-tintBlue-white/right.png" alt="Air Jordan Low" />
            <div class="card-content">
              <h3>Air Jordan Low</h3>
              <p>Desde ₡65,000</p>
              <span>Ver 4 Modelos</span>
            </div>
          </RouterLink>
        </div>
        <div class="card">
          <RouterLink to="/mid" class="card-link">
            <img
              src="@/assets/images/mid/1midSEEdge/paleIvory-black-muslin-carreraPink/right.png"
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
              src="@/assets/images/high/1retroHighOG/coolGrey-sail-gameRoyal-black/right.png"
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
            <img src="@/assets/images/collection/airJordan4RetroSEWeAreEternal/right.png" alt="Colección" />
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
      </div>
      <form novalidate class="comment-form" @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="comentario-nombre">Nombre</label>
          <input
            id="comentario-nombre"
            v-model="commentForm.nombre"
            :aria-invalid="Boolean(commentErrors.nombre)"
            :class="{ 'is-invalid': commentErrors.nombre }"
            autocomplete="name"
            name="comentario-nombre"
            type="text"
            placeholder="Tu nombre"
            @blur="validateCommentName"
          />
          <p v-if="commentErrors.nombre" class="field-error">{{ commentErrors.nombre }}</p>
        </div>
        <div class="form-group">
          <label for="comentario-texto">Comentario</label>
          <textarea
            id="comentario-texto"
            v-model="commentForm.comentario"
            :aria-invalid="Boolean(commentErrors.comentario)"
            :class="{ 'is-invalid': commentErrors.comentario }"
            name="comentario-texto"
            placeholder="Escribe tu comentario"
            @blur="validateCommentText"
          ></textarea>
          <p v-if="commentErrors.comentario" class="field-error">{{ commentErrors.comentario }}</p>
        </div>
        <button class="cta" type="submit">Publicar comentario</button>
      </form>
    </section>
  </main>
</template>

<style scoped>
.carousel-item {
  position: relative !important;
  inset: auto !important;
  opacity: 1 !important;
  transform: none !important;
  pointer-events: auto !important;
  display: grid !important;
  text-align: left;
}

/* On mobile/tablet (768px and below) */
@media (max-width: 768px) {
  .carousel-item {
    display: grid !important;
    pointer-events: auto !important;
  }
}

:deep(.carousel__track) {
  margin: 0;
}

:deep(.carousel__viewport) {
  border-radius: 14px;
  overflow: hidden;
}

:deep(.carousel__slide) {
  padding: 0;
}

:deep(.carousel__prev),
:deep(.carousel__next) {
  width: 44px;
  height: 44px;
  border: none;
  border-radius: 999px;
  background: var(--primary-color) !important;
  color: var(--card-and-section-background) !important;
  font-size: 1.6rem;
  line-height: 1;
  cursor: pointer;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
  transition: background 0.2s ease, box-shadow 0.2s ease;
  z-index: 3;
}

:deep(.carousel__prev:hover),
:deep(.carousel__next:hover) {
  background: #d94f22 !important;
  box-shadow: 0 10px 22px rgba(0, 0, 0, 0.25);
  color: var(--card-and-section-background) !important;
}

:deep(.carousel__prev) {
  left: 0.75rem;
}

:deep(.carousel__next) {
  right: 0.75rem;
}

@media (max-width: 768px) {
  :deep(.carousel__prev),
  :deep(.carousel__next) {
    width: 38px;
    height: 38px;
  }
  
  :deep(.carousel__prev) {
    left: 0.25rem;
  }

  :deep(.carousel__next) {
    right: 0.25rem;
  }
}

:deep(.carousel__pagination) {
  display: flex;
  justify-content: center;
  list-style: none;
  padding: 0;
  margin-top: 1.5rem;
  gap: 0.5rem;
}

:deep(.carousel__pagination-button) {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background-color: #ccc;
  border: none;
  padding: 0;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

:deep(.carousel__pagination-button::after) {
  display: none; /* Hide standard pseudo-element if active in newer vue3-carousel designs */
}

:deep(.carousel__pagination-button--active) {
  background-color: var(--primary-color);
}
</style>
