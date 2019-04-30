<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">{{ confirmationTitle }}</p>
        <button class="delete" aria-label="close" @click="$emit('cancel')"></button>
      </header>
      <section class="modal-card-body">{{ confirmationMessage }}</section>
      <footer class="modal-card-foot">
        <button class="button is-success" v-if="notifyUser == false" @click="removeJobPosting()">Yes</button>
        <button class="button" v-if="notifyUser == false" @click="$emit('cancel')">No</button>
        <div class="notification" v-if="notifyUser">
          <strong>{{ notifyMessage }}</strong>
        </div>
      </footer>
    </div>

  </div>
</template>

<script>
import axios from "axios";
import { setTimeout } from "timers";

export default {
  props: ["helpWantedId"],
  data() {
    return {
      confirmationTitle: "Are you sure you want to delete this job?",
      confirmationMessage:
        "This will delete all data associated with a job such as resume submissions and information. This cannot be undone!",
      notifyUser: false,
      notifyMessage: ""
    };
  },
  methods: {
    async removeJobPosting() {
      // Removes a job posting from the database
      await axios
        .delete("https://api.broadwaybuilder.xyz/helpwanted/deletetheaterjob", {
          params: {
            helpWantedId: this.helpWantedId
          }
        })
        .then(
          response => (
            (this.notifyUser = true),
            (this.notifyMessage = response.data),
            setTimeout(() => {
              this.$emit("cancel");
            }, 3000)
          )
        );
    }
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.modal-background
  background-color: rgba(0, 0, 0, 0.1);

.modal
  opacity: 1
  transition: all 3s ease 0s;
</style>

