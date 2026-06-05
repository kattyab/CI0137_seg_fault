import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/producto/:id',
      name: 'product',
      component: () => import('@/views/ProductView.vue'),
    },
    {
      path: '/low',
      name: 'low',
      component: () => import('@/views/LowView.vue'),
    },
    {
      path: '/mid',
      name: 'mid',
      component: () => import('@/views/MidView.vue'),
    },
    {
      path: '/high',
      name: 'high',
      component: () => import('@/views/HighView.vue'),
    },
    {
      path: '/coleccion',
      name: 'coleccion',
      component: () => import('@/views/ColeccionView.vue'),
    },
    {
      path: '/carrito',
      name: 'carrito',
      component: () => import('@/views/CarritoView.vue'),
    },
    {
      path: '/checkout',
      name: 'checkout',
      component: () => import('@/views/CheckoutView.vue'),
    },
    {
      path: '/checkout/confirmado',
      name: 'checkout-confirmado',
      component: () => import('@/views/CheckoutSuccessView.vue'),
    },
    {
      path: '/contacto',
      name: 'contacto',
      component: () => import('@/views/ContactoView.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
    },
    {
      path: '/privacidad',
      name: 'privacidad',
      component: () => import('@/views/PrivacidadView.vue'),
    },
    {
      path: '/foro',
      name: 'foro',
      component: () => import('@/views/ForoView.vue'),
    },
    {
      path: '/registro',
      name: 'registro',
      component: () => import('@/views/RegistroView.vue'),
    },
    {
      path: '/recuperar-contrasena',
      name: 'recuperar-contrasena',
      component: () => import('@/views/RecuperarContrasenaView.vue'),
    },
  ],
})

export default router
