<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">Are you sure you want to delete this job?</p>
        <button class="delete" aria-label="close" @click="$emit('cancel')"></button>
      </header>
      <section class="modal-card-body">This will delete all data associated with a job such as resume submissions and information. This cannot be undone!</section>
      <footer class="modal-card-foot">
        <button class="button is-success" @click="removeJobPosting()">Yes</button>
        <button class="button" @click="$emit('cancel')">No</button>
      </footer>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: ["helpWantedId"],
  methods: {
    async removeJobPosting() {
      // Removes a job posting from the database
      await axios
        .delete(
          "https://api.broadwaybuilder.xyz/helpwanted/deletetheaterjob/" +
            this.helpWantedId
        )
        .then(response => console.log(response.data));
    }
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.modal-background
  background-color: rgba(0, 0, 0, 0.1);
</style>

