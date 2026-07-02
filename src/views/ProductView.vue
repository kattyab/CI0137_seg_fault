<template>
  <main class="product-page">
    <div v-if="loading" class="state-msg">
      <p>Cargando producto…</p>
    </div>
    <div v-else-if="product" class="container">
      <div class="thumbs-col" @mouseleave="clearPreview">
        <img v-for="(t, i) in leftPreviews" :key="i" :src="t" class="thumb-single" @click="selectMainView(i)"
          @mouseenter="previewMainView(i)" @focus="previewMainView(i)" @keydown.enter.prevent="selectMainView(i)"
          tabindex="0" alt="preview" />
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
          <div v-for="(v, i) in variants" :key="v.id" class="swatch" role="button" tabindex="0"
            @click="selectVariant(i)" @keydown.enter.prevent="selectVariant(i)"
            @keydown.space.prevent="selectVariant(i)" :aria-label="`Select ${v.name} variant`" :title="v.name"
            :style="{ backgroundImage: `url(${v.image})`, backgroundSize: 'cover', backgroundPosition: 'center' }">
          </div>
        </div>

        <div class="sizes">
          <h4>Select Size</h4>
          <div class="grid-sizes">
            <button v-for="s in sizes" :key="s" class="size" :class="{ active: selectedSize === s }"
              @click="chooseSize(s)">{{ s }}</button>
          </div>
          <button type="button" class="size-guide" @click.prevent="openSizeChart">Size Guide</button>
        </div>

        <div class="actions">
          <AddToCartButton :id="String(product.id)" :nombre="product.nombre" :precio="product.precio" :size="selectedSize"
            :color="variants[selectedVariant]?.name || ''"
            :image="variants[selectedVariant]?.image || ''"
            :inventory-id="selectedEntry?.inventoryId" :stock="selectedEntry?.stock" :requireSize="true"
            @added="handleProductAdded" />
        </div>
      </aside>
    </div>
    <div v-else class="state-msg">
      <h2>Producto no encontrado</h2>
      <p>El producto que buscas no existe o ya no está disponible.</p>
      <RouterLink to="/" class="cta">Volver al inicio</RouterLink>
    </div>
  </main>

  <div v-if="showSizeChart" class="size-modal" @click.self="closeSizeChart">
    <div class="size-modal__content" role="dialog" aria-modal="true" aria-label="Size chart">
      <button class="size-modal__close" type="button" @click="closeSizeChart" aria-label="Cerrar">×</button>
      <img :src="sizeChart" alt="Size chart" class="size-chart-img" />
    </div>
  </div>

  <AddToCartModal v-if="showCartModal && addedProduct" :product="addedProduct" @close="showCartModal = false" :class="{ 'is-visible': showCartModal }" />
</template>

<script setup lang="ts">
import { computed, ref, onMounted, watch, onBeforeUnmount } from 'vue'
import { useRoute, RouterLink } from 'vue-router'
import AddToCartButton from '@/components/AddToCartButton.vue'
import AddToCartModal from '@/components/AddToCartModal.vue'
import { getProduct, type ProductDetail } from '@/services/productService'
import { productImage } from '@/services/productImages'
import type { CartItem } from '@/stores/cart'

const route = useRoute()

const product = ref<ProductDetail | null>(null)
const loading = ref(true)

async function loadProduct() {
  loading.value = true
  product.value = null
  selectedSize.value = null
  try {
    product.value = await getProduct(String(route.params.id))
  } catch {
    product.value = null
  } finally {
    loading.value = false
  }
  applyInitialImage()
}

const formattedPrice = computed(() => {
  const n = Number(product.value?.precio ?? 0)
  return new Intl.NumberFormat('en-US').format(n)
})

const selectedVariant = ref(0)
const mainViewIndex = ref(0)

type Variant = { id: string; name: string; image: string }

// unique colors from the inventory, mapped to local images (imagenUrl wins when the API provides it)
const variants = computed<Variant[]>(() => {
  const p = product.value
  if (!p) return []
  const seen = new Set<string>()
  const out: Variant[] = []
  for (const entry of p.inventario) {
    const color = entry.color ?? ''
    if (seen.has(color)) continue
    seen.add(color)
    out.push({ id: color || String(p.id), name: color, image: productImage(p.nombre, entry.color, p.imagenUrl) })
  }
  if (out.length === 0) out.push({ id: String(p.id), name: '', image: productImage(p.nombre, null, p.imagenUrl) })
  return out
})

const initialRouteImage = computed(() => (typeof route.query.image === 'string' ? route.query.image : ''))

const variantImages = computed(() => {
  const v = variants.value[selectedVariant.value]
  return v?.image ? [v.image] : []
})

// produce a left-column list with several small previews (repeat if needed)
const leftPreviews = computed(() => {
  const imgs = variantImages.value
  if (imgs.length === 0) return []
  const count = 6
  const out: string[] = []
  for (let i = 0; i < count; i++) out.push(imgs[i % imgs.length]!)
  return out
})

const mainImageOverride = ref<string | null>(null)

function applyInitialImage() {
  const queryImage = initialRouteImage.value
  selectedVariant.value = 0
  mainViewIndex.value = 0
  mainImageOverride.value = null

  if (!queryImage) return

  const matchedVariantIndex = variants.value.findIndex(v => v.image === queryImage)

  if (matchedVariantIndex !== -1) {
    selectedVariant.value = matchedVariantIndex
    return
  }

  mainImageOverride.value = queryImage
}

const imageSrc = computed(() => {
  if (mainImageOverride.value) return mainImageOverride.value
  return variantImages.value[mainViewIndex.value] ?? variantImages.value[0] ?? ''
})

function selectVariant(i: string | number) { const idx = Number(i); selectedVariant.value = idx; mainViewIndex.value = 0 }
function selectMainView(i: string | number) { const idx = Number(i); mainViewIndex.value = idx; mainImageOverride.value = null }
function previewMainView(i: string | number) { const idx = Number(i); mainImageOverride.value = leftPreviews.value[idx] ?? null }
function clearPreview() { mainImageOverride.value = null }

// unique tallas from the inventory, sorted by the leading W value
const sizes = computed(() => {
  const p = product.value
  if (!p) return []
  const seen = new Set<string>()
  for (const entry of p.inventario) seen.add(entry.talla)
  return [...seen].sort(
    (a, b) => parseFloat(a.replace(/^W\s*/, '')) - parseFloat(b.replace(/^W\s*/, '')),
  )
})

const selectedSize = ref<string | null>(null)

// inventory entry for the chosen color + size (carries inventoryId and stock for the cart)
const selectedEntry = computed(() => {
  const p = product.value
  if (!p || !selectedSize.value) return null
  const color = variants.value[selectedVariant.value]?.name || null
  return p.inventario.find(e => (e.color ?? null) === color && e.talla === selectedSize.value) ?? null
})

const showCartModal = ref(false)
const addedProduct = ref<CartItem | null>(null)

function handleProductAdded(item: CartItem) {
  addedProduct.value = item
  showCartModal.value = true
}

function chooseSize(s: string) {
  selectedSize.value = s
}

onMounted(() => {
  globalThis.scrollTo({ top: 0, behavior: 'auto' })
  void loadProduct()
})

watch(() => route.params.id, () => {
  globalThis.scrollTo({ top: 0, behavior: 'auto' })
  void loadProduct()
})

watch(() => route.query.image, () => {
  applyInitialImage()
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
  globalThis.addEventListener('keydown', onKeyDown)
})

onBeforeUnmount(() => {
  globalThis.removeEventListener('keydown', onKeyDown)
})
</script>

<style scoped>
.product-page {
  min-height: calc(100vh - 80px);
  overflow-y: auto
}

.state-msg {
  max-width: 1200px;
  margin: 0 auto;
  padding: 4rem 1rem;
  text-align: center;
}

.state-msg h2 {
  font-size: 1.6rem;
  font-weight: 800;
  margin-bottom: 0.5rem;
}

.state-msg .cta {
  display: inline-block;
  margin-top: 1rem;
}

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
    box-shadow: 0 12px 26px rgba(0, 0, 0, 0.12);
  }
}

/* no active indicator for thumbnails */

.thumb-active {
  outline: 3px solid #111;
  outline-offset: 4px;
}

.image-col {
  display: flex;
  align-items: flex-start;
  justify-content: flex-start
}

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
  box-shadow: 0 8px 18px rgba(0, 0, 0, 0.08);
  color: #333;
}

.main-image {
  width: 100%;
  max-width: 500px;
  height: auto;
  max-height: none;
  object-fit: contain;
  display: block;
  border: none;
  border-radius: 0;
  box-shadow: none
}

.details {
  width: 360px;
  align-self: start;
  padding-left: 0.35rem;
}

.product-title {
  font-size: 1.6rem;
  margin-bottom: 0.5rem;
  font-weight: 800
}

.price {
  font-weight: 900;
  margin: 0.5rem 0;
  font-size: 1.3rem
}

.color-swatches {
  display: flex;
  gap: 0.5rem;
  margin: 0.75rem 0 1.25rem
}

.swatch {
  width: 40px;
  height: 40px;
  border-radius: 8px;
  border: 1px solid #ddd;
  background: #fff
}

.sizes h4 {
  margin-bottom: 0.5rem;
  font-weight: 700
}

.grid-sizes {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 0.65rem;
  justify-items: start
}

.size {
  padding: 0.7rem 1rem;
  border: 1px solid #e6e6e6;
  background: white;
  border-radius: 8px;
  font-weight: 700;
  cursor: pointer;
  text-align: center;
  width: 100%;
  box-sizing: border-box;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis
}

.size:hover {
  border-color: #bbb
}

.size:active {
  transform: translateY(1px)
}

.size.active {
  background: #111;
  color: #fff;
  border-color: #111
}

.size-guide {
  display: inline-block;
  margin-top: 0.6rem;
  color: #666;
  text-decoration: underline;
  font-size: 0.9rem;
  border: none;
  background: none;
  padding: 0;
  cursor: pointer
}

.size-guide:focus {
  outline: none
}
.sizes h4 { margin-bottom: 0.5rem; font-weight:700 }
.grid-sizes { display: grid; grid-template-columns: repeat(2, 1fr); gap: 0.65rem; justify-items: start }
.size { padding: 0.65rem 0.5rem; border: 1px solid #e6e6e6; background: white; border-radius: 8px; font-weight:700; font-size: 0.9rem; cursor:pointer; text-align:center; width: 100%; box-sizing: border-box; white-space: nowrap; overflow: hidden; text-overflow: ellipsis }
.size:hover { border-color: #bbb }
.size:active { transform: translateY(1px) }
.size.active { background: #111; color: #fff; border-color: #111 }

.size-guide:active {
  outline: none
}

.actions {
  margin-top: 1rem
}

.size-modal {
  position: fixed;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0, 0, 0, 0.6);
  z-index: 1200;
}

.size-modal__content {
  background: white;
  padding: 1rem;
  border-radius: 8px;
  max-width: 92vw;
  max-height: 92vh;
  overflow: auto;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4);
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

.size-chart-img {
  width: 100%;
  height: auto;
  display: block
}

@media (max-width: 900px) {
  .container {
    grid-template-columns: 1fr;
    padding: 0.75rem
  }

  .thumbs-col {
    flex-direction: row;
    order: 2;
    gap: 0.5rem
  }

  .image-wrap {
    width: min(100%, 500px);
    min-height: auto
  }

  .details {
    width: 100%;
    order: 3;
    padding-left: 0
  }
}
</style>
