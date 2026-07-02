import { apiGet } from './api'

export interface OrderHistoryItem {
  nombre: string
  talla: string
  color: string | null
  cantidad: number
  precioUnit: number
  imagenUrl: string | null
}

export interface OrderHistory {
  id: string
  fecha: string
  estado: string
  total: number
  items: OrderHistoryItem[]
}

export const getOrdersByUser = (userId: string) =>
  apiGet<OrderHistory[]>(`/api/orders/user/${encodeURIComponent(userId)}`)
