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
          <p class="menu-label">Help Wanted</p>
          <ul class="menu-list">
            <li>
              <router-link to="/theater/{theaterid}/helpwanted">Job Postings</router-link>
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

        <div class="columns">
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
      this.editTheater = !this.editTheater;
    },
    createProductionForTheater() {
      this.createProduction = !this.createProduction;
    },
    displayProductions() {
      this.viewProductions = !this.viewProductions;
    }
  }
};
</script>

<style lang="sass">
.table
  &.is-scrollable
    tbody
      overflow-y: scroll
      width: 100%
      position: absolute
.card
  &.events-card
    background-color: white
    -webkit-box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1)
    box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1)
    color: #4a4a4a

a:hover
  font-weight: bold
</style>
