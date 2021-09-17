<template>
  <!--
    Menüyü oluşturuyoruz.
    Lobi yazan kısım bir button şeklinde ve root adrese yönlendiriyor.

    Bilgi linkine bastığımızda router bunu /about bileşenine yönlendiriyor.

    Envanter Yönetimi dashboard klasörüne yönlendirme yapacak ve oradaki index.vue bileşenine göre şekillenecek.
    -->
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

  <!--
    authModule modülündeki isAuthenticated özelliğinin değerine göre dashboard görünecek aksi durumda login olunmasını isteyen bir vue button olacak.
    -->
      <v-btn
        v-if="isAuthenticated"
        color="primary"
        outlined
        :to="{ path: '/dashboard' }"
      >
        <span class="menu">Envanter</span>
      </v-btn>
      <v-btn v-else color="primary" outlined :to="{ path: '/login' }">
        <span class="menu">Giriş</span>
      </v-btn>
    </div>
  </v-app-bar>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  name: "NavigationBar",
  computed: {
    ...mapGetters("authModule", {
      isAuthenticated: "isAuthenticated",
      username: "username",
    }),
  },
};
</script>

<style scoped>
.menu {
  color: white;
  text-decoration: none;
}
</style>