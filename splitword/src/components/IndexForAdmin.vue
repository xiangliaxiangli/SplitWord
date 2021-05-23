<template>
  <v-app>
    <NavBarForAdmin/>
    <v-content>
      <router-view></router-view>
    </v-content>
  </v-app>
</template>

<script>
import NavBarForAdmin from "@/components/NavBarForAdmin";
export default {
  components: {NavBarForAdmin},
  name: "IndexForAdmin"
}
</script>

<style scoped>

</style>