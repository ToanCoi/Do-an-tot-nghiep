import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home
    },
    {
      path: '/sell',
      name: 'Sell',
      component: () => import('../views/sell/ContentSell.vue')
    },
    {
      path: '/setup/:key?',
      name: 'Setup',
      component: () => import('../views/setup/index.vue')
    },
    
    // {
    //   path: "/about",
    //   name: "about",
    //   // route level code-splitting
    //   // this generates a separate chunk (About.[hash].js) for this route
    //   // which is lazy-loaded when the route is visited.
    //   component: () => import("../views/AboutView.vue"),
    // },
  ],
});

export default router;
