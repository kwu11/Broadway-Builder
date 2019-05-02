<template>
  <div class="TheatersTable">
    <table class="table is-hoverable">
      <thead>
        <tr>Theaters</tr>
        <tr>
          <th>Theater ID</th>
          <th>Theater Name</th>
          <th>Company Name</th>
          <th>Address</th>
          <th>Phone Number</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
      </thead>
      <tbody v-for="(theater, index) in theaters" :key="index">
        <tr>
          <td>{{theater.TheaterID}}</td>
          <td>{{theater.TheaterName}}</td>
          <td>{{theater.CompanyName}}</td>
          <td>{{theater.StreetAddress}} {{theater.City}}, {{theater.State}} {{theater.Country}}</td>
          <td>{{theater.PhoneNumber}}</td>
          <td>
            <a v-on:click="showModal(theater)">
              <img src="@/assets/edit.png" alt="Edit">
            </a>
            <TheatersTableModal v-if="isModalVisible" v-bind:passedTheater="modalTheater" @close="closeModal"/>
          </td>
          <td>
            <a v-on:click="deleteTheater(theater)">
              <img src="@/assets/tester.png" alt="Delete">
            </a>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import axios from "axios";
import TheatersTableModal from "@/components/SystemAdmin/TheatersTableModal.vue";
export default {
  name: "TheatersTable",
  components: {
    TheatersTableModal
  },
  data() {
    return {
      theaters: [],
      modalTheater: null,
      isModalVisible: false
    };
  },
  async mounted() {
    await axios
      .get("https://api.broadwaybuilder.xyz/theater/all")
      .then(response => (this.theaters = response.data));
  },
  methods: {
    async deleteTheater(theater) {
      await axios
        .delete("https://api.broadwaybuilder.xyz/theater/deleteTheater",{data: theater})
        .then(response => alert(response.data));
        this.$forceUpdate();
    },
    showModal(theater) {
      this.modalTheater = theater;
      this.isModalVisible = true;
    },
    closeModal() {
      this.isModalVisible = false;
    },
    cancelManageTheaters() {
        this.$emit("cancel", false);
    }
  }
};
</script>

<style lang="sass" scoped>

img
 width: 2em
 height: 2em


</style>

