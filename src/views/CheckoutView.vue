<script setup lang="ts">
import { computed, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '@/stores/cart'

const router = useRouter()
const cart = useCartStore()

const formatCRC = (amount: number) => `₡${amount.toLocaleString('es-CR')}`

const subtotal = computed(() => cart.subtotal)
const shipping = computed(() => (subtotal.value > 0 ? 10000 : 0))
const total = computed(() => subtotal.value + shipping.value)

const payment = reactive({
  name: '',
  cardNumber: '',
  expiry: '',
  cvc: '',
})

function onCardInput(e: Event) {
  const el = e.target as HTMLInputElement
  let digits = (el.value || '').replace(/\D/g, '')
  digits = digits.slice(0, 16)
  // format as groups of 4
  payment.cardNumber = digits.replace(/(.{4})/g, '$1 ').trim()
}

function isExpiryValid(value: string) {
  const match = value.trim().match(/^(\d{2})\/(\d{2})$/)
  if (!match) return false

  const month = Number(match[1])
  const year = Number(match[2])
  if (month < 1 || month > 12) return false

  const current = new Date()
  const currentMonth = current.getMonth() + 1
  const currentYear = current.getFullYear() % 100

  return year > currentYear || (year === currentYear && month >= currentMonth)
}

const expiryIsValid = computed(() => isExpiryValid(payment.expiry))
const canPay = computed(() => cart.items.length > 0 && expiryIsValid.value)

function pay() {
  if (!canPay.value) return
  const paidTotal = total.value
  cart.clearCart()
  router.push({
    name: 'checkout-confirmado',
    query: {
      total: String(paidTotal),
    },
  })
}
</script>

<template>
  <main class="checkout-page">
    <section class="checkout-card">
      <div class="checkout-header">
        <h1>Pago</h1>
        <p>Ingresa los datos de tu tarjeta para completar la compra.</p>
      </div>

      <div class="checkout-grid">
        <form class="payment-form" @submit.prevent="pay">
          <label>
            Nombre en la tarjeta
            <input v-model="payment.name" type="text" placeholder="Juan Pérez" required />
          </label>

          <label>
            Número de tarjeta
            <input :value="payment.cardNumber" @input="onCardInput" type="text" inputmode="numeric" maxlength="19" placeholder="1234 5678 9012 3456" required />
          </label>

          <div class="two-cols">
            <label>
              Vencimiento
              <input v-model="payment.expiry" type="text" placeholder="MM/AA" maxlength="5" required />
              <small v-if="payment.expiry && !expiryIsValid" class="field-error">Inserte una tarjeta válida.</small>
            </label>

            <label>
              CVC
              <input v-model="payment.cvc" type="password" inputmode="numeric" maxlength="4" placeholder="123" required />
            </label>
          </div>

          <button class="pay-btn" type="submit" :disabled="!canPay">
            Pagar {{ formatCRC(total) }}
          </button>
        </form>

        <aside class="summary-card">
          <h2>Resumen</h2>
          <div class="summary-row">
            <span>Subtotal</span>
            <strong>{{ formatCRC(subtotal) }}</strong>
          </div>
          <div class="summary-row">
            <span>Envío</span>
            <strong>{{ formatCRC(shipping) }}</strong>
          </div>
          <div class="summary-row total">
            <span>Total</span>
            <strong>{{ formatCRC(total) }}</strong>
          </div>
        </aside>
      </div>
    </section>
  </main>
</template>

<style scoped>
.checkout-page {
  min-height: calc(100vh - 80px);
  padding: 2rem 1rem;
  background: linear-gradient(180deg, #fff7f2 0%, #ffffff 100%);
}

.checkout-card {
  max-width: 980px;
  margin: 0 auto;
  background: white;
  border-radius: 20px;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.08);
  padding: 2rem;
}

.checkout-header h1 {
  margin: 0 0 0.5rem;
  font-size: 2rem;
}

.checkout-header p {
  margin: 0;
  color: #666;
}

.checkout-grid {
  display: grid;
  grid-template-columns: minmax(0, 1.2fr) minmax(280px, 360px);
  gap: 1.5rem;
  margin-top: 1.5rem;
}

.payment-form {
  display: grid;
  gap: 1rem;
}

.payment-form label {
  display: grid;
  gap: 0.45rem;
  font-weight: 700;
  color: #222;
}

.payment-form input {
  padding: 0.95rem 1rem;
  border: 1px solid #ddd;
  border-radius: 12px;
  font-size: 1rem;
}

.field-error {
  color: #c62828;
  font-weight: 600;
}

.two-cols {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 1rem;
}

.pay-btn {
  margin-top: 0.5rem;
  border: none;
  border-radius: 12px;
  padding: 1rem 1.25rem;
  background: #111;
  color: white;
  font-size: 1rem;
  font-weight: 800;
  cursor: pointer;
}

.pay-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.summary-card {
  background: #fff6f1;
  border: 1px solid #ffd8c7;
  border-radius: 16px;
  padding: 1.25rem;
  align-self: start;
}

.summary-card h2 {
  margin: 0 0 1rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.85rem;
  color: #333;
}

.summary-row.total {
  border-top: 1px solid #f0c8b3;
  margin-top: 0.85rem;
  padding-top: 0.85rem;
  font-size: 1.1rem;
}

@media (max-width: 820px) {
  .checkout-grid {
    grid-template-columns: 1fr;
  }
}
</style>
