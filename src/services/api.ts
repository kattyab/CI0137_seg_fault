const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5269'

export class ApiError extends Error {
  constructor(
    message: string,
    public readonly status: number,
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

export async function apiGet<T>(path: string): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`)
  if (!res.ok) {
    throw new Error(`API error ${res.status} en ${path}`)
  }
  return res.json() as Promise<T>
}

export async function apiPost<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse> {
  const res = await fetch(`${BASE_URL}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })

  if (!res.ok) {
    let message = `Error del servidor (${res.status}).`
    try {
      const data = (await res.json()) as { message?: unknown }
      if (typeof data?.message === 'string' && data.message.trim()) {
        message = data.message
      }
    } catch {
      // Cuerpo no-JSON (p. ej. ProblemDetails vacío): usar mensaje genérico
    }
    throw new ApiError(message, res.status)
  }

  return res.json() as Promise<TResponse>
}
