<template>
  <div class="ProductionsTable">
    Productions
    <div v-if="programID > 0">
      <input type="file" ref="file" id="file" v-on:change="onFileChange()">
      <div class="button is-primary" v-on:click="uploadProgram(programID)">Submit</div>
    </div>
    <table class="table is-hoverable">
      <thead>
        <tr>
          <th>Theater ID</th>
          <th>Production ID</th>
          <th>Production Name</th>
          <th>Director</th>
          <th>Address</th>
          <th>Created</th>
          <th>Edit</th>
          <th>Delete</th>
          <th>Upload New Program</th>
        </tr>
      </thead>
      <tbody v-for="(production, index) in productions" :key="index">
        <tr>
          <td>{{production.TheaterID}}</td>
          <td>{{production.ProductionID}}</td>
          <td>{{production.ProductionName}}</td>
          <td>{{production.DirectorFirstName}} {{production.DirectorLastName}}</td>
          <td>{{production.Street}}, {{production.City}}, {{production.StateProvince}} {{production.Zipcode}}</td>
          <td>{{production.DateTimes[0].Date}}</td>
          <td>
            <a v-on:click="showModal">
              <img src="@/assets/edit.png" alt="Edit">
            </a>
            <modal :production="production" v-show="isModalVisible" @close="closeModal"/>
          </td>

          <td>
            <a v-on:click="deleteProduction(production.ProductionID)">
              <img src="@/assets/tester.png" alt="Delete">
            </a>
          </td>
          <td>
            <a v-on:click="programIDSelect(production.ProductionID)">
              <img src="@/assets/upload.png" alt="Upload">
            </a>
          </td>
          <!-- <td>
            <input
              type="file"
              ref="file"
              id="file"
              v-on:change="onFileChange(), programIDSelect(production.ProductionID)"
            >
            <div
              v-if="programID === production.ProductionID"
              class="button is-primary"
              v-on:click="uploadProgram(production.ProductionID)"
            >Submit</div>
          </td>-->
        </tr>
      </tbody>
    </table>
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
      programID: 0
    };
  },
  async mounted() {
    await axios
      .get(
        "https://api.broadwaybuilder.xyz/production/getProductions?currentDate=3%2F23%2F2019"
      )
      .then(response => (this.productions = response.data));
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
    onFileChange() {
      this.file = this.$refs.file.files[0];
    },
    showModal() {
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
a
 color: black
img
 width: 2em
 height: 2em
.button 
  color:black

</style>

