<template>
  <v-sheet
    height="100vh"
    class="overflow-hidden"
    style="display: flex; flex-direction: row; justify-content: flex-start"
  >
    <v-navigation-drawer permanent expand-on-hover>
      <v-list class="d-flex flex-column justify-center align-center">
        <v-list-item class="px-2">
          <v-list-item-avatar>
            <v-img
              src="https://randomuser.me/api/portraits/women/27.jpg"
            ></v-img>
          </v-list-item-avatar>
        </v-list-item>

        <v-list-item link>
          <v-list-item-content>
            <v-list-item-subtitle>email here</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>

      <v-divider></v-divider>

      <v-list nav dense>
        <router-link to="/dashboard/" class="menu link">
          <v-list-item link>
            <v-list-item-icon>
              <v-icon>mdi-view-dashboard-variant-outline</v-icon>
            </v-list-item-icon>
            <v-list-item-title>Dashboard</v-list-item-title>
          </v-list-item>
        </router-link>
        <router-link to="/dashboard/book-list" class="menu link">
          <v-list-item link>
            <v-list-item-icon>
              <v-icon>mdi-format-list-bulleted</v-icon>
            </v-list-item-icon>
            <v-list-item-title> Kitap Listesi </v-list-item-title>
          </v-list-item>
        </router-link>
        <router-link to="/logout" class="menu link">
          <v-list-item link>
            <v-list-item-icon>
              <v-icon>mdi-logout-variant</v-icon>
            </v-list-item-icon>
            <v-list-item-title> Çıkış </v-list-item-title>
          </v-list-item>
        </router-link>
      </v-list>
    </v-navigation-drawer>
    <v-container>
      <router-view />
    </v-container>
  </v-sheet>
</template>

<script>
export default {
  name: "dashboard",
};
</script>

<style scoped>
.link {
  text-decoration: none;
}
</style>