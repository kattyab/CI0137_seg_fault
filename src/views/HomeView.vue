<script setup lang="ts">
import { reactive, ref } from 'vue'
import { RouterLink } from 'vue-router'

import lowImage from '@/assets/images/low/1lowG/gray.png'
import midImage from '@/assets/images/mid/1midSEEdge/pink-light brown-black.png'
import highImage from '@/assets/images/high/1retroHighOG/light blue-light brown.png'
import collectionImage from '@/assets/images/collection/3retroSailandJadeAura/image.png'

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

  window.alert('Comentario validado correctamente.')
}

const carouselItems = [
  {
    to: '/low',
    image: lowImage,
    alt: 'Air Jordan Low',
    title: 'Air Jordan Low',
    subtitle: 'Ligero y versatil para uso diario.',
    price: 'Desde ₡60,000',
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

const currentSlide = ref(0)

const nextSlide = () => {
  currentSlide.value = (currentSlide.value + 1) % carouselItems.length
}

const prevSlide = () => {
  currentSlide.value = (currentSlide.value - 1 + carouselItems.length) % carouselItems.length
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

    <section class="carrusel carrusel-home">
      <h2>Lanzamientos Recientes</h2>
      <div class="carrusel-container">
        <button class="carrusel-arrow" type="button" aria-label="Imagen anterior" @click="prevSlide">
          &#10094;
        </button>
        <div class="carrusel-track">
          <RouterLink
            v-for="(item, index) in carouselItems"
            :key="item.to"
            :to="item.to"
            class="carrusel-item"
            :class="{ active: index === currentSlide }"
            :data-index="index"
            :aria-hidden="index !== currentSlide"
          >
            <div class="carrusel-media">
              <img :src="item.image" :alt="item.alt" />
            </div>
            <div class="carrusel-content">
              <p class="carrusel-kicker">Lanzamiento destacado</p>
              <h3>{{ item.title }}</h3>
              <p class="carrusel-subtitle">{{ item.subtitle }}</p>
              <p class="carrusel-price">{{ item.price }}</p>
              <p class="carrusel-models">{{ item.models }}</p>
              <span class="carrusel-cta">Ver detalles</span>
            </div>
          </RouterLink>
        </div>
        <button class="carrusel-arrow" type="button" aria-label="Siguiente imagen" @click="nextSlide">
          &#10095;
        </button>
      </div>
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
