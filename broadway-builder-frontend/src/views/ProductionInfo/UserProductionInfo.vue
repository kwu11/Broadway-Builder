<template>
  <div class="UserProductionInfo">
    <h1 class="hidden-sm-and-down">Past Productions | {{ theater.TheaterName }}</h1>
    <h2 class="hidden-md-and-up">Past Productions | {{ theater.TheaterName }}</h2>

    <PicGrid v-bind:TheaterID="TheaterID" :today="today"/>
  </div>
</template>

<script>
import PicGrid from "@/components/ProductionInfo/PicGrid.vue";
import axios from "axios";

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
  font-weight: bold
  margin-top: 1em
  text-align: center

h2
  font-size: 2em
  font-weight: bold
  margin-top: 1em
  text-align: center

</style>