<template>
  <div class="ProductionsTable">
    <table class="table is-hoverable">
      <thead>
        <tr>Productions</tr>
        <tr>
          <th>Theater ID</th>
          <th>Production ID</th>
          <th>Production Name</th>
          <th>Director</th>
          <th>Address</th>
          <th>Created</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
      </thead>
      <tbody v-for="(production, index) in productions" :key="index">
        <tr>
          <td>{{production.TheaterID}}</td>
          <td>{{production.ProductionID}}</td>
          <td>{{production.ProductionName}}</td>
          <td>{{production.DirectorFirstName}} {{production.DirectorLastName}}</td>
          <td>{{production.Street}}, {{production.City}}, {{production.StateProvince}} {{production.Zipcode}}</td>
          <td>{{production.DateTimes[0].Date}}</td>
          <td>
            <a v-on:click="showModal">editing</a>
          </td>

          <td>
            <a v-on:click="deleteProduction(production.ProductionID)">deleting</a>
          </td>
        </tr>
      </tbody>
    </table>
    <modal v-show="isModalVisible" @close="closeModal"/>
  </div>
</template>

<script>
import axios from "axios";
import Modal from "@/components/Modal.vue";
export default {
  name: "ProductionsTable",
  components: {
    Modal
  },
  data() {
    return {
      productions: [],
      isModalVisible: false
    };
  },
  async mounted() {
    await axios
      .get(
        "https://api.broadwaybuilder.xyz/production/getProductions?currentDate=3%2F23%2F2019"
      )
      .then(response => (this.productions = response.data));
  },
  methods: {
    async deleteProduction(ProductionID) {
      await axios
        .delete(
          "https://api.broadwaybuilder.xyz/production/delete/" + ProductionID
        )
        .then(alert("Production Successfully Deleted"));
    },
    showModal() {
      this.isModalVisible = true;
    },
    closeModal() {
      this.isModalVisible = false;
    }
  }
};
</script>

<style lang="sass" scoped>
a
 color: black

</style>

