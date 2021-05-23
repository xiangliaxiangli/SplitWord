<template>
  <v-app>
    <NavBar/>
    <v-content>
      <router-view></router-view>
    </v-content>
  </v-app>
</template>

<script>
import NavBar from "@/components/NavBar";
export default {
  name: "Index",
  components:{ NavBar },
  data(){
    return{

    }
  }
}
</script>

<style scoped>

</style>