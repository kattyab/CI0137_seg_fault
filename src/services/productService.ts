import { apiGet } from './api'

export interface ProductListItem {
  id: number
  nombre: string
  categoria: string
  precio: number
  imagenUrl: string | null
}

export interface InventoryEntry {
  inventoryId: number
  color: string | null
  talla: string
  stock: number
  imagenUrl: string | null
}

export interface ProductDetail {
  id: number
  nombre: string
  categoria: string
  precio: number
  descripcion: string | null
  imagenUrl: string | null
  inventario: InventoryEntry[]
}

export interface Category {
  id: number
  nombre: string
}

export const getProducts = () => apiGet<ProductListItem[]>('/api/products')
export const getProduct = (id: number | string) => apiGet<ProductDetail>(`/api/products/${id}`)
export const getCategories = () => apiGet<Category[]>('/api/products/categories')
