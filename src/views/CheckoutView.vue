<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '@/stores/cart'
import { useAuthStore } from '@/stores/auth'
import { purchaseService } from '@/services/purchaseService'

const router = useRouter()
const cart = useCartStore()
const auth = useAuthStore()

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

const cvcError = ref('')
const submitAttempted = ref(false)
const touched = reactive({
  name: false,
  cardNumber: false,
  expiry: false,
  cvc: false,
})
const focused = reactive({
  name: false,
  cardNumber: false,
  expiry: false,
  cvc: false,
})

const namePattern = /^[\p{L}\s'-]+$/u

function onlyDigits(value: string) {
  return value.replace(/\D/g, '')
}

function onCardInput(e: Event) {
  const el = e.target as HTMLInputElement
  const digits = onlyDigits(el.value || '').slice(0, 16)
  payment.cardNumber = digits.replace(/(.{4})/g, '$1 ').trim()
}

function onNameInput(e: Event) {
  const el = e.target as HTMLInputElement
  payment.name = el.value || ''
}

function onExpiryInput(e: Event) {
  const el = e.target as HTMLInputElement
  const digits = onlyDigits(el.value).slice(0, 4)
  if (digits.length <= 2) {
    payment.expiry = digits
    return
  }

  payment.expiry = `${digits.slice(0, 2)}/${digits.slice(2)}`
}

function onCvcInput(e: Event) {
  const el = e.target as HTMLInputElement
  const rawValue = el.value || ''
  const digits = onlyDigits(rawValue).slice(0, 4)
  payment.cvc = rawValue

  if (!rawValue.trim()) {
    cvcError.value = ''
    return
  }

  if (/\D/.test(rawValue)) {
    cvcError.value = 'Ingresa un código CVC válido.'
    return
  }

  cvcError.value = digits.length === 3 || digits.length === 4 ? '' : 'Ingresa un código CVC válido.'
}

function markTouched(field: keyof typeof touched) {
  touched[field] = true
}

function markFocused(field: keyof typeof focused) {
  focused[field] = true
}

function markBlurred(field: keyof typeof focused) {
  focused[field] = false
  markTouched(field)
}

function normalizeExpiry(value: string) {
  return value.trim()
}

function isExpiryValid(value: string) {
  const match = normalizeExpiry(value).match(/^(\d{2})\/(\d{2})$/)
  if (!match) return false

  const month = Number(match[1])
  const year = Number(match[2])
  if (month < 1 || month > 12) return false

  const current = new Date()
  const currentMonth = current.getMonth() + 1
  const currentYear = current.getFullYear() % 100

  return year > currentYear || (year === currentYear && month >= currentMonth)
}

function getExpiryError(value: string) {
  const normalized = normalizeExpiry(value)
  if (!normalized) return ''

  const match = normalized.match(/^(\d{2})\/(\d{2})$/)
  if (!match) return 'Ingresa una fecha válida en formato MM/AA.'

  const month = Number(match[1])
  if (month < 1 || month > 12) {
    return 'Ingresa una fecha válida en formato MM/AA.'
  }

  return isExpiryValid(normalized) ? '' : 'La tarjeta está vencida. Revisa la fecha de vencimiento.'
}

function isCardNumberValid(value: string) {
  return onlyDigits(value).length === 16
}

function isNameValid(value: string) {
  const normalized = value.trim()
  return normalized.length >= 3 && namePattern.test(normalized)
}

function isCvcValid(value: string) {
  const digits = onlyDigits(value)
  return digits.length === 3 || digits.length === 4
}

const expiryIsValid = computed(() => isExpiryValid(payment.expiry))
const cardNumberIsValid = computed(() => isCardNumberValid(payment.cardNumber))
const nameIsValid = computed(() => isNameValid(payment.name))
const cvcIsValid = computed(() => isCvcValid(payment.cvc))
const canPay = computed(
  () => cart.items.length > 0 && nameIsValid.value && cardNumberIsValid.value && expiryIsValid.value && cvcIsValid.value,
)

function pay() {
  submitAttempted.value = true
  touched.name = true
  touched.cardNumber = true
  touched.expiry = true
  touched.cvc = true

  if (!auth.user) {
    window.alert('Debes iniciar sesión para completar la compra.')
    router.push({ name: 'login' })
    return
  }

  if (!canPay.value) return

  const paidSubtotal = subtotal.value
  const paidShipping = shipping.value
  const paidTotal = total.value

  purchaseService.createPurchase({
    userId: auth.user.id,
    items: cart.items,
    subtotal: paidSubtotal,
    shipping: paidShipping,
    total: paidTotal,
  })

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
            <input
              :value="payment.name"
              autocomplete="cc-name"
              inputmode="text"
              placeholder="Juan Pérez"
              required
              type="text"
              @input="onNameInput"
              @focus="markFocused('name')"
              @blur="markBlurred('name')"
            />
            <small v-if="(touched.name || submitAttempted) && !focused.name && !nameIsValid" class="field-error">Ingresa solo letras para el nombre.</small>
          </label>

          <label>
            Número de tarjeta
            <input
              :value="payment.cardNumber"
              autocomplete="cc-number"
              inputmode="numeric"
              maxlength="19"
              placeholder="1234 5678 9012 3456"
              required
              type="text"
              @input="onCardInput"
              @focus="markFocused('cardNumber')"
              @blur="markBlurred('cardNumber')"
            />
            <small v-if="(touched.cardNumber || submitAttempted) && !focused.cardNumber && !cardNumberIsValid" class="field-error">Ingresa un número de tarjeta de 16 dígitos.</small>
          </label>

          <div class="two-cols">
            <label>
              Vencimiento
              <input
                :value="payment.expiry"
                autocomplete="cc-exp"
                inputmode="numeric"
                maxlength="5"
                placeholder="MM/AA"
                required
                type="text"
                @input="onExpiryInput"
                @focus="markFocused('expiry')"
                @blur="markBlurred('expiry')"
              />
              <small v-if="(touched.expiry || submitAttempted) && !focused.expiry && !expiryIsValid" class="field-error">{{ getExpiryError(payment.expiry) }}</small>
            </label>

            <label>
              CVC
              <input
                :value="payment.cvc"
                autocomplete="cc-csc"
                inputmode="numeric"
                maxlength="4"
                placeholder="123"
                required
                type="password"
                @input="onCvcInput"
                @focus="markFocused('cvc')"
                @blur="markBlurred('cvc')"
              />
              <small v-if="(touched.cvc || submitAttempted) && !focused.cvc && cvcError" class="field-error">{{ cvcError }}</small>
              <small v-else-if="(touched.cvc || submitAttempted) && !focused.cvc && payment.cvc && !cvcIsValid" class="field-error">Ingresa un CVC de 3 o 4 dígitos.</small>
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
