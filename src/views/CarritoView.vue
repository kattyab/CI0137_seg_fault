<script setup lang="ts">
import { computed, ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useCartStore, type CartItem } from '@/stores/cart'
import { useAuthStore } from '@/stores/auth'
import { productImage } from '@/services/productImages'

const cart = useCartStore()
const auth = useAuthStore()
const router = useRouter()

const isModalVisible = ref(false)
const pendingItemId = ref<string | null>(null)
const pendingQty = ref(0)
const removeProductName = ref('este producto')

const formatCRC = (amount: number) => `₡${amount.toLocaleString('es-CR')}`

const subtotal = computed(() => cart.subtotal)
const shipping = computed(() => (subtotal.value > 0 ? 10000 : 0))
const total = computed(() => subtotal.value + shipping.value)

const maxFor = (item: CartItem) => (typeof item.stock === 'number' ? item.stock : 99)

const onQtyInput = (item: CartItem, event: Event) => {
  const input = event.target as HTMLInputElement
  const parsed = Number.parseInt(input.value, 10)
  cart.setQuantity(item._key ?? item.id, Number.isFinite(parsed) ? parsed : 1)
  // reflect the clamped value back into the input
  input.value = String(item.quantity)
}

const openModal = (item: CartItem) => {
  removeProductName.value = item.nombre
  pendingItemId.value = item._key ?? item.id
  pendingQty.value = item.quantity
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
  cart.removeLine(pendingItemId.value)
  closeModal()
}

const closeIfOverlay = (event: MouseEvent) => {
  if (event.target === event.currentTarget) {
    closeModal()
  }
}

const goToCheckout = () => {
  if (cart.items.length === 0) return
  if (auth.user) {
    router.push({ name: 'checkout' })
  } else {
    router.push({ name: 'login' })
  }
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
              <th style="padding: 1rem; text-align: left">Foto</th>
              <th style="padding: 1rem; text-align: left">Producto</th>
              <th style="padding: 1rem; text-align: center">Cantidad</th>
              <th style="padding: 1rem; text-align: right">Precio Unitario</th>
              <th style="padding: 1rem; text-align: right">Importe</th>
              <th style="padding: 1rem; text-align: center">Eliminar</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in cart.items" :key="item._key ?? item.id" style="border-bottom: 1px solid #ddd">
              <td style="padding: 0.5rem; width: 80px; text-align: center">
                <img :src="item.image || productImage(item.nombre, item.color)" :alt="item.nombre" style="width:64px; height:64px; object-fit:contain; display:block; margin:0 auto;" />
              </td>
              <td style="padding: 1rem">
                {{ item.nombre }}
                <span v-if="item.size"> - {{ item.size }}</span>
                <span v-if="item.color"> - {{ item.color }}</span>
              </td>
              <td style="padding: 1rem; text-align: center">
                <div class="qty-control">
                  <button
                    type="button"
                    class="qty-btn"
                    :disabled="item.quantity <= 1"
                    :aria-label="`Reducir cantidad de ${item.nombre}`"
                    @click="cart.setQuantity(item._key ?? item.id, item.quantity - 1)"
                  >
                    −
                  </button>
                  <input
                    type="number"
                    class="qty-input"
                    min="1"
                    :max="maxFor(item)"
                    :value="item.quantity"
                    :aria-label="`Cantidad de ${item.nombre}`"
                    @change="onQtyInput(item, $event)"
                  />
                  <button
                    type="button"
                    class="qty-btn"
                    :disabled="item.quantity >= maxFor(item)"
                    :aria-label="`Aumentar cantidad de ${item.nombre}`"
                    @click="cart.setQuantity(item._key ?? item.id, item.quantity + 1)"
                  >
                    +
                  </button>
                </div>
              </td>
              <td style="padding: 1rem; text-align: right">{{ formatCRC(item.precio) }}</td>
              <td style="padding: 1rem; text-align: right">
                {{ formatCRC(item.precio * item.quantity) }}
              </td>
              <td style="padding: 1rem; text-align: center">
                <button
                  class="remove-btn"
                  type="button"
                  :aria-label="`Eliminar todas las unidades de ${item.nombre}`"
                  @click="openModal(item)"
                >
                  ×
                </button>
              </td>
            </tr>
            <tr v-if="cart.items.length === 0">
              <td colspan="6" style="padding: 1rem; text-align: center">
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
          <button
            class="cta cta-buy cart-action-btn"
            type="button"
            :disabled="cart.items.length === 0"
            @click="goToCheckout()"
          >
            <span v-if="!auth.user">Inicia sesión</span>
            <span v-else>Proceder al Pago</span>
          </button>
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
        <p v-if="pendingQty > 1">
          ¿Seguro que deseas eliminar las
          <strong>{{ pendingQty }} unidades</strong> de
          <strong id="removeProductName">{{ removeProductName }}</strong> del carrito?
        </p>
        <p v-else>
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
.qty-control {
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
}

.qty-btn {
  width: 28px;
  height: 28px;
  border: 1px solid #ddd;
  background: #fff;
  border-radius: 6px;
  font-size: 1.1rem;
  line-height: 1;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.qty-btn:hover:not(:disabled) {
  border-color: #ff6b35;
  color: #ff6b35;
}

.qty-input {
  width: 3.5rem;
  padding: 0.35rem 0.25rem;
  text-align: center;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  -moz-appearance: textfield;
  appearance: textfield;
}

.qty-input::-webkit-outer-spin-button,
.qty-input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

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
  background: var(--main-text-color);
  color: var(--card-and-section-background);
  border: none;
}

button:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}
</style>
