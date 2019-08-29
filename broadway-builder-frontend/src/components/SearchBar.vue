<template>
  <div class="SearchBar">
    <h1>{{ info }}</h1>
    <div class="control">
      <h1>Search Bar</h1>
      <input
        class="input is-medium is-rounded"
        type="text"
        placeholder="Theatre, company, city, etc.."
      >
    </div>
  </div>
</template>

<script>
export default {
  name: "SearchBar",
  data() {
    return {
      info: ""
    };
  },
  mounted() {}
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}

h1 {
  font-size: 2em;
}

.control {
  padding: 2em;
}
</style>
