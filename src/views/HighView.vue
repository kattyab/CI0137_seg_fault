<script setup lang="ts">
const high1Main = new URL('@/assets/images/high/1retroHighOG/gray.png', import.meta.url).href
const high2Main = new URL('@/assets/images/high/9retro/black.png', import.meta.url).href
</script>

<template>
  <main>
    <section class="hero" style="height: 30vh">
      <h1>Air Jordan High</h1>
      <p>El modelo icónico de todo coleccionista, desde ₡80,000</p>
    </section>

    <section class="categorias">
      <h2>Modelos Disponibles</h2>
      <div class="grid">
        <div class="card">
          <router-link
            :to="{ name: 'product', params: { id: 'air-jordan-1-retro-high-og' }, query: { image: high1Main } }"
            class="product-detail card-link"
          >
            <div class="color-slider">
              <div class="slides">
                <div class="slide">
                  <img src="@/assets/images/high/1retroHighOG/gray.png" alt="Color 1" />
                </div>
              </div>
            </div>
            <div class="thumbs">
              <label for="1retroHighOG-1" class="thumb">
                <img src="@/assets/images/high/1retroHighOG/gray.png" alt="1" />
              </label>
              <label for="1retroHighOG-2" class="thumb">
                <img
                  src="@/assets/images/high/1retroHighOG/light blue-light brown.png"
                  alt="2"
                />
              </label>
            </div>
            <div class="card-content">
              <h3>Air Jordan 1 Retro High OG</h3>
              <p>₡80,000</p>
            </div>
          </router-link>
        </div>

        <div class="card">
          <router-link
            :to="{ name: 'product', params: { id: 'air-jordan-1-retro-high-9' }, query: { image: high2Main } }"
            class="product-detail card-link"
          >
            <div class="color-slider">
              <div class="slides">
                <div class="slide"><img src="@/assets/images/high/9retro/black.png" alt="Color 1"/></div>
              </div>
            </div>
            <div class="thumbs">
              <label for="9retro-1" class="thumb">
                <img src="@/assets/images/high/9retro/black.png" alt="1" />
              </label>
              <label for="9retro-2" class="thumb">
                <img src="@/assets/images/high/9retro/brown.png" alt="2" />
              </label>
            </div>
            <div class="card-content">
              <h3>Air Jordan 1 Retro High 9</h3>
              <p>₡85,000</p>
            </div>
          </router-link>
        </div>
      </div>
    </section>
  </main>
</template>
