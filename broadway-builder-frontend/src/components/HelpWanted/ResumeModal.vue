<template>
  <!-- Modal to show the resumes for a job posting -->
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">Resumes</p>
        <button class="delete" aria-label="close" @click="$emit('cancel')"></button>
      </header>
      <section class="modal-card-body">
        <!-- Content ... -->
        <div v-for="(resume, index) in resumes" :key="index">
          <a :href="resume" target="_blank">Resume {{index + 1}}</a>
        </div>
      </section>
      <footer class="modal-card-foot"></footer>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: ["helpWantedId"],
  data() {
    return {
      resumes: []
    };
  },
  methods: {
    async getJobApplicants() {
      await axios
        .get(
          "https://api.broadwaybuilder.xyz/helpwanted/getResumesForJob/" +
            this.helpWantedId
        )
        .then(response => (this.resumes = response.data));
    }
  },
  created() {
    this.getJobApplicants();
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.modal-background
  background-color: rgba(0, 0, 0, 0.1);
</style>

