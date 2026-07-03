import { apiPost } from './api'

export interface CheckoutItem {
  inventoryId: number
  quantity: number
}

export interface PaymentData {
  cardholderName: string
  cardNumber: string
  expMonth: number
  expYear: number
  cvc: string
}

export interface CheckoutRequest {
  userId: string
  items: CheckoutItem[]
  payment: PaymentData
}

export interface CheckoutResponse {
  orderId: string
  paymentId: string
  status: string
  total: number
  authorizationCode: string | null
  message: string
}

export const checkout = (request: CheckoutRequest) =>
  apiPost<CheckoutRequest, CheckoutResponse>('/api/payments/checkout', request)

export interface CardValidationResponse {
  valid: boolean
  error: string | null
  brand: string | null
  issuerBank: string | null
  last4: string | null
}

export const validateCard = (cardNumber: string) =>
  apiPost<{ cardNumber: string }, CardValidationResponse>('/api/payments/validate-card', { cardNumber })
