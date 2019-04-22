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
    <nav class="pagination" is-medium role="navigation" aria-label="pagination">
      <a class="pagination-previous" v-if="currentPage != minPage" v-on:click="prevPage()">Previous</a>
      <a class="pagination-previous" disabled v-else>Previous</a>
      <a class="pagination-next" v-if="currentPage != maxPage" v-on:click="nextPage()">Next page</a>
      <a class="pagination-next" disabled v-else>Next page</a>

      <ul class="pagination-list">
        <li v-for="(page, index) in maxPage" :key="index">
          <a
            v-if="page === currentPage"
            class="pagination-link is-current"
            aria-label="Page 1"
            aria-current="page"
          >{{ index + 1 }}</a>
          <a
            v-else
            v-on:click="choosePage(index)"
            class="pagination-link"
            aria-label="Page 1"
            aria-current="page"
          >{{ index + 1 }}</a>
        </li>
      </ul>
    </nav>
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
      startPoint: 0,
      numberOfItems: 10
    };
  },
  async mounted() {
    this.getProductions();
    this.getMaxPage();
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
          "https://api.broadwaybuilder.xyz/production/getProductions?currentDate=3%2F23%2F2019",
          {
            params: {
              pageNum: this.currentPage,
              pagesize: this.numberOfItems
            }
          }
        )
        .then(response => (this.productions = response.data));
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
    },
    choosePage(page) {
      this.currentPage = page + 1;
      this.getProductions();
    },
    prevPage() {
      this.currentPage -= 1;
      this.getProductions();
    },
    nextPage() {
      this.currentPage += 1;
      this.getProductions();
    },
    async getMaxPage() {
      await axios
        .get("https://api.broadwaybuilder.xyz/helpwanted/length", {
          params: {
            theaterid: 1
          }
        })
        .then(response => {
          if (this.numberOfItems === 1) {
            this.maxPage = response.data;
          } else if (this.numberOfItems === response.data) {
            this.maxPage = Math.floor(response.data / this.numberOfItems);
          } else {
            this.maxPage = Math.floor(response.data / this.numberOfItems) + 1;
          }
        });
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

