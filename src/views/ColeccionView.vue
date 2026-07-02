<script setup lang="ts">
import { useProducts } from '@/composables/useProducts'
import { productImage, variantImages } from '@/services/productImages'

const { products, loading, error } = useProducts('Collection')

const fmt = (n: number) => `₡${n.toLocaleString('es-CR')}`
</script>

<template>
  <main>
    <section class="hero" style="height: 30vh">
      <h1>Colección Especial</h1>
      <p>Ediciones limitadas y modelos exclusivos</p>
    </section>

    <section class="categorias">
      <h2>Ediciones Limitadas</h2>
      <p v-if="loading" style="text-align: center">Cargando modelos…</p>
      <p v-else-if="error" style="text-align: center">{{ error }}</p>
      <div v-else class="grid">
        <div v-for="p in products" :key="p.id" class="card">
          <router-link
            :to="{ name: 'product', params: { id: p.id }, query: { image: productImage(p.nombre, null, p.imagenUrl) } }"
            class="product-detail card-link"
          >
            <div class="color-slider">
              <div class="slides">
                <div class="slide">
                  <img :src="productImage(p.nombre, null, p.imagenUrl)" :alt="p.nombre" />
                </div>
              </div>
            </div>
            <div v-if="variantImages(p.nombre).length > 1" class="thumbs">
              <span v-for="v in variantImages(p.nombre)" :key="v.color" class="thumb">
                <img :src="v.url" :alt="v.color" />
              </span>
            </div>
            <div class="card-content">
              <h3>{{ p.nombre }}</h3>
              <p>{{ fmt(p.precio) }}</p>
              <p style="font-size: 0.9rem; color: #ff6b35">Edición Limitada</p>
            </div>
          </router-link>
        </div>
      </div>
    </section>
  </main>
</template>
