<template>
  <div id="app">
    <v-toolbar style="color: white; background: linear-gradient(to right, #6F0000, #200122);">
      <v-toolbar-side-icon style="color: white"></v-toolbar-side-icon>
      <v-toolbar-title>Broadway Builder</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items class="hidden-sm-and-down">
        <v-btn to="/" flat style="color: white;">Home</v-btn>
        <v-btn to="/theaters" flat style="color: white">Theaters</v-btn>
        <v-btn to="/adminaccount/{userID}" flat style="color: white">Account</v-btn>
        <v-btn flat style="color: white">Register</v-btn>
      </v-toolbar-items>
    </v-toolbar>
  </div>
</template>

<script>
export default {
  data: function() {
    return {};
  }
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css?family=Roboto");
</style>