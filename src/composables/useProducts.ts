import { computed, ref } from 'vue'
import { getProducts, type ProductListItem } from '@/services/productService'

// module-level cache so category views don't refetch on every navigation
let cache: ProductListItem[] | null = null
let inflight: Promise<ProductListItem[]> | null = null

export function useProducts(categoria?: string) {
  const all = ref<ProductListItem[]>(cache ?? [])
  const loading = ref(!cache)
  const error = ref<string | null>(null)

  async function load(force = false) {
    if (cache && !force) {
      all.value = cache
      loading.value = false
      return
    }
    loading.value = true
    error.value = null
    try {
      if (!inflight) inflight = getProducts()
      cache = await inflight
      all.value = cache
    } catch {
      error.value = 'No se pudieron cargar los productos. Intenta de nuevo más tarde.'
    } finally {
      inflight = null
      loading.value = false
    }
  }

  void load()

  const products = computed(() =>
    categoria ? all.value.filter(p => p.categoria === categoria) : all.value,
  )

  return { products, loading, error, reload: () => load(true) }
}
