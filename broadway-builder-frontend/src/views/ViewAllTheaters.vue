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
          <TheaterCard :theater="t" />
        </v-flex>
      </v-layout>
      <v-layout row justify-center v-if="hasNext">
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
      nextTheaters: [],
      hasNext: false,
      nextPage: 1,
      numResults: 12,
      endpoint: `https://api.broadwaybuilder.xyz/ptheater/all?numberOfItems=${this.numResults}&currentPage=1`
    };
  },

  async mounted() {
    var moreThanN = false;
    // Initial theater list:
    // Load one extra item to check if there are exactly N items, disable the 'load more' button if there are
    await axios
      .get(
        `https://api.broadwaybuilder.xyz/ptheater/all?numberOfItems=${this.numResults+1}&currentPage=${this.nextPage}`
      )
      .then(response => {
        moreThanN = this.responseReturnsMoreThanNTheaters(response);
      });

    if (moreThanN) {
      await this.preloadTheaters();
    }
  },

  methods: {
    responseReturnsMoreThanNTheaters(response) {
      let count = response.data.length;
      var c = 0; // Number of items to load into theater list

      if (count == this.numResults) {
        this.hasNext = false;
        c = count;
      }
      else {
        c = count-1;
      }        

      for (let i = 0; i < c; i++) {
        this.theaters.push(response.data[i]);
      }

      return count > this.numResults;
    },

    async preloadTheaters() {
      this.nextPage++;

      // Next theater list:
      // Pre-load the next set of N items and activate the 'load more' button only if there are more items
      await axios
        .get(
          `https://api.broadwaybuilder.xyz/ptheater/all?numberOfItems=${this.numResults}&currentPage=${this.nextPage}`
        )
        .then(r => {
          for (let i = 0; i < r.data.length; i++) {
            this.nextTheaters.push(r.data[i]);
          }

          this.hasNext = this.nextTheaters.length > 0;
        });
    },

    checkAndAddMoreTheaters() {
      this.nextTheaters = [];
      this.nextPage++;

      axios
        .get(
          `https://api.broadwaybuilder.xyz/ptheater/all?numberOfItems=${this.numResults}&currentPage=${this.nextPage}`
        )
        .then(response => {
          let count = response.data.length;
          this.hasNext = count > 0;

          for (let i = 0; i < count; i++) {
            this.nextTheaters.push(response.data[i]);
          }
        });
    },

    getMoreTheaters() {
      if (this.nextTheaters.length > 0) {
        this.theaters = this.theaters.concat(this.nextTheaters);

        // If there were N or more items, check if there are more to load
        if (this.nextTheaters.length >= this.numResults) {
          this.checkAndAddMoreTheaters();
        }
        else {
          this.hasNext = false;
        }
      }
    }

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
