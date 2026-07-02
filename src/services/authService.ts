import { apiPost } from './api'

export interface RegisterRequest {
  nombre: string
  email: string
  telefono: string
  password: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface AuthResponse {
  id: string
  nombre: string
  email: string
  telefono: string
  token: string
  createdDate: string
}

export function register(body: RegisterRequest): Promise<AuthResponse> {
  return apiPost<RegisterRequest, AuthResponse>('/api/auth/register', body)
}

export function login(body: LoginRequest): Promise<AuthResponse> {
  return apiPost<LoginRequest, AuthResponse>('/api/auth/login', body)
}
