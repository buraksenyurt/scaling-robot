<template>
  <v-app-bar app color="primary" light>
    <div>
      <v-btn color="primary" outlined :to="{ path: '/' }">
        <span class="menu">Lobi</span>
      </v-btn>

      <router-link to="/about">
        <v-btn color="primary" outlined>
          <span class="menu">Bilgi</span>
        </v-btn>
      </router-link>

      <v-btn color="primary" outlined :to="{ path: '/dashboard' }">
        <span class="menu">Envanter Yönetimi</span>
      </v-btn>
      <v-btn color="primary" outlined :to="{ path: '/login' }">
        <span class="menu">Giriş</span>
      </v-btn>
    </div>
  </v-app-bar>
</template>

<script>
export default {
  name: "NavigationBar",
};
</script>

<style scoped>
.menu {
  color: white;
  text-decoration: none;
}
</style>