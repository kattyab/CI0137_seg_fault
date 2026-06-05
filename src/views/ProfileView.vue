<script setup lang="ts">
import { computed, ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { userService } from '@/services/userService'
import { purchaseService } from '@/services/purchaseService'

const auth = useAuthStore()

const user = computed(() => auth.user)

const fullUser = computed(() => {
  if (!auth.user) {
    return undefined
  }

  return userService.getUserByEmail(auth.user.email)
})

const createdAtText = computed(() => {
  if (!fullUser.value?.createdAt) {
    return 'No disponible'
  }

  return new Date(fullUser.value.createdAt).toLocaleDateString('es-CR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
})

const showPurchaseHistory = ref(false)

const togglePurchaseHistory = () => {
  showPurchaseHistory.value = !showPurchaseHistory.value
}

const purchases = computed(() => {
  if (!auth.user) {
    return []
  }

  return purchaseService.getPurchasesByUserId(auth.user.id)
})

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
            <div v-if="purchases.length === 0" class="purchase-empty">
              <p>Aún no tienes compras registradas.</p>
              <p>Cuando completes una compra, aparecerá aquí.</p>
            </div>

            <div v-else class="purchase-list">
              <article
                v-for="purchase in purchases"
                :key="purchase.id"
                class="purchase-card"
              >
                <div class="purchase-header">
                  <div>
                    <h4>Compra #{{ purchase.id.replace('purchase_', '') }}</h4>
                    <p>{{ formatDate(purchase.createdAt) }}</p>
                  </div>

                  <span class="purchase-status">
                    {{ purchase.status }}
                  </span>
                </div>

                <div class="purchase-products">
                  <div
                    v-for="item in purchase.items"
                    :key="`${purchase.id}-${item.lineKey}`"
                    class="purchase-product"
                  >
                    <img
                      v-if="item.image"
                      :src="item.image"
                      :alt="item.nombre"
                      class="purchase-product-image"
                    />

                    <div>
                      <strong>{{ item.nombre }}</strong>
                      <p v-if="item.size">Talla: {{ item.size }}</p>
                      <p v-if="item.color">Color: {{ item.color }}</p>
                      <p>Cantidad: {{ item.quantity }}</p>
                      <p>Precio unitario: {{ formatCRC(item.precio) }}</p>
                      <p>Importe: {{ formatCRC(item.precio * item.quantity) }}</p>
                    </div>
                  </div>
                </div>

                <div class="purchase-total">
                  <span>Subtotal:</span>
                  <strong>{{ formatCRC(purchase.subtotal) }}</strong>
                </div>

                <div class="purchase-total">
                  <span>Envío:</span>
                  <strong>{{ formatCRC(purchase.shipping) }}</strong>
                </div>

                <div class="purchase-total purchase-total-final">
                  <span>Total:</span>
                  <strong>{{ formatCRC(purchase.total) }}</strong>
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
