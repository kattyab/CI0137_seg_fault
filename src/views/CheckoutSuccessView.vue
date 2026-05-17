<script setup lang="ts">
import { computed } from 'vue'
import { RouterLink, useRoute } from 'vue-router'

const route = useRoute()

const formatCRC = (amount: number) => `₡${amount.toLocaleString('es-CR')}`
const total = computed(() => Number(route.query.total ?? 0))
</script>

<template>
  <main class="success-page">
    <section class="success-card">
      <div class="success-icon">✓</div>
      <h1>Pago confirmado</h1>
      <p>Tu compra se realizó correctamente.</p>
      <div class="success-summary">
        <span>Monto pagado</span>
        <strong>{{ formatCRC(total) }}</strong>
      </div>
      <RouterLink to="/" class="back-btn">Volver al inicio</RouterLink>
    </section>
  </main>
</template>

<style scoped>
.success-page {
  min-height: calc(100vh - 80px);
  display: grid;
  place-items: center;
  padding: 2rem 1rem;
  background: linear-gradient(180deg, #f4fff6 0%, #ffffff 100%);
}

.success-card {
  width: min(100%, 560px);
  background: white;
  border-radius: 20px;
  padding: 2.5rem 2rem;
  text-align: center;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.08);
}

.success-icon {
  width: 80px;
  height: 80px;
  margin: 0 auto 1rem;
  border-radius: 50%;
  display: grid;
  place-items: center;
  background: #16a34a;
  color: white;
  font-size: 2rem;
  font-weight: 900;
}

.success-card h1 {
  margin: 0 0 0.5rem;
  font-size: 2rem;
}

.success-card p {
  margin: 0 0 1.5rem;
  color: #555;
}

.success-summary {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #f7f7f7;
  border-radius: 12px;
  padding: 1rem 1.25rem;
  margin-bottom: 1.5rem;
}

.back-btn {
  display: inline-block;
  background: #111;
  color: white;
  text-decoration: none;
  padding: 0.95rem 1.25rem;
  border-radius: 12px;
  font-weight: 800;
}
</style>
