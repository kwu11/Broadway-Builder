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
          <th></th>
          <th></th>
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
            <button class="button" v-on:click="showModal" v-bind:passedTheater="theater">EDIT</button>
          </td>
          <td>
            <button class="button" v-on:click="deleteTheater(theater)">DELETE</button>
          </td>
          <TheatersTableModal v-if="isModalVisible" v-bind:passedTheater="theaters[index]" @close="closeModal"/>
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
    },
    showModal() {
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
a
 color: black

</style>

