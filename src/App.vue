<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="logo">
      <img alt="Vue logo" class="logo" src="@/assets/logo.svg" />
    </div>
    <nav>
      <ul>
        <li>
          <RouterLink to="/">Home</RouterLink>
        </li>
        <li>
          <RouterLink to="/low">Low</RouterLink>
        </li>
      </ul>
    </nav>
    <div class="search-login">
      <input type="search" placeholder="Buscar Sneakers...">
      <a href="pages/login.html" class="login-link">Iniciar Sesión</a>
    </div>
  </header>

  <main>
    <RouterView />
  </main>

  <footer>
    <p>Contacto: +506 1234-5678 | charliessneakers@cr.com | Santo Domingo, Heredia</p>
    <p><a href="pages/contacto.html">Contacto</a> | <a href="pages/privacidad.html">Aviso de Privacidad</a> | © 2026
      Charlie's Sneakers</p>
  </footer>
</template>

<style scoped></style>
