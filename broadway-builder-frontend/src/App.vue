<template>
  <v-app id="app">
    <div class="site">
      <Navbar />

      <div class="site-content">
        <router-view />
      </div>
      
      <Footer />
    </div>
  </v-app>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Footer from "@/components/Footer.vue";

export default {
  components: {
    Navbar,
    Footer
  },
  async created() {
    await this.$store.dispatch('updateUserInfo');
  }
};
</script>

<style lang="sass">
@import url('https://fonts.googleapis.com/css?family=Roboto');

#app
  font-family: Roboto;
  
.site
  display: flex;
  min-height: 100vh;
  flex-direction: column;

.site-content
  flex: 1;
</style>