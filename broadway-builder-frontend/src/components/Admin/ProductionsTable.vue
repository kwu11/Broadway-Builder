<template>
  <div class="ProductionsTable">
    <v-app id="inspire">
      <v-data-table :headers="headers" :items="productions" class="elevation-1">
        <template v-slot:items="props">
          <td>{{props.item.ProductionID}}</td>
          <td>{{props.item.ProductionName}}</td>
          <td>{{props.item.TheaterID}}</td>
          <td>{{props.item.DirectorFirstName}} {{props.item.DirectorLastName}}</td>
          <td>{{props.item.Street}}, {{props.item.City}}, {{props.item.StateProvince}} {{props.item.Zipcode}}</td>
          <td>
            <a v-on:click="showModal(props.item)">
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
    <modal v-show="isModalVisible" v-bind:production="modalProduction" @close="closeModal" />
  </div>
</template>

<script>
import axios from "axios";
import Modal from "@/components/Modal.vue";
export default {
  name: "ProductionsTable",
  components: {
    Modal
  },
  data() {
    return {
      productions: [],
      isModalVisible: false,
      file: "",
      programID: 0,
      currentPage: 1,
      minPage: 1,
      maxPage: 1,
      numberOfItems: 5,
      modalProduction: null,
      prod: [],
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
          text: "Upload",
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

    onFileChange() {
      this.file = this.$refs.file.files[0];
    },
    showModal(production) {
      this.modalProduction = production;
      this.isModalVisible = true;
    },
    closeModal() {
      this.isModalVisible = false;
    },
    programIDSelect(id) {
      this.programID = id;
    }
  }
};
</script>

<style lang="sass" scoped>

img
 width: 2em
 height: 2em


</style>

