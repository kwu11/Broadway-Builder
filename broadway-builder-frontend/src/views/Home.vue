<template>
  <div class="home">
    <!-- <SearchBar /> -->
    <v-divider></v-divider>

    <v-parallax style="margin: 1em" dark src="https://img-new.cgtrader.com/items/769294/51f124929c/theatre-stage-3d-model-obj-3ds-fbx-blend-dae.jpg">
      <v-layout align-center column justify-center>
        <h1 class="display-2 font-weight-thin mb-3">Welcome</h1>
        <h4 class="subheading">Build your application today!</h4>
      </v-layout>
    </v-parallax>
  </div>
</template>

<script>
// @ is an alias to /src
import SearchBar from "@/components/SearchBar.vue";

export default {
  name: "home",
  components: {
    SearchBar
  }
};
</script>
