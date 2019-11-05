<template>
  <div>
    <!-- Navbar for desktop -->
    <v-toolbar
      class="hidden-sm-and-down"
      style="color: white; background: #7d1b1b;"
    >
      <v-toolbar-title>Broadway Builder</v-toolbar-title>
      <v-spacer></v-spacer>
      <!-- Displays the links that route to other pages -->
      <v-toolbar-items v-for="(route, index) in pageRoutes" :key="index">
        <v-btn :to="route.link" flat v-if="route.show">
          <span class="color-white">{{ route.title }}</span>
        </v-btn>
      </v-toolbar-items>
    </v-toolbar>

    <!-- Navbar for mobile -->
    <v-toolbar
      class="hidden-md-and-up"
      style="color: white; background: #7d1b1b;"
    >
      <v-toolbar-title>Broadway Builder</v-toolbar-title>
      <v-spacer></v-spacer>
      <!-- When on a smaller screen, a navigation hamburger will show up -->
      <v-menu class="hidden-md-and-up" transition="slide-y-transition" bottom left>
        <template v-slot:activator="{ on }">
          <v-toolbar-side-icon v-on="on" style="margin: 0 6px 0 0; color: white;"></v-toolbar-side-icon>
        </template>
        <!-- Using the list of destinations, display and route to the page link -->
        <v-list v-for="(route, index) in activeRoutes" :key="index">
          <v-list-tile>
            <v-btn :to="route.link" flat block>
              <span class="color-black">{{ route.title }}</span>
            </v-btn>
          </v-list-tile>
        </v-list>
      </v-menu>
    </v-toolbar>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  data: function() {
    return {};
  },
  computed: mapState({
    pageRoutes: state => [
      { title: "Home", link: "/", show: true },
      { title: "Theaters", link: "/theaters", show: true },
      {
        title: "Account",
        link: "/sysadminaccount/{userID}",
        show: state.isSysAdmin
      },
      {
        title: "Account",
        link: "/adminaccount/{userID}",
        show: state.isTheaterAdmin
      },
      { title: "About Us", link: "/aboutus", show: true },
      { title: "Log out", link: "/logout", show: state.token !== null }
    ],
    activeRoutes() {
      return this.pageRoutes.filter(pageRoutes => pageRoutes.show);
    }
  })
};
</script>

<style>
@import url("https://fonts.googleapis.com/css?family=Roboto");

div.v-toolbar__content {
  padding: 0 0 0 16px;
}

.color-white {
  color: white;
}

.color-black {
  color: black;
}
</style>