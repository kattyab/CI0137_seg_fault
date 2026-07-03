<template>
  <div class="modal-overlay" @click="$emit('close')">
    <div class="cart-modal" @click.stop>
      <div class="modal-header">
        <div class="success-icon">✓</div>

        <h3>Producto agregado</h3>

        <button class="close-btn" @click="$emit('close')">
          ✕
        </button>
      </div>

      <div class="product-info">
        <img :src="product.image" :alt="product.nombre" />

        <div>
          <h4>{{ product.nombre }}</h4>

          <p v-if="product.color">
            Color: {{ product.color }}
          </p>

          <p v-if="product.size">
            Talla: {{ product.size }}
          </p>

          <p class="price">
            ₡{{ product.precio.toLocaleString('es-CR') }}
          </p>
        </div>
      </div>

      <div class="actions">
        <RouterLink
          to="/carrito"
          class="view-cart-btn"
          @click="$emit('close')"
        >
          Ver carrito
        </RouterLink>

        <RouterLink
          to="/checkout"
          class="checkout-btn"
          @click="$emit('close')"
        >
          Finalizar compra
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'

defineEmits(['close'])

defineProps<{
  product: {
    nombre: string
    precio: number
    image?: string
    size?: string
    color?: string
  }
}>()
</script>

<style scoped>
.cart-modal {
  width: 420px;
  max-width: 90vw;
  background: white;
  border-radius: 18px;
  padding: 1.5rem;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.25);
}

.modal-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.modal-header h3 {
  margin: 0;
  font-size: 1.2rem;
}

.success-icon {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: #16a34a;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
}

.close-btn {
  margin-left: auto;
  border: none;
  background: transparent;
  cursor: pointer;
  font-size: 1.2rem;
}

.product-info {
  display: flex;
  gap: 1rem;
  margin: 1.5rem 0;
  align-items: center;
}

.product-info img {
  width: 100px;
  height: 100px;
  object-fit: contain;
}

.product-info h4 {
  margin: 0 0 0.5rem;
}

.price {
  font-weight: 700;
  margin-top: 0.5rem;
}

.actions {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.view-cart-btn,
.checkout-btn {
  text-align: center;
  padding: 0.9rem;
  border-radius: 999px;
  text-decoration: none;
  font-weight: 700;
  transition: 0.2s ease;
}

.view-cart-btn {
  border: 1px solid #ddd;
  color: black;
}

.view-cart-btn:hover {
  background: #f5f5f5;
}

.checkout-btn {
  background: black;
  color: white;
}

.checkout-btn:hover {
  background: #222;
}
</style>
