<script setup lang="ts">
import { computed, ref } from 'vue'
import { RouterLink, RouterView } from 'vue-router'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

const isLoggedIn = computed(() => Boolean(auth.user))
const showUserMenu = ref(false)

const handleLogout = () => {
  auth.logout()
  router.push('/')
  showUserMenu.value = false
}
</script>

<template>
  <header>
    <div class="logo-wrapper">
      <RouterLink to="/">
        <img alt="Logo principal" class="logo-img" src="@/assets/logo.png" />
      </RouterLink>
    </div>
    <nav>
      <ul>
        <li>
          <RouterLink to="/">Home</RouterLink>
        </li>
        <li>
          <RouterLink to="/low">Low</RouterLink>
        </li>
        <li>
          <RouterLink to="/mid">Mid</RouterLink>
        </li>
        <li>
          <RouterLink to="/high">High</RouterLink>
        </li>
        <li>
          <RouterLink to="/coleccion">Colección</RouterLink>
        </li>
      </ul>
    </nav>
    <div class="search-login">
      <input type="search" placeholder="Buscar Sneakers..." />
      <RouterLink v-if="!isLoggedIn" to="/login" class="login-link">Iniciar Sesión</RouterLink>
      <div v-else style="position: relative">
        <button
          type="button"
          @click="showUserMenu = !showUserMenu"
          style="
            background: none;
            border: none;
            cursor: pointer;
            padding: 0;
            display: flex;
            align-items: center;
          "
          aria-label="Menú de usuario"
        >
          <img src="@/assets/images/user.svg" alt="" aria-hidden="true" class="cart-icon" />
        </button>
        <div
          v-if="showUserMenu"
          style="
            position: absolute;
            top: 100%;
            right: 0;
            background: white;
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 1rem;
            margin-top: 0.5rem;
            z-index: 1000;
            min-width: 250px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
          "
        >
          <div style="margin-bottom: 1rem">
            <div style="font-weight: 700; margin-bottom: 0.25rem; color: #000">{{ auth.user?.nombre }}</div>
            <div style="font-size: 0.85rem; color: #666">{{ auth.user?.email }}</div>
            <div style="font-size: 0.85rem; color: #666">{{ auth.user?.telefono }}</div>
          </div>
          <button
            type="button"
            @click="handleLogout"
            style="
              width: 100%;
              padding: 0.5rem;
              background: #ff6b35;
              color: white;
              border: none;
              border-radius: 4px;
              cursor: pointer;
              font-weight: 600;
            "
          >
            Cerrar sesión
          </button>
        </div>
      </div>
      <RouterLink to="/carrito" class="cart-link" aria-label="Ir al carrito de compras">
        <img src="@/assets/images/shopping_cart.svg" alt="" aria-hidden="true" class="cart-icon" />
      </RouterLink>
    </div>
  </header>

  <div
    v-if="showUserMenu"
    @click="showUserMenu = false"
    style="
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      z-index: 999;
    "
  ></div>

  <main>
    <RouterView />
  </main>

  <footer>
    <p>Contacto: +506 1234-5678 | charliessneakers@cr.com | Santo Domingo, Heredia</p>
    <p>
      <RouterLink to="/contacto">Contacto</RouterLink> |
      <RouterLink to="/privacidad">Aviso de Privacidad</RouterLink> | © 2026 Charlie's Sneakers
    </p>
  </footer>
</template>

<style scoped></style>
