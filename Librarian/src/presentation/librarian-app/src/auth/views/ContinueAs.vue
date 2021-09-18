<template>
  <div class="container" v-if="username">
    <div class="text-h4 my-5">{{ username }} olarak devam etmek ister misin? 🔒 </div>
    <v-btn @click="onSubmit" color="primary" class="mr-4">Elbette</v-btn>
    <v-btn @click="handleLogOut()" outlined color="primary">Çıkayım Ben</v-btn>
  </div>
  <div v-else class="text-h4 m-10">
    Yeniden görüştüğümüze sevindim.
    <v-btn @click="handleLogOut()" outlined color="primary">Lobiye</v-btn>
  </div>
</template>

<script>
import { getUsernameFromToken, logOut } from "@/auth/auth.service";
export default {
  name: "ContinueAs",
  data: () => ({
    username: getUsernameFromToken()
  }),
  methods: {
    handleLogOut() {
      logOut();
    },
    onSubmit() {
      this.$router.push({ path: "/dashboard" });
    }
  }
};
</script>