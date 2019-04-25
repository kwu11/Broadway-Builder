<template>
<body>
  <div class="adminaccount">
    <div class="columns">
      <div class="column is-3">
        <aside class="menu" style="padding-left: 1em">
          <p style="padding-top: 15px" class="menu-label">Administration</p>
          <ul class="menu-list">
            <li>
              <a v-on:click="editTheaterComp">Edit Profile</a>
            </li>
          </ul>
          <p class="menu-label">Manage Productions</p>
          <ul class="menu-list">
            <li>
              <a v-on:click="createProductionForTheater">Create Production</a>
            </li>
            <li>
              <a v-on:click="displayProductions">Display Productions</a>
            </li>
          </ul>
        </aside>
      </div>
      <div class="column is-9">
        <section class="hero is-info welcome is-small">
          <div class="hero-body">
            <div class="container">
              <h1 class="title">Hello, Admin.</h1>
              <h2 class="subtitle">I hope you are having a great day!</h2>
            </div>
          </div>
        </section>

        <ProductionsTable v-if="viewProductions === true"/>

        <div class="columns is-9">
          <EditTheater v-if="editTheater === true" v-bind:theater="theater"/>
          <CreateProduction v-if="createProduction === true" v-bind:theater="theater"/>
        </div>
      </div>
    </div>
  </div>
</body>
</template>

<script>
import EditTheater from "@/components/Admin/EditTheater.vue";
import CreateProduction from "@/components/Admin/CreateProduction.vue";
import ProductionsTable from "@/components/Admin/ProductionsTable.vue";
export default {
  name: "AdminAccount",
  components: {
    EditTheater,
    CreateProduction,
    ProductionsTable
  },
  data() {
    return {
      editTheater: false,
      createProduction: false,
      viewProductions: true,
      theater: {
        TheaterName: "Theater",
        CompanyName: "Company",
        StreetAddress: "123 Testfield Way",
        City: "Long Beach",
        State: "CA",
        Country: "USA",
        PhoneNumber: "5555555555"
      }
    };
  },
  mounted() {},
  methods: {
    editTheaterComp() {
      this.viewProductions = false;
      this.createProduction = false;
      this.editTheater = !this.editTheater;
    },
    createProductionForTheater() {
      this.viewProductions = false;
      this.editTheaterComp = false;
      this.createProduction = !this.createProduction;
    },
    displayProductions() {
      this.editTheaterComp = false;
      this.createProduction = false;
      this.viewProductions = !this.viewProductions;
    }
  }
};
</script>

<style lang="sass">
a:hover
  font-weight: bold

CreateProduction
  padding-top: 1em
</style>
