import Vue from "vue";
import VueRouter from "vue-router";
import Home from "@/views/Home.vue";
import BookList from "@/views/dashboard/BookList";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  {
    path: "/about",
    name: "About",
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue"),
  },
  {
    path: "/dashboard",
    component: () => import("@/views/dashboard"),
    children: [
      {
        path: "",
        component: () => import("@/views/dashboard/DefaultContent"),
      },
      {
        path: "book-list",
        component: BookList,
      }
    ],
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

export default router;
