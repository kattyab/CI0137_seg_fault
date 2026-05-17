<template>
  <button class="cart-button" @click="$emit('toggle')">
    🛒 <span v-if="totalItems">{{ totalItems }}</span>
  </button>
</template>

<script setup lang="ts">
import { useCartStore } from '@/stores/cart'

const cart = useCartStore()
const totalItems = cart.totalItems
</script>

<style scoped>
.cart-button {
  background: transparent;
  border: none;
  cursor: pointer;
  font-size: 1rem;
}
</style>
