import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Main from "@/components/Main";
import Search from "@/components/Search";
import File from "../components/file";

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: Main,
    component: Main
  },
  {
    path: '/Home',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },{
    path: '/Search',
    name: 'Search',
    component: Search
  },{
    path: '/File',
    name: 'File',
    component: File
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
