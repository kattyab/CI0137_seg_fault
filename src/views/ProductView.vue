<template>
  <main class="product-page">
    <div class="container">
      <div class="thumbs-col" @mouseleave="clearPreview">
        <img
          v-for="(t,i) in leftPreviews"
          :key="i"
          :src="t"
          class="thumb-single"
          @click="selectMainView(i)"
          @mouseenter="previewMainView(i)"
          @focus="previewMainView(i)"
          @keydown.enter.prevent="selectMainView(i)"
          tabindex="0"
          alt="preview"
        />
      </div>

      <div class="image-col">
        <div class="image-wrap">
          <img :src="imageSrc" alt="" class="main-image" />
        </div>
      </div>

      <aside class="details">
        <h1 class="product-title">{{ product.nombre }}</h1>
        <div class="price">₡{{ formattedPrice }}</div>

        <div class="color-swatches">
          <div
            v-for="(v,i) in variants"
            :key="v.id"
            class="swatch"
            role="button"
            tabindex="0"
            @click="selectVariant(i)"
            @keydown.enter.prevent="selectVariant(i)"
            @keydown.space.prevent="selectVariant(i)"
            :aria-label="`Select ${v.name} variant`"
            :title="v.name"
            :style="{ backgroundImage: `url(${v.image})`, backgroundSize: 'cover', backgroundPosition: 'center' }"
          ></div>
        </div>

        <div class="sizes">
          <h4>Select Size</h4>
          <div class="grid-sizes">
            <button v-for="s in sizes" :key="s" class="size" :class="{ active: selectedSize === s }" @click="chooseSize(s)">{{ s }}</button>
          </div>
          <button type="button" class="size-guide" @click.prevent="openSizeChart">Size Guide</button>
        </div>

        <div class="actions">
          <AddToCartButton :id="product.id" :nombre="product.nombre" :precio="product.precio" :size="selectedSize" :requireSize="true" />
        </div>
      </aside>
    </div>
  </main>

      <div v-if="showSizeChart" class="size-modal" @click.self="closeSizeChart">
        <div class="size-modal__content" role="dialog" aria-modal="true" aria-label="Size chart">
          <button class="size-modal__close" type="button" @click="closeSizeChart" aria-label="Cerrar">×</button>
          <img :src="sizeChart" alt="Size chart" class="size-chart-img" />
        </div>
      </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import AddToCartButton from '@/components/AddToCartButton.vue'

const route = useRoute()
const id = String(route.params.id || 'unknown')

// minimal product resolver from id — you can replace with API call
const product = computed(() => {
  // map id -> product data (fallback sample)
  const map: Record<string, any> = {
      'air-jordan-3-retro-sail-jade-aura': { id: 'air-jordan-3-retro-sail-jade-aura', nombre: 'Air Jordan 3 Retro Sail Jade Aura', precio: 120000, image: new URL('@/assets/images/collection/3retroSailandJadeAura/image.png', import.meta.url).href },
      'air-jordan-9-retro-flit-grey-french-blue': { id: 'air-jordan-9-retro-flit-grey-french-blue', nombre: 'Air Jordan 9 Retro Flit Grey French Blue', precio: 95000, image: new URL('@/assets/images/collection/9retroFlitGreyandFrenchBlue/image.png', import.meta.url).href },
      'air-jordan-mvp-92': { id: 'air-jordan-mvp-92', nombre: 'Air Jordan MVP 92', precio: 110000, image: new URL('@/assets/images/collection/MVP92/black-light%20blue.png', import.meta.url).href },
      'air-jordan-spizike': { id: 'air-jordan-spizike', nombre: 'Air Jordan Spizike', precio: 115000, image: new URL('@/assets/images/collection/spizikeG/image.png', import.meta.url).href },
      'air-jordan-1-low': {
        id: 'air-jordan-1-low',
        nombre: 'Air Jordan 1 Low',
        precio: 65000,
        variants: [
          { id: '1', name: 'White', image: new URL('@/assets/images/low/1low/white.png', import.meta.url).href },
          { id: '2', name: 'White / Blue', image: new URL('@/assets/images/low/1low/white-blue-light blue.png', import.meta.url).href },
          { id: '3', name: 'Gray / White / Black', image: new URL('@/assets/images/low/1low/gray-white-black.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-low-g': {
        id: 'air-jordan-1-low-g',
        nombre: 'Air Jordan 1 Low G',
        precio: 68000,
        variants: [
          { id: '1', name: 'Gray', image: new URL('@/assets/images/low/1lowG/gray.png', import.meta.url).href },
          { id: '2', name: 'Light Brown', image: new URL('@/assets/images/low/1lowG/light brown.png', import.meta.url).href },
          { id: '3', name: 'Neon White', image: new URL('@/assets/images/low/1lowG/neon-white.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-low-se': {
        id: 'air-jordan-1-low-se',
        nombre: 'Air Jordan 1 Low SE',
        precio: 70000,
        variants: [
          { id: '1', name: 'Brown', image: new URL('@/assets/images/low/1lowSE/brown.png', import.meta.url).href },
          { id: '2', name: 'Black', image: new URL('@/assets/images/low/1lowSE/black.png', import.meta.url).href },
          { id: '3', name: 'Wine', image: new URL('@/assets/images/low/1lowSE/wine.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-skyline-low': {
        id: 'air-jordan-1-skyline-low',
        nombre: 'Air Jordan 1 Skyline Low',
        precio: 72000,
        variants: [
          { id: '1', name: 'Red / White / Gray', image: new URL('@/assets/images/low/skylineLow/red-white-gray.png', import.meta.url).href },
          { id: '2', name: 'White / Black / Gray', image: new URL('@/assets/images/low/skylineLow/white-black-gray.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-mid': {
        id: 'air-jordan-1-mid',
        nombre: 'Air Jordan 1 Mid',
        precio: 75000,
        variants: [
          { id: '1', name: 'White / Black / Red', image: new URL('@/assets/images/mid/1mid/white-black-red.png', import.meta.url).href },
          { id: '2', name: 'White / Black / Light Blue', image: new URL('@/assets/images/mid/1mid/white-black-light blue.png', import.meta.url).href },
          { id: '3', name: 'Green', image: new URL('@/assets/images/mid/1mid/green.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-mid-se': {
        id: 'air-jordan-1-mid-se',
        nombre: 'Air Jordan 1 Mid SE',
        precio: 78000,
        variants: [
          { id: '1', name: 'Brown', image: new URL('@/assets/images/mid/1midSE/brown.png', import.meta.url).href },
          { id: '2', name: 'Blue', image: new URL('@/assets/images/mid/1midSE/blue.png', import.meta.url).href },
          { id: '3', name: 'Light Green', image: new URL('@/assets/images/mid/1midSE/light green.png', import.meta.url).href },
          { id: '4', name: 'Wine', image: new URL('@/assets/images/mid/1midSE/wine.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-mid-se-edge': {
        id: 'air-jordan-1-mid-se-edge',
        nombre: 'Air Jordan 1 Mid SE Edge',
        precio: 80000,
        variants: [
          { id: '1', name: 'Pink / Light Brown / Black', image: new URL('@/assets/images/mid/1midSEEdge/pink-light%20brown-black.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-retro-high-og': {
        id: 'air-jordan-1-retro-high-og',
        nombre: 'Air Jordan 1 Retro High OG',
        precio: 125000,
        variants: [
          { id: '1', name: 'Gray', image: new URL('@/assets/images/high/1retroHighOG/gray.png', import.meta.url).href },
          { id: '2', name: 'Light Blue', image: new URL('@/assets/images/high/1retroHighOG/light blue-light brown.png', import.meta.url).href },
        ],
      },
      'air-jordan-1-retro-high-9': {
        id: 'air-jordan-1-retro-high-9',
        nombre: 'Air Jordan 1 Retro High 9',
        precio: 128000,
        variants: [
          { id: '1', name: 'Black', image: new URL('@/assets/images/high/9retro/black.png', import.meta.url).href },
          { id: '2', name: 'Brown', image: new URL('@/assets/images/high/9retro/brown.png', import.meta.url).href },
        ],
      },
  }
  return map[id] ?? { id, nombre: id, precio: 0, image: '' }
})

const formattedPrice = computed(() => {
  const n = Number(product.value.precio ?? 0)
  return new Intl.NumberFormat('en-US').format(n)
})

import { ref, onMounted, watch, onBeforeUnmount } from 'vue'


const selectedVariant = ref(0)
const mainViewIndex = ref(0)
const variants = computed(() => product.value.variants ?? [{ id: product.value.id, name: product.value.nombre, image: product.value.image, images: [product.value.image] }])

const variantImages = computed(() => {
  const v = variants.value[selectedVariant.value]
  if (!v) return [product.value.image]
  return v.images ?? (v.image ? [v.image] : [product.value.image])
})

// produce a left-column list with several small previews (repeat if needed)
  // TEST OVERRIDE: set to true to force a temporary test image on all left thumbnails
  // (disabled now — set to true only for manual testing)
  const testThumbOverride = false
  const testThumb = new URL('@/assets/images/collection/3retroSailandJadeAura/image.png', import.meta.url).href

  const leftPreviews = computed(() => {
  const imgs = variantImages.value
  const count = 6
  const out: string[] = []
  if (testThumbOverride) {
    for (let i = 0; i < count; i++) out.push(testThumb)
    return out
  }
  for (let i = 0; i < count; i++) out.push(imgs[i % imgs.length])
  return out
})

const mainImageOverride = ref<string | null>(null)

const imageSrc = computed(() => {
  if (mainImageOverride.value) return mainImageOverride.value
  return variantImages.value[mainViewIndex.value] ?? variantImages.value[0] ?? ''
})

function selectVariant(i: string | number) { const idx = Number(i); selectedVariant.value = idx; mainViewIndex.value = 0 }
function selectMainView(i: string | number) { const idx = Number(i); mainViewIndex.value = idx; mainImageOverride.value = null }
function previewMainView(i: string | number) { const idx = Number(i); mainImageOverride.value = leftPreviews.value[idx] ?? null }
function clearPreview() { mainImageOverride.value = null }

const sizes = ['W 5 / M 3.5','W 5.5 / M 4','W 6 / M 4.5','W 6.5 / M 5','W 7 / M 5.5','W 7.5 / M 6']

const selectedSize = ref<string | null>(null)

function chooseSize(s: string) {
  selectedSize.value = s
}

onMounted(() => {
  window.scrollTo({ top: 0, behavior: 'auto' })
})

watch(() => route.params.id, () => {
  window.scrollTo({ top: 0, behavior: 'auto' })
})

// size chart modal
const sizeChart = new URL('@/assets/images/size/size-chart.jpg', import.meta.url).href
const showSizeChart = ref(false)
function openSizeChart() { showSizeChart.value = true }
function closeSizeChart() { showSizeChart.value = false }

function onKeyDown(e: KeyboardEvent) {
  if (e.key === 'Escape' && showSizeChart.value) closeSizeChart()
}

onMounted(() => {
  window.addEventListener('keydown', onKeyDown)
})

onBeforeUnmount(() => {
  window.removeEventListener('keydown', onKeyDown)
})
</script>

<style scoped>
  .product-page { min-height: calc(100vh - 80px); overflow-y: auto }

  .container {
  display: grid;
  grid-template-columns: 78px minmax(0, 500px) 360px;
    column-gap: 0.5rem;
    row-gap: 0.35rem;
    padding: 1rem;
    max-width: 1200px;
    margin: 0 auto;
    align-items: start;
  }

.thumbs-col {
  display: flex;
  flex-direction: column;
  gap: 18px;
  align-items: center;
  justify-content: flex-start;
  padding: 24px 0;
  height: 100%;
}

.thumb-single {
  width: 72px;
  height: 72px;
  object-fit: contain;
  background: transparent;
  padding: 8px;
  border-radius: 10px;
  border: none;
  box-shadow: none;
}

@media (hover: hover) {
  .thumbs-col:hover .thumb-single {
    filter: brightness(0.6);
    transition: filter 180ms ease, transform 180ms ease;
  }

  .thumbs-col .thumb-single:hover {
    filter: brightness(1);
    transform: translateY(-6px);
    box-shadow: 0 12px 26px rgba(0,0,0,0.12);
  }
}

/* no active indicator for thumbnails */

.thumb-active {
  outline: 3px solid #111;
  outline-offset: 4px;
}

.image-col { display: flex; align-items: flex-start; justify-content: flex-start }
.image-wrap {
  width: min(100%, 500px);
  min-height: auto;
  max-height: none;
  background: transparent;
  border-radius: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  padding: 0;
  overflow: visible;
}

.badge {
  position: absolute;
  top: 18px;
  left: 18px;
  background: white;
  padding: 0.45rem 0.8rem;
  border-radius: 999px;
  font-weight: 700;
  box-shadow: 0 8px 18px rgba(0,0,0,0.08);
  color: #333;
}

.main-image { width: 100%; max-width: 500px; height: auto; max-height: none; object-fit: contain; display: block; border: none; border-radius: 0; box-shadow: none }

.details { width: 360px; align-self: start; padding-left: 0.35rem; }
.product-title { font-size: 1.6rem; margin-bottom: 0.5rem; font-weight: 800 }
.price { font-weight: 900; margin: 0.5rem 0; font-size: 1.3rem }

.color-swatches { display:flex; gap:0.5rem; margin: 0.75rem 0 1.25rem }
.swatch { width:40px; height:40px; border-radius:8px; border:1px solid #ddd; background:#fff }

.sizes h4 { margin-bottom: 0.5rem; font-weight:700 }
.grid-sizes { display: grid; grid-template-columns: repeat(2, 1fr); gap: 0.65rem }
.size { padding: 0.9rem 1rem; border: 1px solid #e6e6e6; background: white; border-radius: 8px; font-weight:700; cursor:pointer; text-align:center }
.size:hover { border-color: #bbb }
.size:active { transform: translateY(1px) }
.size.active { background: #111; color: #fff; border-color: #111 }

.size-guide { display:inline-block; margin-top:0.6rem; color: #666; text-decoration: underline; font-size:0.9rem; border: none; background: none; padding: 0; cursor: pointer }
.size-guide:focus { outline: none }
.size-guide:active { outline: none }

.actions { margin-top: 1rem }

.size-modal {
  position: fixed;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0,0,0,0.6);
  z-index: 1200;
}
.size-modal__content {
  background: white;
  padding: 1rem;
  border-radius: 8px;
  max-width: 92vw;
  max-height: 92vh;
  overflow: auto;
  box-shadow: 0 20px 40px rgba(0,0,0,0.4);
  position: relative;
}
.size-modal__close {
  position: absolute;
  top: 8px;
  right: 8px;
  background: transparent;
  border: none;
  font-size: 1.6rem;
  cursor: pointer;
}
.size-chart-img { width: 100%; height: auto; display: block }

@media (max-width: 900px) {
  .container { grid-template-columns: 1fr; padding: 0.75rem }
  .thumbs-col { flex-direction: row; order: 2; gap: 0.5rem }
  .image-wrap { width: min(100%, 500px); min-height: auto }
  .details { width: 100%; order: 3; padding-left: 0 }
}
</style>
