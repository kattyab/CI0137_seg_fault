<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { getOrdersByUser, type OrderHistory } from '@/services/orderService'
import { productImage } from '@/services/productImages'

const auth = useAuthStore()

const user = computed(() => auth.user)

const createdAtText = computed(() => {
  if (!auth.user?.createdDate) {
    return 'No disponible'
  }

  return new Date(auth.user.createdDate).toLocaleDateString('es-CR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
})

const showPurchaseHistory = ref(false)

const togglePurchaseHistory = () => {
  showPurchaseHistory.value = !showPurchaseHistory.value
}

const orders = ref<OrderHistory[]>([])
const ordersLoading = ref(false)
const ordersError = ref('')

async function loadOrders() {
  if (!auth.user) return

  ordersLoading.value = true
  ordersError.value = ''

  try {
    orders.value = await getOrdersByUser(auth.user.id)
  } catch {
    ordersError.value = 'No se pudo cargar el historial de compras. Intenta de nuevo más tarde.'
  } finally {
    ordersLoading.value = false
  }
}

onMounted(loadOrders)

const orderSubtotal = (order: OrderHistory) =>
  order.items.reduce((sum, item) => sum + item.precioUnit * item.cantidad, 0)

const formatCRC = (amount: number) => `₡${amount.toLocaleString('es-CR')}`

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('es-CR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
}
</script>

<template>
  <main>
    <section class="profile-section">
      <div v-if="user" class="profile-card">
        <div class="profile-header">
          <div class="profile-avatar">
            {{ user.nombre.charAt(0).toUpperCase() }}
          </div>

          <div>
            <h2>Mi perfil</h2>
            <p class="profile-subtitle">
              Información de tu cuenta registrada.
            </p>
          </div>
        </div>

        <div class="profile-grid">
          <div class="profile-item">
            <span class="profile-label">Nombre completo</span>
            <span class="profile-value">{{ user.nombre }}</span>
          </div>

          <div class="profile-item">
            <span class="profile-label">Correo electrónico</span>
            <span class="profile-value">{{ user.email }}</span>
          </div>

          <div class="profile-item">
            <span class="profile-label">Teléfono</span>
            <span class="profile-value">
              {{ user.telefono || 'No registrado' }}
            </span>
          </div>

          <div class="profile-item">
            <span class="profile-label">Fecha de registro</span>
            <span class="profile-value">{{ createdAtText }}</span>
          </div>
        </div>

        <section class="purchase-history">
          <button
            type="button"
            class="purchase-history-toggle"
            :aria-expanded="showPurchaseHistory"
            aria-controls="purchase-history-content"
            @click="togglePurchaseHistory"
          >
            <span>Historial de compras</span>

            <img
              src="@/assets/images/user/arrow-up.png"
              alt=""
              aria-hidden="true"
              class="purchase-history-arrow"
              :class="{ 'is-open': showPurchaseHistory }"
            />
          </button>

          <div
            v-show="showPurchaseHistory"
            id="purchase-history-content"
            class="purchase-history-content"
          >
            <div v-if="ordersLoading" class="purchase-empty">
              <p>Cargando historial…</p>
            </div>

            <div v-else-if="ordersError" class="purchase-empty">
              <p>{{ ordersError }}</p>
            </div>

            <div v-else-if="orders.length === 0" class="purchase-empty">
              <p>Aún no tienes compras registradas.</p>
              <p>Cuando completes una compra, aparecerá aquí.</p>
            </div>

            <div v-else class="purchase-list">
              <article
                v-for="order in orders"
                :key="order.id"
                class="purchase-card"
              >
                <div class="purchase-header">
                  <div>
                    <h4>Compra #{{ order.id.replace('ord_', '') }}</h4>
                    <p>{{ formatDate(order.fecha) }}</p>
                  </div>

                  <span class="purchase-status">
                    {{ order.estado }}
                  </span>
                </div>

                <div class="purchase-products">
                  <div
                    v-for="(item, idx) in order.items"
                    :key="`${order.id}-${idx}`"
                    class="purchase-product"
                  >
                    <img
                      :src="productImage(item.nombre, item.color, item.imagenUrl)"
                      :alt="item.nombre"
                      class="purchase-product-image"
                    />

                    <div>
                      <strong>{{ item.nombre }}</strong>
                      <p>Talla: {{ item.talla }}</p>
                      <p v-if="item.color">Color: {{ item.color }}</p>
                      <p>Cantidad: {{ item.cantidad }}</p>
                      <p>Precio unitario: {{ formatCRC(item.precioUnit) }}</p>
                      <p>Importe: {{ formatCRC(item.precioUnit * item.cantidad) }}</p>
                    </div>
                  </div>
                </div>

                <div class="purchase-total">
                  <span>Subtotal:</span>
                  <strong>{{ formatCRC(orderSubtotal(order)) }}</strong>
                </div>

                <div class="purchase-total">
                  <span>Envío:</span>
                  <strong>{{ formatCRC(order.total - orderSubtotal(order)) }}</strong>
                </div>

                <div class="purchase-total purchase-total-final">
                  <span>Total:</span>
                  <strong>{{ formatCRC(order.total) }}</strong>
                </div>
              </article>
            </div>
          </div>
        </section>
      </div>

      <div v-else class="profile-card profile-empty">
        <h2>No has iniciado sesión</h2>
        <p>Para ver tu perfil, primero debes ingresar a tu cuenta.</p>

        <RouterLink to="/login" class="profile-primary-link">
          Iniciar sesión
        </RouterLink>
      </div>
    </section>
  </main>
</template>
