<template>
  <v-app>
    <NavigationBar /> <!-- Vue bileşenlere(component) dayalı çalışır. src/components altında tasarladığımız NavigationBar bileşeni buraya gelecektir. Sayfanın üst kısmında yatay bir menü çubuğu olarak-->
    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script>
import NavigationBar from "@/components/NavigationBar";
export default {
  name: "App",
  components: {
    NavigationBar,
  },
};
</script>