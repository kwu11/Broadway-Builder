<template>
  <!-- Clicking on the image goes to that theater -->
  <v-card>
    <v-img
      id="clickableImg"
      :src="imgUrl"
      height="150px"
      @click="goToProfile()"
    >
    </v-img>

    <v-card-title primary-title>
      <div @click="goToProfile()">
        <div class="headline">{{ theater.TheaterName }}</div>
        <span class="grey--text">{{ theater.CompanyName }}</span>
      </div>
      <v-spacer></v-spacer>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn small depressed text v-on:click="toggleShow()">
          {{ button.text }}
        </v-btn>
      </v-card-actions>
    </v-card-title>

    <!-- Open description -->
    <v-expand-transition >
      <div v-show="isActive">
        <v-divider></v-divider>
        <v-card-text>
          {{ descriptionText }}
        </v-card-text>
      </div>
    </v-expand-transition>
  </v-card>
</template>

<script>
import Vue from "vue";
import ReadMore from "vue-read-more";
Vue.use(ReadMore);

Vue.component("TheaterCard");

export default {
  name: "TheaterCard",
  components: {},
  props: ['theater'],
  data() {
    return {
      theaterInfo: this.theater,
      isActive: false,
      imgUrl: "",
      button: {
        text: "Show Description"
      },
      descriptionText: "Add a description here."
    };
  },
  methods: {
    toggleShow: function() {
      this.isActive = !this.isActive;
      this.button.text = this.isActive ? "Hide Description" : "Show Description";
    },
    goToProfile() {
      this.$router.push({
        name: "theater",
        params: {
          TheaterID: this.theaterInfo.TheaterID,
          ProfilePic: this.imgUrl
        }
      });
    },
    generateProfileImage() {
      let num = Math.floor(Math.random()*1000);
      return `https://picsum.photos/500/300?image=${num}`;
    }
  },
  mounted() {
    this.imgUrl = this.generateProfileImage();
  }
};
</script>

<style scoped>
  #clickableImg {
    cursor: pointer;
  }
</style>