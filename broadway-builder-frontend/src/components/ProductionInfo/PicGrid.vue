<template>
  <div class="PicGrid">
    <v-container v-if="viewPix === false" fluid grid-list-md>
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
                <a v-on:click="viewPics(production.ProductionID)">Pictures</a> |
                <a>Program</a>
              </div>
            </footer>
          </div>
        </v-flex>
      </v-layout>
    </v-container>
    <AdminPictureWheel
      v-bind:TheaterID="this.TheaterID"
      v-bind:currentProd="currentProd"
      v-if="viewPix === true"
    />
    <a
      v-on:click="goToPictures(TheaterID)"
      class="button is-danger is-rounded is-medium"
      v-if="viewPix=== true"
    >Return</a>
  </div>
</template>

<script>
import axios from "axios";
import { isDate } from "util";
import AdminPictureWheel from "@/components/ProductionInfo/AdminPictureWheel.vue";
export default {
  name: "PicGrid",
  components: { AdminPictureWheel },
  data() {
    return {
      productions: [],
      theaters: [],
      TheaterID: this.$attrs.TheaterID,
      viewPix: false,
      currentProd: Number
    };
  },
  props: {
    today: Date
  },
  async mounted() {
    await axios
      .get(
        "https://api.broadwaybuilder.xyz/production/getProductions?previousDate=3%2F23%2F2019"
      )
      .then(response => (this.productions = response.data));
  },
  methods: {
    viewPics(ProductionID) {
      this.currentProd = ProductionID;
      this.viewPix = !this.viewPix;
    },
    goToPictures(theater) {
      this.$router.push({
        name: "userproductioninfo",
        params: {
          TheaterID: theater.TheaterID
        }
      });
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