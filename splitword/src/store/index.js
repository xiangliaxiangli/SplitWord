import Vue from 'vue'
import Vuex from 'vuex'
import axios from "axios";
import element from 'element-ui'

Vue.prototype.axios = axios;
Vue.use(Vuex)
Vue.use(axios)
Vue.use(element)

export default new Vuex.Store({
  state: {
  },
  mutations: {
  },
  actions: {
  },
  modules: {
  }
})

