<template>
  <v-footer height="auto" lighten-1>
    <v-layout class="footer" justify-center row wrap>
      <v-btn v-for="link in links" :key="link" color="white" flat>
        {{ link }}
      </v-btn>
      <v-flex class="footer" lighten-2 py-3 text-xs-center white--text xs12>
        &copy;2019 — <strong white--text>Broadway Builder</strong>
      </v-flex>
    </v-layout>
  </v-footer>
</template>

<script>
export default {
  data: () => ({
    links: ["Team", "Privacy", "Terms of Service", "Contact Us"]
  })
};
</script>

<style lang="css" scoped>
@import url("https://fonts.googleapis.com/css?family=Roboto");

.footer {
  color: white;
  background: #7d1b1b;
  z-index: 10;
}
</style>
