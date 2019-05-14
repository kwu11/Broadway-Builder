<template>
  <div>
    <!-- The entire navbar options and dropdown menu (navigation hamburger) -->
    <v-toolbar class="hidden-sm-and-down" style="padding: 0 10em; color: white; background: linear-gradient(to right, #6F0000, #000);">
      <v-toolbar-title>Broadway Builder</v-toolbar-title>
      <v-spacer></v-spacer>
      <!-- Displays the links that route to other pages -->
      <v-toolbar-items v-for="(route, index) in pageRoutes" :key="index">
        <v-btn :to="route.link" flat  v-if="route.show">
          <span class="color-white">
            {{ route.title }}
          </span>
        </v-btn>
      </v-toolbar-items>
    </v-toolbar>

    <v-toolbar class="hidden-md-and-up" style="padding: 0 0em; color: white; background: linear-gradient(to right, #6F0000, #000);">
      <v-toolbar-title>Broadway Builder</v-toolbar-title>
      <v-spacer></v-spacer>

      <!-- When on a smaller screen, a navigation hamburger will show up -->
      <v-menu class="hidden-md-and-up" transition="slide-y-transition" bottom left>
        <!-- Shows the hamburger icon when on a small screen -->
        <template v-slot:activator="{ on }">
          <v-toolbar-side-icon v-on="on" right style="color: white;"></v-toolbar-side-icon>
        </template>
        <!-- Using the list of destinations, display and route to the page link -->
        <v-list v-for="(route, index) in pageRoutes" :key="index">
          <v-list-tile v-if="route.show">
            <v-btn :to="route.link" flat block>
              <span class="color-black">
                {{ route.title }}
              </span>
            </v-btn>
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
      pageRoutes: []
    };
  },
  async mounted() {
    this.pageRoutes = [
      { title: "Home", link: "/", show: true },
      { title: "Theaters", link: "/theaters", show: true },
      { title: "Account", link: "/sysadminaccount/{userID}", show: this.$store.state.isSysAdmin },
      { title: "Account", link: "/adminaccount/{userID}", show: this.$store.state.isTheaterAdmin },
      { title: "About Us", link: "/aboutus", show: true },
      { title:"Log out",link: "/logout",show : User.token !==null}
    ];
  }
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css?family=Roboto");

.color-white {
  color: white;
}

.color-black {
  color: black;
}
</style>