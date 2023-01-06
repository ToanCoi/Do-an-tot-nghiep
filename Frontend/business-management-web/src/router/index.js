import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'
import Login from '../views/login/Login.vue'
import { store } from '../store/index'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'Login',
      component: Login
    },
    {
      path: '/logout',
      name: 'Logout',
      component: Login
    },
    {
      path: '/',
      name: 'MainLayout',
      component: () => import('../views/MainLayout.vue'),
      meta: {
        requiresAuth: true
      },
      children: [
        {
          path: '/home',
          name: 'Home',
          component: Home
        },
        {
          path: '/sell',
          name: 'Sell',
          component: () => import('../views/sell/index.vue')
        },
        {
          path: '/setup/:key?',
          name: 'Setup',
          component: () => import('../views/setup/index.vue')
        },
      ],
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

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {

    if (!store.getters.token) {
      next({ name: 'Login' })
    } else {
      next() 
    }
  } else {
    next() 
  }
})

export default router;
