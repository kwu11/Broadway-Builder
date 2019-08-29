<!-- App.vue -->

<!-- HTML Template -->
<template>
  <div id="app">
    <div class="file has-name is-boxed">
      <label class="file-label">
        <input class="file-input" type="file" name="resume" @change="getFileName()">
        <span class="file-cta">
          <span class="file-icon">
            <FontAwesomeIcon icon="cloud-upload-alt"/>
          </span>
          <span class="file-label">Click to upload</span>
        </span>
        <span class="file-name">{{ fileName }}</span>
      </label>
    </div>
  </div>
</template>

<!-- Javascript -->
<script>
export default {
  date() {
    return {
      fileName: ""
    };
  },
  methods: {
    getFileName() {
      this.fileName = document.getElementsByClassName("file-input")[0].value;
    }
  }
};
</script>

<!-- SASS styling -->
<style lang="scss">
.dropbox {
  outline: 2px dashed grey; /* the dash box */
  outline-offset: -10px;
  background: lightcyan;
  color: dimgray;
  padding: 10px 10px;
  min-height: 200px; /* minimum height */
  position: relative;
  cursor: pointer;
}

.input-file {
  opacity: 0; /* invisible but it's there! */
  width: 100%;
  height: 200px;
  position: absolute;
  cursor: pointer;
}

.dropbox:hover {
  background: lightblue; /* when mouse over to the drop zone, change color */
}

.dropbox p {
  font-size: 1.2em;
  text-align: center;
  padding: 50px 0;
}
</style>