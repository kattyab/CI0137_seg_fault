<template>
  <button class="cart-button" @click="$emit('toggle')">
    <span class="cart-icon">
      🛒
      <span v-if="totalItems > 0" class="cart-badge">
        {{ totalItems }}
      </span>
    </span>
  </button>
</template>

<script setup lang="ts">
import { useCartStore } from '@/stores/cart'
import { storeToRefs } from 'pinia'

const cart = useCartStore()
const { totalItems } = storeToRefs(cart)
</script>

<style scoped>
.cart-button {
  background: transparent;
  border: none;
  cursor: pointer;
  position: relative;
}

.cart-icon {
  position: relative;
  font-size: 1.8rem;
  display: inline-block;
}

.cart-badge {
  position: absolute;
  top: -8px;
  right: -10px;
  background: red;
  color: white;
  border-radius: 50%;
  min-width: 18px;
  height: 18px;
  font-size: 12px;
  font-weight: bold;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
