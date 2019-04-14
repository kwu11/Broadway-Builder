<template>
  <div class="AdminPictureWheel">
    <v-carousel>
      <v-carousel-item
        v-for="(pic,i) in pics"
        :key="i"
        :src="pic"
        reverse-transition="fade"
        transition="fade"
      ></v-carousel-item>
    </v-carousel>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "AdminPictureWheel",
  data() {
    return {
      pics: [],
      viewPix: true,
      currentProd: this.$attrs.currentProd
    };
  },
  async mounted() {
    await axios
      .get(
        "https://api.broadwaybuilder.xyz/production/" +
          this.currentProd +
          "/getPhotos"
      )
      .then(response => (this.pics = response.data));
  }
};
</script>

<style lang="stylus">
#example-custom-transition {
  .fade {
    &-enter-active, &-leave-active, &-leave-to {
      transition: 0.3s ease-out;
      position: absolute;
      top: 0;
      left: 0;
    }

    &-enter, &-leave, &-leave-to {
      opacity: 0;
    }
  }
}
</style>