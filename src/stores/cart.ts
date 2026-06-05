import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { useAuthStore } from './auth'

export type CartItem = { id: string; nombre: string; precio: number; quantity: number; _key?: string; [key: string]: unknown }

const STORAGE_PREFIX = 'cart:'

export const useCartStore = defineStore('cart', () => {
  const items = ref<CartItem[]>([])
  const auth = useAuthStore()

  const guestCart = computed(() => `${STORAGE_PREFIX}guest`)
  const storageKey = () => `${STORAGE_PREFIX}${auth.user?.id ?? 'guest'}`

  function load() {
    try {
      const raw = localStorage.getItem(storageKey())
      items.value = raw ? JSON.parse(raw) : []
    } catch {
      items.value = []
    }
  }

  function mergeGuestCart() {
    if (!auth.user){
      return
    }
    const guestRaw = localStorage.getItem(guestCart.value)
    const itemsGuest = guestRaw ? JSON.parse(guestRaw) : []
    if (itemsGuest.length === 0)
    {
      return
    }
    for (const item of itemsGuest) {
      addItem(item)
    }
    save()
    localStorage.removeItem(guestCart.value)
  }

  // ensure existing items have an internal composite key
  function normalizeKeys() {
    for (const it of items.value) {
      if (!it._key) it._key = compositeKeyFor(it)
    }
  }

  function save() {
    try {
      localStorage.setItem(storageKey(), JSON.stringify(items.value))
    } catch {
      // ignore
    }
  }

  // persist when items change
  watch(items, save, { deep: true })

  // reload and merge when user changes (runs immediately on init and synchronously on change)
  watch(
    () => auth.user,
    () => {
      load()
      mergeGuestCart()
      normalizeKeys()
    },
    { immediate: true, flush: 'sync' }
  )

  function compositeKeyFor(item: CartItem) {
    // build deterministic key from id plus any extra attributes (size, variant, etc.)
    const clone: Record<string, unknown> = { ...item }
    delete clone.quantity
    delete clone.nombre
    delete clone.precio
    delete clone._key
    // normalize string attributes to avoid mismatches from whitespace
    for (const k of Object.keys(clone)) {
      if (typeof clone[k] === 'string') {
        clone[k] = clone[k].replace(/\s+/g, ' ').trim()
      }
    }
    const keys = Object.keys(clone).sort((a, b) => a.localeCompare(b))
    const obj: Record<string, unknown> = {}
    for (const k of keys) obj[k] = clone[k]
    return `${item.id}::${JSON.stringify(obj)}`
  }

  function addItem(item: CartItem) {
    const key = compositeKeyFor(item)
    // prefer exact key match; compute existing keys on the fly if missing
    let idx = -1
    for (let i = 0; i < items.value.length; i++) {
      const existing = items.value[i]
      if (!existing) continue
      const existingKey = existing._key ?? compositeKeyFor(existing)
      if (existingKey === key) { idx = i; break }
    }
    if (idx === -1) {
      items.value.push({ ...item, quantity: item.quantity ?? 1, _key: key })
    } else {
      const existing = items.value[idx]!
      // merge quantities for exact same composite attributes
      existing.quantity = (existing.quantity || 0) + (item.quantity || 1)
    }
  }

  function removeItem(idOrKey: string) {
    // if idOrKey matches an internal _key, decrement quantity by 1 and remove the line when it reaches 0
    const byKey = items.value.findIndex(i => i._key === idOrKey)
    if (byKey !== -1) {
      const existing = items.value[byKey]!
      if ((existing.quantity || 0) > 1) {
        existing.quantity -= 1
      } else {
        items.value.splice(byKey, 1)
      }
      return
    }
    const byId = items.value.findIndex(i => i.id === idOrKey)
    if (byId !== -1) {
      const existing = items.value[byId]!
      if ((existing.quantity || 0) > 1) {
        existing.quantity -= 1
      } else {
        items.value.splice(byId, 1)
      }
    }
  }

  function setQuantity(idOrKey: string, qty: number) {
    // prefer matching internal _key, fallback to id
    let idx = items.value.findIndex(i => i._key === idOrKey)
    if (idx === -1) idx = items.value.findIndex(i => i.id === idOrKey)
    if (idx === -1) return
    const existing = items.value[idx]!
    if (qty <= 0) removeItem(existing._key ?? existing.id)
    else existing.quantity = qty
  }

  function clearCart() {
    items.value = []
  }

  const totalItems = computed(() => items.value.reduce((s, i) => s + (i.quantity || 0), 0))
  const subtotal = computed(() => items.value.reduce((s, i) => s + (i.precio || 0) * (i.quantity || 0), 0))

  return { items, addItem, removeItem, setQuantity, clearCart, totalItems, subtotal, load, save }
})
