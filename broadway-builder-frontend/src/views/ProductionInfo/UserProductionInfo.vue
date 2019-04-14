<template>
  <div class="UserProductionInfo">
    <h1>{{theater.TheaterName}} |</h1>
    {{theater.CompanyName}}.
    <PicGrid v-bind:TheaterID="TheaterID" :today="today"/>
  </div>
</template>

<script>
import PicGrid from "@/components/ProductionInfo/PicGrid.vue";
import AdminPictureWheel from "@/components/ProductionInfo/AdminPictureWheel.vue";
import axios from "axios";
import { isDate } from "util";

export default {
  name: "UserProductionInfo",
  components: {
    PicGrid
  },
  data() {
    return {
      TheaterID: this.$route.params.TheaterID,
      theater: {},
      today: new Date("2019-12-30T10:20:20Z")
    };
  },
  async mounted() {
    await axios
      .get("https://api.broadwaybuilder.xyz/theater/get/" + this.TheaterID)
      .then(response => (this.theater = response.data));
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

h1 
  font-size: 3em


</style>