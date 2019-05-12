<template>
  <div class="TheatersTable">
    <v-data-table :headers="headers" :items="theaters" class="elevation-1">
      <template v-slot:items="props">
        <td>{{props.item.TheaterID}}</td>
        <td>{{props.item.TheaterName}}</td>
        <td>{{props.item.CompanyName}}</td>
        <td>{{props.item.StreetAddress}} {{props.item.City}}, {{props.item.State}} {{props.item.Country}}</td>
        <td>{{props.item.PhoneNumber}}</td>
        <td>
          <a @click="showModal(props.item)">
            <img src="@/assets/edit.png" alt="Edit">
          </a>
        </td>
        <td>
          <a v-on:click="deleteTheater(props.item)">
            <img src="@/assets/tester.png" alt="Delete">
          </a>
        </td>
      </template>
    </v-data-table>
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
      isModalVisible: false,
      headers: [
        {
          text: "Theater ID",
          align: "left",
          value: "TheaterID"
        },
        {
          text: "Theater Name",
          align: "left",
          sortable: false,
          value: "TheaterName"
        },
        {
          text: "Company Name",
          align: "left",
          sortable: false
        },
        {
          text: "Address",
          align: "left",
          sortable: false
        },
        {
          text: "Phone Number",
          align: "left",
          sortable: false
        },
        {
          text: "Edit",
          align: "left",
          sortable: false
        },
        {
          text: "Delete",
          align: "left",
          sortable: false
        }
      ]
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

