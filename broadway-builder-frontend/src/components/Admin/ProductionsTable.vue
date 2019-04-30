<template>
  <div class="ProductionsTable">
    <v-app id="inspire">
      <v-toolbar flat color="white">
        <v-toolbar-title>Productions</v-toolbar-title>
        <v-divider class="mx-2" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-dialog v-model="dialog" max-width="600px">
          <template v-slot:activator="{on}">
            <v-btn color="primary" dark class="mb-2" v-on="on">New Production</v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">New Production</span>
            </v-card-title>
            <v-card-text>
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model="editedProduction.productionName" label="Production Name"></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field v-model="editedProduction.theaterID" label="Theater ID"></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field
                      v-model="editedProduction.directorFirstName"
                      label="Director First Name"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md4>
                    <v-text-field
                      v-model="editedProduction.directorLastName"
                      label="Director Last Name"
                    ></v-text-field>
                  </v-flex>
                  <v-flex>
                    <v-text-field v-model="editedProduction.street" label="Street"></v-text-field>
                  </v-flex>
                  <v-flex>
                    <v-text-field v-model="editedProduction.city" label="City"></v-text-field>
                  </v-flex>
                  <v-flex>
                    <v-text-field v-model="editedProduction.zipcode" label="Zipcode"></v-text-field>
                  </v-flex>
                  <v-flex>
                    <v-text-field v-model="editedProduction.country" label="Country"></v-text-field>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" flat @click="close">Cancel</v-btn>
              <v-btn color="blue darken-1" flat @click="confirm">Confirm</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>

      <v-data-table :headers="headers" :items="productions" class="elevation-1">
        <template v-slot:items="props">
          <td>{{props.item.ProductionID}}</td>
          <td>{{props.item.ProductionName}}</td>
          <td>{{props.item.TheaterID}}</td>
          <td>{{props.item.DirectorFirstName}} {{props.item.DirectorLastName}}</td>
          <td>{{props.item.Street}}, {{props.item.City}}, {{props.item.StateProvince}} {{props.item.Zipcode}}</td>
          <td>
            <a>
              <img src="@/assets/edit.png" alt="Edit">
            </a>
          </td>
          <td>
            <a v-on:click="deleteProduction(props.item.ProductionID)">
              <img src="@/assets/tester.png" alt="Delete">
            </a>
          </td>
          <td>
            <a v-on:click="programIDSelect(props.item.ProductionID)">
              <img src="@/assets/upload.png" alt="Upload">
            </a>
            <div v-if="programID === props.item.ProductionID">
              <input type="file" ref="file" id="file" v-on:change="onFileChange()">
              <div class="button is-primary" v-on:click="uploadProgram(programID)">Submit</div>
            </div>
          </td>
        </template>
      </v-data-table>
    </v-app>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "ProductionsTable",
  data() {
    return {
      productions: [],
      dialog: false,

      file: "",
      programID: 0,
      currentPage: 1,
      minPage: 1,
      maxPage: 1,
      numberOfItems: 5,

      prod: [],
      editedItem: -1,
      editedProduction: {
        productionName: "",
        theaterID: 1,
        directorFirstName: "",
        directorLastName: "",
        street: "",
        city: "",
        stateProvince: "",
        zipcode: "",
        country: ""
      },
      defaultProduction: {
        productionName: "",
        theaterID: 1,
        directorFirstName: "",
        directorLastName: "",
        street: "",
        city: "",
        stateProvince: "",
        zipcode: "",
        country: ""
      },
      headers: [
        {
          text: "Production ID",
          align: "right",
          value: "ProductionID"
        },
        {
          text: "Production Name",
          align: "left",
          sortable: false,
          value: "ProductionName"
        },
        {
          text: "Theater ID",
          align: "right",
          value: "TheaterID"
        },
        {
          text: "Director",
          align: "right",
          sortable: false
        },
        {
          text: "Address",
          align: "right",
          sortable: false
        },
        {
          text: "Edit",
          align: "right",
          sortable: false
        },
        {
          text: "Delete",
          align: "right",
          sortable: false
        },
        {
          text: "Upload Program",
          align: "right",
          sortable: false
        }
      ]
    };
  },
  async mounted() {
    this.getProductions();
  },
  methods: {
    async deleteProduction(ProductionID) {
      if (confirm("Do you really wish to delete?")) {
        await axios
          .delete(
            "https://api.broadwaybuilder.xyz/production/delete/" + ProductionID
          )
          .then(alert("Production Successfully Deleted"));
      }
    },
    async uploadProgram(ProductionID) {
      let formData = new FormData();
      formData.append("file", this.file);
      await axios
        .put(
          "https://api.broadwaybuilder.xyz/production/" +
            ProductionID +
            "/uploadProgram",
          formData
        )
        .then(function() {
          console.log("Success!");
        })
        .catch(function() {
          console.log("Failure!");
        });
    },
    async getProductions() {
      await axios
        .get(
          "https://api.broadwaybuilder.xyz/production/getProductions?currentDate=3%2F23%2F2019"
        )
        .then(response => (this.productions = response.data));
    },
    async createProduction(createdProduction) {
      await axios
        .post(
          "https://api.broadwaybuilder.xyz/production/create",
          createdProduction
        )
        .then(function() {
          console.log("New Production Created!");
        })
        .catch(function() {
          console.log("Failure.");
        });
    },
    onFileChange() {
      this.file = this.$refs.file.files[0];
    },
    programIDSelect(id) {
      this.programID = id;
    },
    close() {
      this.dialog = false;
      setTimeout(() => {
        this.editedProduction = Object.assign({}, this.defaultProduction);
        this.editedIndex = -1;
      }, 300);
    },
    confirm() {
      if (this.editedIndex > -1) {
        Object.assign(
          this.productions[this.editedIndex],
          this.editedProduction
        );
      } else {
        this.productions.push(this.editedProduction);
      }
      this.close();
    }
  }
};
</script>

<style lang="sass" scoped>

img
 width: 2em
 height: 2em


</style>

