<template>
  <div class="UsersTable">
    <table class="table is-hoverable">
      <thead>
        <tr>Users</tr>
        <tr>
          <th>User ID</th>
          <th>User Name</th>
          <th>First Name</th>
          <th>Last Name<th>
          <th>Age</th>
          <th>Address</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
      </thead>
      <tbody v-for="(user, index) in users" :key="index">
        <tr>
          <td>{{user.UserId}}</td>
          <td>{{user.Username}}</td>
          <td>{{user.FirstName}}</td>
          <td>{{user.LastName}}</td>
          <td>{{user.Age}}</td>
          <td>{{user.StreetAddress}} {{user.City}}, {{user.StateProvince}} {{user.Country}}</td>
          <td>
            <a v-on:click="showModal(user)">
              <img src="@/assets/edit.png" alt="Edit">
            </a>
            <UsersTableModal v-if="isModalVisible" v-bind:passedUser="modalUser" @close="closeModal"/>
          </td>
          <td>
            <a v-on:click="deleteUser(user)">
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
      isModalVisible: false
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

