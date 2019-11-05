<template>
  <v-navigation-drawer
    :mini-variant.sync="mini"
    stateless
    value="true"
    absolute
    floating
    style="height: auto; top: 81px; background-color: #fafafa;"
  >
    <v-list>
      <v-list-tile>
        <v-list-tile-action>
          <v-btn icon @click.stop="miniMode">
            <v-icon>menu</v-icon>
          </v-btn>
        </v-list-tile-action>
        <v-list-tile-title>Admin Dashboard</v-list-tile-title>
      </v-list-tile>

      <v-list-group no-action sub-group v-model="theaterManagementDropdownState">
        <template v-slot:activator>
          <v-list-tile>
            <v-list-tile-title>Theater Management</v-list-tile-title>
          </v-list-tile>
        </template>

        <v-list-tile v-for="(admin, i) in admins" :key="i" @click="$emit('productions')">
          <v-list-tile-title v-text="admin[0]"></v-list-tile-title>
        </v-list-tile>
      </v-list-group>

      <v-list-group sub-group no-action v-model="userManagementDropdownState">
        <template v-slot:activator>
          <v-list-tile>
            <v-list-tile-title>User Management</v-list-tile-title>
          </v-list-tile>
        </template>
        <v-list-tile v-for="(crud, i) in cruds" :key="i">
          <v-list-tile-title v-text="crud[0]"></v-list-tile-title>
        </v-list-tile>
      </v-list-group>

      <v-list-tile @click="$emit('analytics')">
        <v-list-tile-action></v-list-tile-action>
        <v-list-tile-title>Analytics</v-list-tile-title>
      </v-list-tile>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
export default {
  data: () => ({
    admins: [["Productions", "people_outline"], ["Help Wanted", "settings"]],
    cruds: [
      ["Create", "add"],
      ["Read", "insert_drive_file"],
      ["Update", "update"],
      ["Delete", "delete"]
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