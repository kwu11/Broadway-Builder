<template>
  <v-app id="app">
    <Navbar />
    <router-view />
    <Footer />
  </v-app>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Footer from "@/components/Footer.vue";

export default {
  components: {
    Navbar,
    Footer
  }
};
</script>

<style lang="sass">
@import url('https://fonts.googleapis.com/css?family=Roboto');

#app
  font-family: Roboto;
  
</style>