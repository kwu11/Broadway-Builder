<template>
<body>
  <div id="sys-admin-account">
    <v-container text-xs-left fluid>
      <v-layout row wrap>
        <v-flex xs3>
          <SystemAdminAccountSideBar
            @publishSite="performPublish"
            @createTheater="createTheater"
            @manageTheaters="manageTheaters"
            @manageUsers="manageUsers"
          />
        </v-flex>
        <v-flex xs9>
          <PublishSite v-if="publish === true" @cancel="cancelPublish"/>
          <CreateTheater v-if="theaterCreated === true" @cancelCreateTheater="cancelCreateTheater"/>
          <!-- <DeleteTheater v-if="theaterDeleted === true" @cancelDeleteTheater="cancelDeleteTheater"/> -->
          <CreateUser v-if="userCreated === true" @cancelCreateUser="cancelCreateUser"/>
          <TheatersTable v-if="theatersManaged === true" @cancelManageTheaters="cancelManageTheaters"/>
          <UsersTable v-if="usersManaged === true" @cancelManageUsers="cancelManageUsers"/>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</body>
</template>

<script>
import PublishSite from "@/components/SystemAdmin/PublishSite.vue";
import CreateTheater from "@/components/SystemAdmin/CreateTheater.vue";
//import DeleteTheater from "@/components/SystemAdmin/DeleteTheater.vue";
import TheatersTable from "@/components/SystemAdmin/TheatersTable.vue";
import CreateUser from "@/components/SystemAdmin/CreateUser.vue";
import UsersTable from "@/components/SystemAdmin/UsersTable.vue";
import SystemAdminAccountSideBar from "@/components/SystemAdmin/SystemAdminAccountSideBar.vue";

export default {
  name: "SysAdminAccount",
  components: {
    PublishSite,
    CreateTheater,
    //DeleteTheater,
    CreateUser,
    TheatersTable,
    UsersTable,
    SystemAdminAccountSideBar
  },
  data() {
    return {
      publish: false,
      theaterCreated: false,
      theaterDeleted: false,
      theatersManaged: false,
      userCreated: false,
      usersManaged: false
    };
  },
  methods: {
    closeAll() {
      this.publish = false;
      this.theaterCreated = false;
      this.theaterDeleted = false;
      this.theatersManaged = false;
      this.usersManaged = false;
    },
    performPublish() {
      this.closeAll();
      this.publish = true;
    },
    cancelPublish() {
      this.publish = false;
    },
    createTheater() {
      this.closeAll();
      this.theaterCreated = true;
    },
    cancelCreateTheater() {
      this.theaterCreated = false;
    },
    manageTheaters() {
      this.closeAll();
      this.theatersManaged = true;
    },
    cancelManageTheaters() {
      this.theatersManaged = false;
    },
    manageUsers() {
      this.closeAll();
      this.usersManaged = true;
    },
    cancelManageUsers() {
      this.usersManaged = false;
    },
    deleteTheater() {
      this.closeAll();
      this.theaterDeleted = true;
    },
    cancelDeleteTheater() {
      this.theaterDeleted = false;
    }
  }
};
</script>

<style lang="sass">

.card
  &.events-card
    background-color: white
    -webkit-box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1)
    box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1)
    color: #4a4a4a
</style>
