<script setup lang="ts">
import { computed, reactive } from 'vue'
import { RouterLink } from 'vue-router'
import { Carousel, Slide, Navigation, Pagination } from 'vue3-carousel'
import 'vue3-carousel/dist/carousel.css'

import CyclingImage from '@/components/CyclingImage.vue'
import { useProducts } from '@/composables/useProducts'
import { productImage } from '@/services/productImages'

const { products } = useProducts()

const fmt = (n: number) => `₡${n.toLocaleString('es-CR')}`

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

const FEATURED = [
  {
    nombre: 'Air Jordan 1 Low G',
    kicker: 'Air Jordan Low',
    subtitle: 'Ligero y versatil para uso diario.',
    fallbackTo: '/low',
    fallbackPrice: 'Desde ₡65,000',
  },
  {
    nombre: 'Air Jordan 1 Mid SE Edge',
    kicker: 'Air Jordan Mid',
    subtitle: 'Balance ideal entre soporte y estilo urbano.',
    fallbackTo: '/mid',
    fallbackPrice: 'Desde ₡70,000',
  },
  {
    nombre: 'Air Jordan 1 Retro High OG',
    kicker: 'Air Jordan High',
    subtitle: 'Silueta clásica con presencia premium.',
    fallbackTo: '/high',
    fallbackPrice: 'Desde ₡80,000',
  },
  {
    nombre: 'Air Jordan 4 Retro SE We Are Eternal',
    kicker: 'Colección Especial',
    subtitle: 'Lanzamiento limitado para coleccionistas.',
    fallbackTo: '/coleccion',
    fallbackPrice: 'Edición limitada',
  },
]

const carouselItems = computed(() =>
  FEATURED.map((f) => {
    const p = products.value.find((x) => x.nombre === f.nombre)
    const image = productImage(f.nombre, null, p?.imagenUrl)
    return {
      key: f.nombre,
      to: p ? { name: 'product', params: { id: p.id }, query: { image } } : f.fallbackTo,
      image,
      title: f.nombre,
      kicker: f.kicker,
      subtitle: f.subtitle,
      price: p ? fmt(p.precio) : f.fallbackPrice,
    }
  }),
)

const CATEGORY_CARDS = [
  {
    categoria: 'Low',
    to: '/low',
    title: 'Air Jordan Low',
    price: 'Desde ₡65,000',
    models: 'Ver 4 Modelos',
    fallbackNombre: 'Air Jordan 1 Low G',
  },
  {
    categoria: 'Mid',
    to: '/mid',
    title: 'Air Jordan Mid',
    price: 'Desde ₡70,000',
    models: 'Ver 3 Modelos',
    fallbackNombre: 'Air Jordan 1 Mid SE Edge',
  },
  {
    categoria: 'High',
    to: '/high',
    title: 'Air Jordan High',
    price: 'Desde ₡80,000',
    models: 'Ver 2 Modelos',
    fallbackNombre: 'Air Jordan 1 Retro High OG',
  },
  {
    categoria: 'Collection',
    to: '/coleccion',
    title: 'Colección Especial',
    price: 'Ediciones limitadas',
    models: 'Ver 4 Modelos',
    fallbackNombre: 'Air Jordan 4 Retro SE We Are Eternal',
  },
]

// price/models fall back to the static strings until products load; the
// Collection card keeps its "Ediciones limitadas" label instead of a price
const categoryCards = computed(() =>
  CATEGORY_CARDS.map((c) => {
    const inCategory = products.value.filter((p) => p.categoria === c.categoria)
    const minPrice = inCategory.length > 0 ? Math.min(...inCategory.map((p) => p.precio)) : null
    return {
      ...c,
      images:
        inCategory.length > 0
          ? inCategory.map((p) => productImage(p.nombre, null, p.imagenUrl))
          : [productImage(c.fallbackNombre)],
      price: c.categoria !== 'Collection' && minPrice !== null ? `Desde ${fmt(minPrice)}` : c.price,
      models: inCategory.length > 0 ? `Ver ${inCategory.length} Modelos` : c.models,
    }
  }),
)
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
      <h2>Lanzamientos Destacados</h2>
      <div class="carousel-container">
        <Carousel :autoplay="4000" :wrap-around="true">
          <Slide v-for="item in carouselItems" :key="item.key">
            <RouterLink
              :to="item.to"
              class="carousel-item"
            >
              <div class="carousel-media">
                <img :src="item.image" :alt="item.title" />
              </div>
              <div class="carousel-content">
                <p class="carousel-kicker">{{ item.kicker }}</p>
                <h3>{{ item.title }}</h3>
                <p class="carousel-subtitle">{{ item.subtitle }}</p>
                <p class="carousel-price">{{ item.price }}</p>
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
      <h2 style="text-align: center; margin-bottom: 2rem">Categorías Disponibles</h2>
      <div class="grid">
        <div v-for="c in categoryCards" :key="c.categoria" class="card">
          <RouterLink :to="c.to" class="card-link">
            <CyclingImage :images="c.images" :alt="c.title" />
            <div class="card-content">
              <h3>{{ c.title }}</h3>
              <p>{{ c.price }}</p>
              <span>{{ c.models }}</span>
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
/* narrower than the site-wide 1200px so the featured cards feel more compact;
   percentage so it tracks the screen size at every breakpoint */
.carousel-container {
  max-width: min(80%, 1200px);
}

.carousel-item {
  position: relative !important;
  inset: auto !important;
  opacity: 1 !important;
  transform: none !important;
  pointer-events: auto !important;
  display: grid !important;
  text-align: left;
  width: 100%;
  height: 100%;
  min-height: 390px;
  box-shadow: none !important;
  border-radius: 0;
}

/* On mobile/tablet (768px and below) */
@media (max-width: 768px) {
  .carousel-item {
    display: grid !important;
    pointer-events: auto !important;
    min-height: 0;
  }
}

:deep(.carousel__track) {
  margin: 0;
}

:deep(.carousel__viewport) {
  border-radius: 14px;
  overflow: hidden;
  box-shadow: 0 14px 30px rgba(0, 0, 0, 0.15);
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

/* negative offsets sit the arrows just outside the card, in the space
   reserved by the container's side padding */
:deep(.carousel__prev) {
  left: -3.25rem;
}

:deep(.carousel__next) {
  right: -3.25rem;
}

@media (max-width: 768px) {
  :deep(.carousel__prev),
  :deep(.carousel__next) {
    width: 38px;
    height: 38px;
  }
  
  :deep(.carousel__prev) {
    left: -2.75rem;
  }

  :deep(.carousel__next) {
    right: -2.75rem;
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
