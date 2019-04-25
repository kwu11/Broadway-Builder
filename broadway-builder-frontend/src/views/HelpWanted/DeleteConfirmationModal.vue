<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">Are you sure you want to delete this job?</p>
        <button class="delete" aria-label="close" @click="$emit('cancel')"></button>
      </header>
      <section
        class="modal-card-body"
      >This will delete all data associated with a job such as resume submissions and information. This cannot be undone!</section>
      <footer class="modal-card-foot">
        <button class="button is-success" @click="$emit('confirmation')">I'm Sure</button>
        <button class="button" @click="$emit('cancel')">This is a mistake!</button>
      </footer>
    </div>
  </div>
</template>

<script>
export default {
  props: ["helpWantedId"]
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.modal-background
  background-color: rgba(0, 0, 0, 0.1);
</style>

