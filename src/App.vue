<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="logo-wrapper">
      <img alt="Logo principal" class="logo-img" src="@/assets/logo.png" />
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
        <li>
          <RouterLink to="/carrito">Carrito</RouterLink>
        </li>
        <li>
          <RouterLink to="/contacto">Contacto</RouterLink>
        </li>
      </ul>
    </nav>
    <div class="search-login">
      <input type="search" placeholder="Buscar Sneakers..." />
      <RouterLink to="/login" class="login-link">Iniciar Sesión</RouterLink>
    </div>
  </header>

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
