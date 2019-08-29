<template>
  <v-navigation-drawer :mini-variant.sync="mini" stateless value="true" absolute floating style="height: auto; top: 81px; background-color: #fafafa;">
    <v-list>
      <v-list-tile>
        <v-list-tile-action>
          <v-btn icon @click.stop="miniMode">
            <v-icon>home</v-icon>
          </v-btn>
        </v-list-tile-action>
        <v-list-tile-title>System Admin Dashboard</v-list-tile-title>
      </v-list-tile>

      <v-list-tile v-for="(admin, i) in admins" :key="i" @click="$emit(admin[1])" >
        <v-list-tile-action></v-list-tile-action>
        <v-list-tile-title v-text="admin[0]"></v-list-tile-title>
      </v-list-tile>

      <v-list-group no-action sub-group v-model="theaterManagementDropdownState">
        <template v-slot:activator>
          <v-list-tile>            
            <v-list-tile-title>Theater Management</v-list-tile-title>
          </v-list-tile>
        </template>

        <v-list-tile v-for="(theaterMenuItem, i) in theaterMenuItems" :key="i" @click="$emit(theaterMenuItem[1])" >
          <v-list-tile-title v-text="theaterMenuItem[0]"></v-list-tile-title>
        </v-list-tile>
      </v-list-group>


      <v-list-group sub-group no-action v-model="userManagementDropdownState">
        <template v-slot:activator>
          <v-list-tile>
            <v-list-tile-title>User Management</v-list-tile-title>
          </v-list-tile>
        </template>
        <v-list-tile v-for="(crudItem, i) in cruds" :key="i" @click="$emit(crudItem[1])">
          <v-list-tile-title v-text="crudItem[0]"></v-list-tile-title>
        </v-list-tile>
      </v-list-group>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
export default {
  data: () => ({
    admins: [
      ["Publish Site", "publishSite"]
    ],
    theaterMenuItems: [
      ["Create Theater", "createTheater"],
      ["Manage Theaters", "manageTheaters"]
    ],
    cruds: [
      ["Manage Users", "manageUsers"]
    ],
    mini: false,
    theaterManagementDropdownState: true,
    userManagementDropdownState: true
  }),
  methods: {
    miniMode() {
      this.mini = !this.mini;
      if (!this.mini) {
        this.theaterManagementDropdownState = true;
        this.userManagementDropdownState = true;
      } else {
        this.theaterManagementDropdownState = false;
        this.userManagementDropdownState = false;
      }
    }
  }
};
</script>