<script setup lang="ts">
import { computed, ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useCartStore } from '@/stores/cart'
import { useAuthStore } from '@/stores/auth'

const cart = useCartStore()
const auth = useAuthStore()
const router = useRouter()

const isModalVisible = ref(false)
const pendingItemId = ref<string | null>(null)
const removeProductName = ref('este producto')

const formatCRC = (amount: number) => `₡${amount.toLocaleString('es-CR')}`

const subtotal = computed(() => cart.subtotal)
const shipping = computed(() => (subtotal.value > 0 ? 10000 : 0))
const total = computed(() => subtotal.value + shipping.value)

const openModal = (itemId: string, productName: string) => {
  removeProductName.value = productName
  pendingItemId.value = itemId
  isModalVisible.value = true
}

const closeModal = () => {
  isModalVisible.value = false
  pendingItemId.value = null
}

const confirmRemove = () => {
  if (pendingItemId.value === null) {
    return
  }
  cart.removeItem(pendingItemId.value)
  closeModal()
}

const closeIfOverlay = (event: MouseEvent) => {
  if (event.target === event.currentTarget) {
    closeModal()
  }
}

const goToCheckout = () => {
  if (cart.items.length === 0) return
  router.push({ name: 'checkout' })
}
</script>

<template>
  <main>
    <section class="categorias">
      <h2>Tu Carrito de Compras</h2>
      <div style="max-width: 800px; margin: 0 auto">
        <table style="width: 100%; border-collapse: collapse; margin-bottom: 2rem">
          <thead>
            <tr style="background: #f0f0f0; border-bottom: 2px solid #ff6b35">
              <th style="padding: 1rem; text-align: left">Producto</th>
              <th style="padding: 1rem; text-align: center">Cantidad</th>
              <th style="padding: 1rem; text-align: right">Precio Unitario</th>
              <th style="padding: 1rem; text-align: right">Importe</th>
              <th style="padding: 1rem; text-align: center">Quitar</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in cart.items" :key="item._key ?? item.id" style="border-bottom: 1px solid #ddd">
              <td style="padding: 1rem">{{ item.nombre }}<span v-if="item.size"> - {{ item.size }}</span></td>
              <td style="padding: 1rem; text-align: center">{{ item.quantity }}</td>
              <td style="padding: 1rem; text-align: right">{{ formatCRC(item.precio) }}</td>
              <td style="padding: 1rem; text-align: right">
                {{ formatCRC(item.precio * item.quantity) }}
              </td>
              <td style="padding: 1rem; text-align: center">
                <button
                  class="remove-btn"
                  type="button"
                  :aria-label="`Eliminar ${item.nombre}`"
                  @click="openModal(item._key ?? item.id, item.nombre)"
                >
                  ×
                </button>
              </td>
            </tr>
            <tr v-if="cart.items.length === 0">
              <td colspan="5" style="padding: 1rem; text-align: center">
                No hay productos en el carrito.
              </td>
            </tr>
          </tbody>
        </table>

        <div
          style="
            background: #fff;
            padding: 1.5rem;
            border-radius: 10px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
            margin-bottom: 2rem;
          "
        >
          <div
            style="
              display: flex;
              justify-content: space-between;
              margin-bottom: 1rem;
              font-size: 1.1rem;
            "
          >
            <span>Subtotal:</span>
            <span id="cartSubtotal">{{ formatCRC(subtotal) }}</span>
          </div>
          <div
            style="
              display: flex;
              justify-content: space-between;
              margin-bottom: 1rem;
              font-size: 1.1rem;
            "
          >
            <span>Envío:</span>
            <span id="cartShipping">{{ formatCRC(shipping) }}</span>
          </div>
          <div
            style="
              border-top: 2px solid #ddd;
              padding-top: 1rem;
              display: flex;
              justify-content: space-between;
              font-size: 1.3rem;
              font-weight: bold;
              color: #ff6b35;
            "
          >
            <span>Total:</span>
            <span id="cartTotal">{{ formatCRC(total) }}</span>
          </div>
        </div>

        <div class="cart-actions" style="display: flex; gap: 1rem">
          <RouterLink to="/" class="cta cart-action-link">Continuar Comprando</RouterLink>
          <button class="cta cta-buy cart-action-btn" type="button" :disabled="cart.items.length === 0" @click="goToCheckout">Proceder al Pago</button>
        </div>
      </div>
    </section>

    <div
      :class="['modal-overlay', { 'is-visible': isModalVisible }]"
      id="removeModal"
      :aria-hidden="!isModalVisible"
      @click="closeIfOverlay"
    >
      <div class="modal" role="dialog" aria-modal="true" aria-labelledby="removeModalTitle">
        <h3 id="removeModalTitle">Eliminar producto</h3>
        <p>
          ¿Seguro que deseas eliminar
          <strong id="removeProductName">{{ removeProductName }}</strong> del carrito?
        </p>
        <div class="modal-actions">
          <button
            class="cta"
            id="confirmRemove"
            type="button"
            style="font-size: 1rem; padding: 1rem 1.75rem; min-width: 140px"
            @click="confirmRemove"
          >
            Eliminar
          </button>
          <button
            class="ghost-btn"
            id="cancelRemove"
            type="button"
            style="font-size: 1rem; padding: 1rem 1.75rem; min-width: 140px"
            @click="closeModal"
          >
            Cancelar
          </button>
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
.modal-actions button {
  min-width: 140px;
  padding: 1rem 1.75rem;
  font-size: 1rem;
  line-height: 1;
  box-sizing: border-box;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

#confirmRemove {
  margin-top: 0;
}

#cancelRemove {
  margin-top: 0;
}
</style>
