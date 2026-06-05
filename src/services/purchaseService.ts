import type { CartItem } from '@/stores/cart'

export interface PurchaseItem {
  id: string
  lineKey: string
  nombre: string
  precio: number
  quantity: number
  image?: string
  size?: string
  color?: string
}

export interface Purchase {
  id: string
  userId: string
  items: PurchaseItem[]
  subtotal: number
  shipping: number
  total: number
  status: 'Completado'
  createdAt: string
}

const PURCHASES_STORAGE_KEY = 'purchase_history'

const safeParsePurchases = (value: string | null): Purchase[] => {
  if (!value) return []

  try {
    return JSON.parse(value) as Purchase[]
  } catch {
    return []
  }
}

export const purchaseService = {
  getAllPurchases(): Purchase[] {
    return safeParsePurchases(localStorage.getItem(PURCHASES_STORAGE_KEY))
  },

  getPurchasesByUserId(userId: string): Purchase[] {
    return this.getAllPurchases()
      .filter((purchase) => purchase.userId === userId)
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
  },

  createPurchase(data: {
    userId: string
    items: CartItem[]
    subtotal: number
    shipping: number
    total: number
  }): Purchase {
    const purchases = this.getAllPurchases()

    const newPurchase: Purchase = {
      id: `purchase_${Date.now()}`,
      userId: data.userId,
      items: data.items.map((item) => ({
        id: item.id,
        lineKey: item._key ?? item.id,
        nombre: item.nombre,
        precio: item.precio,
        quantity: item.quantity,
        image: item.image,
        size: item.size,
        color: item.color,
      })),
      subtotal: data.subtotal,
      shipping: data.shipping,
      total: data.total,
      status: 'Completado',
      createdAt: new Date().toISOString(),
    }

    purchases.push(newPurchase)
    localStorage.setItem(PURCHASES_STORAGE_KEY, JSON.stringify(purchases))

    return newPurchase
  },
}
