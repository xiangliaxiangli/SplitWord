import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Main from "@/components/Main";
import Search from "@/components/Search";
import File from "../components/file";
import Login from "../components/Login";
import Index from "@/components/Index";
import Logon from "../components/Logon";

Vue.use(VueRouter)

const routes = [
  {
    path: '/Login',
    name: Login,
    component: Login
  },
  {
    path: '/Logon',
    name: Logon,
    component: Logon
  },
  {
    path: '/',
    name: Login,
    component: Login
  },
  {
    path: '/Index',
    name: Index,
    component: Index,
    children:[
      {
        path: '/Main',
        name: Main,
        component: Main
      },
      {
        path: '/Search',
        name: 'Search',
        component: Search
      },{
        path: '/File',
        name: 'File',
        component: File
      }
    ]
  },

]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
