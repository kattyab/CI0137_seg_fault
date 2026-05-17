<script setup lang="ts">
const low1Main = new URL('@/assets/images/low/1low/gray-white-black.png', import.meta.url).href
const low2Main = new URL('@/assets/images/low/1lowG/gray.png', import.meta.url).href
const low3Main = new URL('@/assets/images/low/1lowSE/black.png', import.meta.url).href
const low4Main = new URL('@/assets/images/low/skylineLow/red-white-gray.png', import.meta.url).href
</script>

<template>
  <main>
    <section class="hero" style="height: 30vh">
      <h1>Air Jordan Low</h1>
      <p>Los clásicos más cómodos, desde ₡60,000</p>
    </section>

    <section class="categorias">
      <h2>Modelos Disponibles</h2>
      <div class="grid">
        <div class="card">
          <router-link
            :to="{ name: 'product', params: { id: 'air-jordan-1-low' }, query: { image: low1Main } }"
            class="product-detail card-link"
          >
            <div class="color-slider">
              <div class="slides">
                <div class="slide">
                  <img src="@/assets/images/low/1low/gray-white-black.png" alt="Color 1" />
                </div>
              </div>
            </div>
            <div class="thumbs">
              <label for="1low-1" class="thumb">
                <img src="@/assets/images/low/1low/gray-white-black.png" alt="1" />
              </label>
              <label for="1low-2" class="thumb">
                <img src="@/assets/images/low/1low/white.png" alt="2" />
              </label>
              <label for="1low-3" class="thumb">
                <img src="@/assets/images/low/1low/white-blue-light blue.png" alt="3" />
              </label>
            </div>
            <div class="card-content">
              <h3>Air Jordan 1 Low</h3>
              <p>₡65,000</p>
            </div>
          </router-link>
        </div>

        <div class="card">
          <router-link
            :to="{ name: 'product', params: { id: 'air-jordan-1-low-g' }, query: { image: low2Main } }"
            class="product-detail card-link"
          >
            <div class="color-slider">
              <div class="slides">
                <div class="slide">
                  <img src="@/assets/images/low/1lowG/gray.png" alt="Color 1" />
                </div>
              </div>
            </div>
            <div class="thumbs">
              <label for="1lowG-1" class="thumb">
                <img src="@/assets/images/low/1lowG/gray.png" alt="1" />
              </label>
              <label for="1lowG-2" class="thumb">
                <img src="@/assets/images/low/1lowG/light brown.png" alt="2" />
              </label>
              <label for="1lowG-3" class="thumb">
                <img src="@/assets/images/low/1lowG/neon-white.png" alt="3" />
              </label>
            </div>
            <div class="card-content">
              <h3>Air Jordan 1 Low G</h3>
              <p>₡75,000</p>
            </div>
          </router-link>
        </div>

        <div class="card">
          <router-link
            :to="{ name: 'product', params: { id: 'air-jordan-1-low-se' }, query: { image: low3Main } }"
            class="product-detail card-link"
          >
            <div class="color-slider">
              <div class="slides">
                <div class="slide">
                  <img src="@/assets/images/low/1lowSE/black.png" alt="Color 1" />
                </div>
              </div>
            </div>
            <div class="thumbs">
              <label for="1lowSE-1" class="thumb">
                <img src="@/assets/images/low/1lowSE/black.png" alt="1" />
              </label>
              <label for="1lowSE-2" class="thumb">
                <img src="@/assets/images/low/1lowSE/brown.png" alt="2" />
              </label>
              <label for="1lowSE-3" class="thumb">
                <img src="@/assets/images/low/1lowSE/wine.png" alt="3" />
              </label>
            </div>
            <div class="card-content">
              <h3>Air Jordan 1 Low SE</h3>
              <p>₡70,000</p>
            </div>
          </router-link>
        </div>

        <div class="card">
          <router-link
            :to="{ name: 'product', params: { id: 'air-jordan-1-skyline-low' }, query: { image: low4Main } }"
            class="product-detail card-link"
          >
            <div class="color-slider">
              <div class="slides">
                <div class="slide">
                  <img src="@/assets/images/low/skylineLow/red-white-gray.png" alt="Color 1" />
                </div>
              </div>
            </div>
            <div class="thumbs">
              <label for="skylineLow-1" class="thumb">
                <img src="@/assets/images/low/skylineLow/red-white-gray.png" alt="1" />
              </label>
              <label for="skylineLow-2" class="thumb">
                <img src="@/assets/images/low/skylineLow/white-black-gray.png" alt="2" />
              </label>
            </div>
            <div class="card-content">
              <h3>Air Jordan 1 Skyline Low</h3>
              <p>₡90,000</p>
            </div>
          </router-link>
        </div>
      </div>
    </section>
  </main>
</template>
