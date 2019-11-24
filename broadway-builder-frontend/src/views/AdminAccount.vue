<template>
  <div id="admin-account">
    <!-- Small screen view, e.g., mobile phones, iPad help vertically -->
    <v-container class="hidden-md-and-up" text-xs-center fluid>
      <v-layout row wrap>
        <v-flex xs12>
          <AccountSideBarSmall
            @productions="viewProductions = true; viewAnalytics = false;"
            @analytics="viewProductions = false; viewAnalytics = true;"
          />
        </v-flex>
        <v-flex xs12>
          <ProductionsTable v-if="viewProductions" />
          <AnalyticsDashboard v-if="viewAnalytics" />
        </v-flex>
      </v-layout>
    </v-container>

    <!-- Large screen view, e.g., iPad held horizontally, desktops, etc -->
    <v-container class="hidden-sm-and-down" text-xs-center fluid>
      <v-layout row wrap>
        <v-flex xs3>
          <AccountSideBar
            @productions="viewProductions = true; viewAnalytics = false;"
            @analytics="viewProductions = false; viewAnalytics = true;"
          />
        </v-flex>
        <v-flex xs9>
          <ProductionsTable v-if="viewProductions" />
          <AnalyticsDashboard v-if="viewAnalytics" />
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import AccountSideBar from "@/components/AccountSideBar.vue";
import AccountSideBarSmall from "@/components/AccountSideBarSmall.vue";
import EditTheater from "@/components/Admin/EditTheater.vue";
import CreateProduction from "@/components/Admin/CreateProduction.vue";
import ProductionsTable from "@/components/Admin/ProductionsTable.vue";
import AnalyticsDashboard from "@/components/Admin/AnalyticsDashboard.vue";

export default {
  name: "AdminAccount",
  components: {
    EditTheater,
    CreateProduction,
    ProductionsTable,
    AccountSideBar,
    AccountSideBarSmall,
    AnalyticsDashboard
  },
  data() {
    return {
      editTheater: false,
      createProduction: false,
      viewProductions: true,
      viewAnalytics: false,
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
