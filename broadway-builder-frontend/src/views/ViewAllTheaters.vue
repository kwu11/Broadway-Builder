<template>
  <div class="ViewAllTheaters">
    <SearchBar/>
    <v-card>
      <h1 id="applicationPortal">Theaters</h1>

      <v-container fluid grid-list-md>
        <v-layout row wrap>
          <v-flex xs12 sm6 md4 lg3 v-for="(theater, index) in theaters" :key="index">
            <v-card hover v-on:click="goToProfile(theater)">
              <v-card-title primary-title>
                <img src="@/assets/download.png">

                <div id="content">
                  <h3 class="headline mb-0">
                    <strong>{{ theater.TheaterName }}</strong>
                  </h3>
                </div>
                <!-- Allows expansion or shrinkage of app description -->
                <read-more
                  more-str="read more"
                  :text="theater.StreetAddress"
                  less-str="read less"
                  :max-chars="150"
                ></read-more>
              </v-card-title>
            </v-card>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card>
  </div>
</template>

<script>
//import TheaterProfile from "@/views/TheaterProfile.vue";
import axios from "axios";
import Vue from "vue";
import ReadMore from "vue-read-more";
import SearchBar from "@/components/SearchBar.vue";
Vue.use(ReadMore);

export default {
  name: "ViewAllTheaters",
  components: {
    SearchBar
  },
  data() {
    return {
      theaters: []
    };
  },
  methods: {
    goToProfile(theater) {
      this.$router.push({
        name: "theater",
        params: {
          TheaterName: theater.TheaterName,
          Theater: theater
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
.v-card {
  margin: 1em;
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
