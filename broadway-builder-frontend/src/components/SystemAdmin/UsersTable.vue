<template>
  <div class="UsersTable">

    <v-data-table :headers="headers" :items="users" class="elevation-1">
      <template v-slot:items="props">
        <td>{{props.item.UserId}}</td>
        <td>{{props.item.Username}}</td>
        <td>{{props.item.FirstName}}</td>
        <td> {{props.item.LastName}}</td>
        <td>{{props.item.StreetAddress}} {{props.item.City}}, {{props.item.State}} {{props.item.Country}}</td>
        <td>
          <a @click="showModal(props.item)">
            <img src="@/assets/edit.png" alt="Edit">
          </a>
        </td>
        <td>
          <a v-on:click="deleteUser(props.item)">
            <img src="@/assets/tester.png" alt="Delete">
          </a>
        </td>
      </template>
    </v-data-table>    

    <UsersTableModal v-if="isModalVisible" v-bind:passedUser="modalUser" @close="closeModal"/>
  </div>
</template>

<script>
import axios from "axios";
import UsersTableModal from "@/components/SystemAdmin/UsersTableModal.vue";
export default {
  name: "UsersTable",
  components: {
    UsersTableModal
  },
  data() {
    return {
      users: [],
      modalUser: null,
      isModalVisible: false,
            headers: [
        {
          text: "User ID",
          align: "left",
          value: "UserID"
        },
        {
          text: "Email",
          align: "left",
          sortable: false,
          value: "Username"
        },
        {
          text: "First Name",
          align: "left",
          value: "FirstName"
        },
        {
          text: "Last Name",
          align: "left",
          value: "LastName"
        },
        {
          text: "Street Address",
          align: "left",
          value: "Street Address"
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
      .get("https://api.broadwaybuilder.xyz/user/all")
      .then(response => (this.users = response.data));
  },
  methods: {
    async deleteUser(user) {
      await axios
        .delete("https://api.broadwaybuilder.xyz/user/deleteUser",{data: user})
        .then(response => alert(response.data));
        this.$forceUpdate();
    },
    showModal(user) {
      this.modalUser = user;
      this.isModalVisible = true;
    },
    closeModal() {
      this.isModalVisible = false;
    },
    cancelManageUsers() {
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

