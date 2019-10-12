<template>
  <div id="view-theaters">
    <h1 class="hidden-sm-and-up text-xs-center font-weight-bold display-1">List of Theaters</h1>
    <h1
      class="hidden-xs-only hidden-md-and-up text-xs-center font-weight-bold display-2"
    >List of Theaters</h1>
    <h1
      class="hidden-sm-and-down hidden-lg-and-up text-xs-center font-weight-bold display-3"
    >List of Theaters</h1>
    <h1 class="hidden-md-and-down text-xs-center font-weight-bold display-3">List of Theaters</h1>
    <v-container>
      <v-layout row wrap>
        <v-flex xs12 sm6 md6 lg4 v-for="(theater, index) in theaters" :key="index">
          <v-flex xs12>
            <!-- Clicking on the image goes to that theater -->
            <v-card>
              <v-img
                src="https://picsum.photos/510/300?random"
                height="200px"
                @click="goToProfile(theater)"
              ></v-img>
              <v-card-title primary-title>
                <div @click="goToProfile(theater)">
                  <div class="headline">{{ theater.TheaterName }}</div>
                  <span class="grey--text">{{ theater.CompanyName }}</span>
                </div>
                <v-spacer></v-spacer>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn icon @click="show = !show">
                    <v-icon>{{ show ? 'keyboard_arrow_down' : 'keyboard_arrow_up' }}</v-icon>
                  </v-btn>
                </v-card-actions>
              </v-card-title>

              <!-- Open description -->
              <v-slide-y-transition>
                <v-card-text
                  v-show="show"
                >I'm a thing. But, like most politicians, he promised more than he could deliver. You won't have time for sleeping, soldier, not with all the bed making you'll be doing. Then we'll go with that data file! Hey, you add a one and two zeros to that or we walk! You're going to do his laundry? I've got to find a way to escape.</v-card-text>
              </v-slide-y-transition>
            </v-card>
          </v-flex>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import axios from "axios";
import Vue from "vue";
import ReadMore from "vue-read-more";
import SearchBar from "@/components/SearchBar.vue";
Vue.use(ReadMore);

export default {
  name: "ViewAllTheaters",
  components: {
    // SearchBar
  },
  data() {
    return {
      show: false,
      theaters: []
    };
  },
  methods: {
    goToProfile(theater) {
      this.$router.push({
        name: "theater",
        params: {
          theater: theater
        }
      });
    }
  },
  async mounted() {
    await axios
      .get("https://api.broadwaybuilder.xyz/theater/all")
      .then(response => (this.theaters = response.data));
  }
};
</script>

<style scoped>
#view-theaters {
  /* top, left/right, bottom */
  margin: 2em 1em 5em;
}

.v-card {
  /* top/bottom, right/left */
  margin: 2em 1em;
}

#applicationPortal {
  padding: 1em;
  font-size: 38px;
  text-decoration: underline;
}

#content {
  margin-left: 1em;
}

img {
  width: 55px;
  height: 55px;
}

.headline:hover {
  text-decoration: underline;
}
</style>
