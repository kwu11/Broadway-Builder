<template>
  <div id="view-theaters">
    <h1 class="hidden-sm-and-up text-xs-center font-weight-bold display-1">List of Theaters</h1>
    <h1
      class="hidden-xs-only hidden-md-and-up text-xs-center font-weight-bold display-2"
    >List of Theaters</h1>
    <h1
      class="hidden-sm-and-down hidden-lg-and-up text-xs-center font-weight-bold display-2"
    >List of Theaters</h1>
    <h1 class="hidden-md-and-down text-xs-center font-weight-bold display-3">List of Theaters</h1>

    <v-container>
      <v-layout row wrap align-top>
        <v-flex id="foo" xs12 sm6 md6 lg4 v-for="(t, index) in theaters" :key="index">
          <TheaterCard :theater="t"/>
        </v-flex>
      </v-layout>
      <v-layout row justify-center>
        <v-btn @click="getMoreTheaters">Load more</v-btn>
      </v-layout>
    </v-container>
  </div>
</template>


<script>
import axios from "axios";
import Vue from "vue";
import ReadMore from "vue-read-more";
// import SearchBar from "@/components/SearchBar.vue";
import TheaterCard from "@/components/TheaterCard.vue";

Vue.use(ReadMore);

export default {
  name: "ViewAllTheaters",
  components: {
    // SearchBar
    TheaterCard
  },
  data() {
    return {
      theaters: [],
      currentPage: 1,
      endpoint: `https://api.broadwaybuilder.xyz/ptheater/all?numberOfItems=24&currentPage=1`,
    };
  },
  methods: {
    getMoreTheaters() {
      axios
        .get(
          "https://api.broadwaybuilder.xyz/ptheater/all?numberOfItems=12&currentPage=" +
            this.currentPage
        )
        .then(response => {
          for (let i = 0; i < response.data.length; i++) {
            this.theaters.push(response.data[i]);
          }

          this.currentPage++;
        });
    }
  },
  async mounted() {
    await axios
      .get(
        "https://api.broadwaybuilder.xyz/ptheater/all?numberOfItems=12&currentPage=" +
          this.currentPage
      )
      .then(response => (this.theaters = response.data));

    this.currentPage++;
  }
};
</script>

<style scoped>
#view-theaters {
  /* top, left/right, bottom */
  margin: 2em 1em 0em;
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
