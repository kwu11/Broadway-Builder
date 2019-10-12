<!-- Displays all productions associated with the theater, as well as CRUD operations for them. -->
<template>
  <div>
    <v-toolbar flat color="white">
      <v-toolbar-title>Productions</v-toolbar-title>
      <v-divider class="mx-2" inset vertical></v-divider>
      <v-spacer></v-spacer>
      <v-text-field v-model="search" append-icon="search" label="Search" single-line hide-details></v-text-field>

      <!-- Dialog with input fields for either creating or editing a production. -->
      <v-dialog v-model="dialog" max-width="600px">
        <template v-slot:activator="{on}">
          <v-btn color="primary" dark class="mb-2" v-on="on">New Production</v-btn>
        </template>
        <v-card>
          <v-card-title>
            <span class="headline">{{ formTitle }}</span>
          </v-card-title>
          <v-card-text>
            <v-container grid-list-md>
              <v-layout wrap>
                <v-flex xs12 sm6 md4>
                  <v-text-field
                    v-model="editedProduction.productionName"
                    :rules="[
                    () => !!editedProduction.productionName || 'This field is required',
                    () => !!editedProduction.productionName && editedProduction.productionName.length <=30 || 'Too many characters']"
                    label="Production Name"
                    counter="30"
                    required
                  ></v-text-field>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-text-field
                    v-model="editedProduction.theaterID"
                    :rules="[() => !!editedProduction.theaterID || 'This field is required']"
                    label="Theater ID"
                    required
                  ></v-text-field>
                </v-flex>

                <v-flex xs12 sm6 md4>
                  <v-text-field
                    v-model="editedProduction.directorFirstName"
                    :rules="[() => !!editedProduction.directorFirstName || 'This field is required',
                    ()=> !!editedProduction.directorFirstName && editedProduction.directorFirstName.length <=15 || 'Too many characters']"
                    label="Director First Name"
                    counter="15"
                    required
                  ></v-text-field>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-text-field
                    v-model="editedProduction.directorLastName"
                    :rules="[() => !!editedProduction.directorLastName || 'This field is required',
                    ()=> !!editedProduction.directorLastName && editedProduction.directorLastName.length <=15 || 'Too many characters']"
                    label="Director Last Name"
                    counter="15"
                    required
                  ></v-text-field>
                </v-flex>
                <v-flex>
                  <v-text-field
                    v-model="editedProduction.street"
                    :rules="[() => !!editedProduction.street || 'This field is required']"
                    label="Street"
                    required
                  ></v-text-field>
                </v-flex>
                <v-flex>
                  <v-text-field
                    v-model="editedProduction.city"
                    :rules="[() => !!editedProduction.city || 'This field is required']"
                    label="City"
                  ></v-text-field>
                </v-flex>
                <v-flex>
                  <v-text-field
                    v-model="editedProduction.zipcode"
                    :rules="[() => !!editedProduction.zipcode || 'This field is required']"
                    label="Zipcode"
                  ></v-text-field>
                </v-flex>
                <v-flex>
                  <v-select
                    :items="states"
                    v-model="editedProduction.stateProvince"
                    label="State/Province"
                  ></v-select>
                </v-flex>
                <v-flex>
                  <v-text-field value="United States" readonly></v-text-field>
                </v-flex>
                <v-flex v-if="editedIndex=-1">
                  <v-subheader>Select an opening date for the show:</v-subheader>
                  <v-date-picker v-model="date"></v-date-picker>
                  <v-subheader>Time of the show</v-subheader>
                  <v-text-field
                    v-model="openTime"
                    label="Start of Show:"
                    value="19:30:00"
                    type="time"
                  ></v-text-field>
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

    <!-- Setting the headers/columns for the production table. -->
    <v-data-table :headers="headers" :items="productions" :search="search" class="elevation-0">
      <template v-slot:items="props">
        <td>{{ props.item.ProductionID }}</td>
        <td>{{ props.item.ProductionName }}</td>
        <td>{{ props.item.TheaterID }}</td>
        <td>{{ props.item.DirectorFirstName }} {{ props.item.DirectorLastName }}</td>
        <td>{{ props.item.Street }}, {{ props.item.City }}, {{ props.item.StateProvince }} {{ props.item.Zipcode }}</td>
        <!-- Edit Production -->
        <td>
          <a @click="editProduction(props.item)">
            <img src="@/assets/edit.png" alt="Edit" />
          </a>
        </td>
        <!-- Delete Production -->
        <td>
          <a v-on:click="deleteProduction(props.item.ProductionID)">
            <img src="@/assets/tester.png" alt="Delete" />
          </a>
        </td>
        <!-- Upload a new program for that production. -->
        <td>
          <a
            v-if="programID != props.item.ProductionID"
            v-on:click="programIDSelect(props.item.ProductionID)"
          >
            <img src="@/assets/upload.png" alt="Upload" />
          </a>
          <div v-if="programID === props.item.ProductionID">
            <input type="file" ref="file" id="file" v-on:change="onFileChange()" />
            <v-btn color="info" v-on:click="uploadProgram(programID)">Submit</v-btn>
          </div>
        </td>
      </template>
    </v-data-table>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      //List of productions mounted at beginning of load
      productions: [],
      //date and OpenTime used to create the Date and Time of a productions performance
      date: "",
      openTime: "",
      dialog: false,
      search: "",
      file: "",
      programID: 0,
      states: [
        "AL",
        "AK",
        "AZ",
        "AR",
        "CA",
        "CO",
        "CT",
        "DE",
        "FL",
        "GA",
        "HI",
        "ID",
        "IL",
        "IN",
        "IA",
        "KS",
        "KY",
        "LA",
        "ME",
        "MD",
        "MA",
        "MI",
        "MN",
        "MS",
        "MO",
        "MT",
        "NE",
        "NV",
        "NH",
        "NJ",
        "NM",
        "NY",
        "NC",
        "ND",
        "OH",
        "OK",
        "OR",
        "PA",
        "RI",
        "SC",
        "SD",
        "TN",
        "TX",
        "UT",
        "VT",
        "VA",
        "WA",
        "WV",
        "WI",
        "WY"
      ],

      editedIndex: -1,
      //Template for new production object
      newProd: {
        productionName: "",
        theaterID: 1,
        directorFirstName: "",
        directorLastName: "",
        street: "",
        city: "",
        stateProvince: "",
        zipcode: "",
        country: "United States",
        ProductionID: 0
      },
      //Template for edited production object
      editedProduction: {
        productionName: "",
        theaterID: 1,
        directorFirstName: "",
        directorLastName: "",
        street: "",
        city: "",
        stateProvince: "",
        zipcode: "",
        country: "United States",
        ProductionID: 0
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
        country: "United States",
        ProductionID: 0
      },
      //Setting headers/columns for the table of productions
      headers: [
        {
          text: "Production ID",
          align: "center",
          value: "ProductionID"
        },
        {
          text: "Production Name",
          align: "center",
          sortable: false,
          value: "ProductionName"
        },
        {
          text: "Theater ID",
          align: "center",
          value: "TheaterID"
        },
        {
          text: "Director",
          align: "center",
          sortable: false
        },
        {
          text: "Address",
          align: "center",
          sortable: false
        },
        {
          text: "Edit",
          align: "center",
          sortable: false
        },
        {
          text: "Delete",
          align: "center",
          sortable: false
        },
        {
          text: "Upload Program",
          align: "center",
          sortable: false
        }
      ]
    };
  },
  //Determines to either create a production or edit existing one
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "New Production" : "Edit Production";
    }
  },
  watch: {
    dialog(val) {
      val || this.close();
    }
  },
  //Gets list of theater's productions upon loading
  async mounted() {
    var today = new Date();
    this.getProductions(today);
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
    async getProductions(today) {
      await axios
        .get("https://api.broadwaybuilder.xyz/production/getProductions", {
          params: {
            previousDate: today,
            theaterID: 1,
            pageSize: 1000
          }
        })
        .then(response => (this.productions = response.data));
    },
    async createProduction(createdProduction) {
      await axios
        .post(
          "https://api.broadwaybuilder.xyz/production/create",
          createdProduction
        )
        .then(response => {
          this.newProd = response.data;
        })
        .catch(function() {
          console.log("Failure.");
        });
    },
    async addDateTime(newProdID) {
      await axios
        .post(
          "https://api.broadwaybuilder.xyz/production/" + newProdID + "/create",
          {
            ProductionID: newProdID,
            Date: this.date,
            Time: this.openTime
          }
        )
        .then(console.log("Successfully created production!!!"))
        .catch(function() {
          console.log("Failure!");
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
    async confirm() {
      if (this.editedIndex > -1) {
        Object.assign(
          this.productions[this.editedIndex],
          this.editedProduction
        );
      } else {
        await this.createProduction(this.editedProduction);

        this.editedProduction.ProductionID = this.newProd.ProductionID;

        await this.addDateTime(this.editedProduction.ProductionID);
      }

      this.close();
    },
    editProduction(item) {
      this.editedIndex = this.productions.indexOf(item);
      this.editedProduction = Object.assign({}, item);
      this.dialog = true;
    }
  }
};
</script>

<style lang="sass" scoped>

img
 width: 2em
 height: 2em

</style>


