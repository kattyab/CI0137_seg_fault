<template>
  <button class="add-to-cart" @click="add" :disabled="isDisabled">
    <span v-if="added">Añadido ✓</span>
    <span v-else-if="props.requireSize && !props.size">Selecciona talla</span>
    <span v-else>Añadir al carrito</span>
  </button>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useCartStore, type CartItem } from '@/stores/cart'

const props = defineProps<{
  id: string
  nombre: string
  precio: number
  quantity?: number
  size?: string | null
  color?: string | null
  image?: string | null
  inventoryId?: number
  stock?: number
  requireSize?: boolean
}>()

const emit = defineEmits(['added'])
const cart = useCartStore()
const added = ref(false)
const adding = ref(false)
const isDisabled = computed(() => adding.value || (!!props.requireSize && !props.size))

function add() {
  if (isDisabled.value) return

  adding.value = true

  const item: CartItem = {
    id: props.id,
    nombre: props.nombre,
    precio: props.precio,
    quantity: props.quantity ?? 1
  }

  if (props.size) item.size = props.size
  if (props.color) item.color = props.color
  if (props.image) item.image = props.image
  if (props.inventoryId != null) item.inventoryId = props.inventoryId
  if (props.stock != null) item.stock = props.stock

  cart.addItem(item)

  emit('added', item)

  added.value = true

  setTimeout(() => {
    added.value = false
    adding.value = false
  }, 1200)
}
</script>

<style scoped>
.add-to-cart {
  background: #111;
  color: white;
  border: none;
  padding: 0.95rem 1.1rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 800;
  font-size: 1rem;
  width: 100%;
  box-shadow: 0 10px 28px rgba(0, 0, 0, 0.14);
}

.add-to-cart,
.add-to-cart span {
  color: white !important
}

.add-to-cart[disabled] {
  opacity: 0.7;
  cursor: default
}

.add-to-cart:hover {
  background: #000
}
.add-to-cart, .add-to-cart span { color: white !important }
.add-to-cart[disabled] { opacity: 0.7; cursor: not-allowed }
.add-to-cart:hover { background: #000 }
</style>
