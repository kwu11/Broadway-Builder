<template>
  <div class="PicGrid">
    <div id="view-theaters">
      <v-container v-if="viewPix === false" fluid grid-list-md>
        <v-layout row wrap>
          <v-flex xs12 sm6 md6 lg4 v-for="(production, index) in productions" :key="index">
            <v-flex xs12>
              <!-- Clicking on the image goes to that theater -->
              <v-card>
                <v-card-title primary-title>
                  <div>
                    <div class="headline">{{ production.ProductionName }}</div>
                    <span
                      class="grey--text"
                    >Directed by {{ production.DirectorFirstName }} {{ production.DirectorLastName }}</span>
                  </div>
                  <span class="grey--text">
                    <a v-on:click="viewCarousel(production.ProductionID)">Pictures</a> |
                    <a
                      :href="'https://api.broadwaybuilder.xyz/Programs/Production'+production.ProductionID+'/'+production.ProductionID+'.pdf'"
                    >Program</a>
                  </span>
                  <v-spacer></v-spacer>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn icon @click="show = !show">
                      <v-icon>{{ show ? 'keyboard_arrow_down' : 'keyboard_arrow_up' }}</v-icon>
                    </v-btn>
                  </v-card-actions>
                </v-card-title>

                <!-- Open description -->
                <v-slide-x-transition>
                  <v-card-text
                    v-show="show"
                  >I'm a thing. But, like most politicians, he promised more than he could deliver. You won't have time for sleeping, soldier, not with all the bed making you'll be doing. Then we'll go with that data file! Hey, you add a one and two zeros to that or we walk! You're going to do his laundry? I've got to find a way to escape.</v-card-text>
                </v-slide-x-transition>
              </v-card>
            </v-flex>
          </v-flex>
        </v-layout>
      </v-container>
    </div>

    <!-- <v-container v-if="viewPix === false" fluid grid-list-md>
      <v-layout row wrap>
        <v-flex v-for="(production, index) in productions" :key="index">
          <div v-show="production.TheaterID == TheaterID" class="card">
            <div class="card-image">
              <figure class="image isrounded is-3by2">
                <img class="isrounded" src="@/assets/download.png" alt>
              </figure>
              <div class="card-content is-overlay is-clipped">
                <span class="tag is-info">{{production.ProductionName}}</span>
              </div>
            </div>
            <footer class="card-footer">
              <div class="card-footer-item">
                <a v-on:click="viewCarousel(production.ProductionID)">Pictures</a> |
                <a
                  :href="'https://api.broadwaybuilder.xyz/Programs/Production'+production.ProductionID+'/'+production.ProductionID+'.pdf'"
                >Program</a>
              </div>
            </footer>
          </div>
        </v-flex>
      </v-layout>
    </v-container>-->

    <div class="button is-primary" v-if="viewPix === true" v-on:click="viewPix=false">Back</div>
    <v-gallery v-if="viewPix === true" type="carousel" :images="pics"></v-gallery>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "PicGrid",
  data() {
    return {
      productions: [],
      theaters: [],
      TheaterID: this.$attrs.TheaterID,
      viewPix: false,
      picSrcs: [],
      pics: [],
      show: false
    };
  },
  props: {
    today: Date
  },
  async mounted() {
    await axios
      .get(
        "https://api.broadwaybuilder.xyz/production/getProductions?previousDate=3%2F23%2F2019",
        {
          params: {
            theaterID: this.TheaterID
          }
        }
      )
      .then(response => (this.productions = response.data));
  },
  methods: {
    async viewCarousel(ProductionID) {
      await axios
        .get(
          "https://api.broadwaybuilder.xyz/production/" +
            ProductionID +
            "/getPhotos"
        )
        .then(response => (this.picSrcs = response.data));
      this.viewPix = !this.viewPix;
      var aLength = this.picSrcs.length;
      for (var i = 0; i < aLength; i++) {
        this.pics[i] = { title: " ", url: this.picSrcs[i] };
      }
    }
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.card
  border-radius: 6px
  overflow: hidden
  box-shadow: 0 14px 28px rgba(0,0,0,0.25), 0 10px 10px rgba(0,0,0,0.22)
  margin: 1em 0 1em 0

.card-footer-item 
    color: black
a
  color: black
  margin: 1em
</style>