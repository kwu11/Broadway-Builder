<template>
  <div>
    <!-- The entire navbar options and dropdown menu (navigation hamburger) -->
    <v-toolbar style="padding: 0 10em; color: white; background: linear-gradient(to right, #6F0000, #000);">
      <v-toolbar-title>Broadway Builder</v-toolbar-title>
      <v-spacer></v-spacer>
      <!-- Displays the links that route to other pages -->
      <v-toolbar-items class="hidden-sm-and-down" v-for="(route, index) in pageRoutes" :key="index">
        <v-btn :to="route.link" flat style="color: white;">{{ route.title }}</v-btn>
      </v-toolbar-items>

      <!-- When on a smaller screen, a navigation hamburger will show up -->
      <v-menu class="hidden-md-and-up" transition="slide-y-transition" bottom left>
        <!-- Shows the hamburger icon when on a small screen -->
        <template v-slot:activator="{ on }">
          <v-toolbar-side-icon v-on="on" right style="color: white"></v-toolbar-side-icon>
        </template>
        <!-- Using the list of destinations, display and route to the page link -->
        <v-list v-for="(route, index) in pageRoutes" :key="index">
          <v-list-tile>
            <v-btn :to="route.link" flat style="color: black" block>{{ route.title }}</v-btn>
          </v-list-tile>
        </v-list>
      </v-menu>
    </v-toolbar>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
      pageRoutes: [
        { title: "Home", link: "/" },
        { title: "Theaters", link: "/theaters" },
        { title: "Account", link: "/adminaccount/{userID}" },
        { title: "About Us", link: "/register" }
      ]
    };
  }
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css?family=Roboto");
</style>