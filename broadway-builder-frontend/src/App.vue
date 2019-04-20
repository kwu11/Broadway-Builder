<template>
  <div id="app">
    <Navbar/>
  </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";

export default {
  components: {
    Navbar
  }
};
</script>

<style lang="sass">
@import '../node_modules/bulma/bulma.sass'
@import url('https://fonts.googleapis.com/css?family=Roboto');

</style>