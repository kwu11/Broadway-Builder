<template>
  <v-layout wrap>
    <v-container id="menu-row">
      <v-layout justify-left>
        <v-btn id="menu-btn" flat @click.stop="drawer = !dnpmrawer">
          <v-icon>menu</v-icon>
        </v-btn>
      </v-layout>
    </v-container>

    <v-navigation-drawer v-model="drawer" absolute temporary style="height: auto; top: 4em;">
      <v-list>
        <v-list-tile
          @click="drawer = !drawer">
          <v-btn icon>
            <v-icon></v-icon>
          </v-btn>
          <v-list-tile-title>Admin Dashboard</v-list-tile-title>
        </v-list-tile>

        <v-list-group no-action sub-group v-model="theaterManagementDropdownState">
          <template v-slot:activator>
            <v-list-tile>
              <v-list-tile-title>Theater Management</v-list-tile-title>
            </v-list-tile>
          </template>

          <v-list-tile
            v-for="(admin, i) in admins"
            :key="i"
            @click="$emit('productions'), drawer = !drawer"
          >
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

        <v-list-tile
          @click="$emit('analytics'),
          drawer = !drawer">
          <v-list-tile-action></v-list-tile-action>
          <v-list-tile-title>Analytics</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>
  </v-layout>
</template>

<script>
export default {
  data: () => ({
    drawer: null,
    admins: [["Productions", "people_outline"], ["Help Wanted", "settings"]],
    cruds: [
      ["Create", "add"],
      ["Read", "insert_drive_file"],
      ["Update", "update"],
      ["Delete", "delete"]
    ],
    theaterManagementDropdownState: true,
    userManagementDropdownState: true
  })
};
</script>

<style lang="css">
#menu-btn {
  margin: 0px 100% 0px 0px;
  padding: 0em 4em 0em 0em;
}

#menu-row {
  padding: 0px 0px 0px 0px;
}
</style>