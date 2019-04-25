<template>
  <v-footer height="auto" color="primary" lighten-1 style="color: white; background: linear-gradient(to right, #6F0000, #200122);">
    <v-layout justify-center row wrap>
      <v-btn v-for="link in links" :key="link" color="white" flat>
        {{ link }}
      </v-btn>
      <v-flex primary lighten-2 py-3 text-xs-center white--text xs12>
        &copy;2018 — <strong whhite--text>Vuetify</strong>
      </v-flex>
    </v-layout>
  </v-footer>
</template>

<script>
export default {
  data: () => ({
    links: ["Home", "About Us", "Team", "Services", "Blog", "Contact Us"]
  })
};
</script>

<style lang="sass" scoped>
@import url('https://fonts.googleapis.com/css?family=Roboto');
</style>
