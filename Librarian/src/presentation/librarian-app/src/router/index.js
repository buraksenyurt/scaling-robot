import Vue from "vue";
import VueRouter from "vue-router";
import Home from "@/views/Home.vue";
import BookList from "@/views/dashboard/BookList";
import { agentScally } from "@/auth/bodyguard";

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
    meta: {
      requiresAuth: false, // Hakkında sayfası için authorization gerekli değil.
    },
  },
  {
    path: "/dashboard",
    component: () => import("@/views/dashboard"),
    meta: {
      requiresAuth: true // Artık dashboard tarafı için authorization istiyoruz
    },
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
  },
  {
    path: "/login",
    component: () => import("@/auth/views/Login"),
    meta: {
      requiresAuth: false
    },
  },
  {
    path: "/logout",
    beforeEnter() {
      localStorage.clear();
      window.location.href = "/";
    }
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

// uygulamadaki hareketlerde agentScally modülü vasıtasıyla authentication mekanizmasını devreye girecek
router.beforeEach((to, from, next) => {
  console.log("Before Each");
  agentScally(to, from, next);
});

export default router;
