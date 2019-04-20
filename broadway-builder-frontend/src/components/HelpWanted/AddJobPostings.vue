<template>
  <div>
    <div class="card">
      <header class="card-header">
        <p class="card-header-title">
          <input class="input" v-model="job.Title" placeholder="Title / Subject" required>
        </p>
      </header>
      <div class="card-content">
        <div class="columns">
          <div class="column is-6 is-narrow">
            <!-- Show inputs for the adding a new job posting -->
            <div class="content" v-for="(category, index) in categories" v-bind:key="index">
              <strong>{{ category }}</strong>
              <div class="control">
                <!-- Description input -->
                <div v-if="index === 0">
                  <textarea v-model="job.Description" class="textarea" required></textarea>
                </div>

                <!-- Hours input -->
                <div v-else-if="index === 1">
                  <textarea v-model="job.Hours" class="textarea" required></textarea>
                </div>

                <!-- Requirements input -->
                <div v-else>
                  <textarea v-model="job.Requirements" class="textarea" required></textarea>
                </div>
              </div>
            </div>
          </div>
          <div class="column is-3 is-narrow">
            <strong>Type of Job (This cannot be changed!)</strong>
            <br>
            <div class="filters" v-for="(type, index) in jobTypes" :key="index">
              <input type="radio" :value="type" name="jobType" v-model="job.JobType" required>
              <label>{{ type }}</label>
              <br>
            </div>
          </div>
          <div class="column is-3 is-narrow">
            <strong>Role (This cannot be changed!)</strong>

            <div class="filters" v-for="(position, index) in positions" :key="index">
              <input
                type="radio"
                id="jobPosition"
                :value="position"
                name="jobPosition"
                v-model="job.Position"
                required
              >
              <label>{{ position }}</label>
            </div>
          </div>
        </div>
      </div>

      <!-- Footer to ADD a job or CANCEL -->
      <footer class="card-footer">
        <a v-on:click="addNewJobPosting" class="card-footer-item">Add</a>
        <a v-on:click="cancelNewJobPosting" class="card-footer-item">Cancel</a>
      </footer>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      error: "",
      categories: ["Description", "Hours", "Requirements"],
      jobTypes: ["Full Time", "Part Time", "Seasonal"],
      positions: [
        "Actors",
        "Stagehands",
        "Backstage",
        "Stage Manager",
        "Wardrobe Adviser",
        "Scenic Artist",
        "Director",
        "Producer",
        "Usher",
        "Dresser"
      ],
      job: {
        Title: "",
        Description: "",
        Hours: "",
        Requirements: "",
        Position: "",
        theaterid: 1
      },
      jobType: "",
      show: true
    };
  },
  methods: {
    async addNewJobPosting() {
      // Sends a new job posting to the database
      await axios
        .post(
          "https://api.broadwaybuilder.xyz/helpwanted/createtheaterjob",
          this.job
        )
        .then(response => this.$emit("add", this.job))
        .catch(err => {
          let statusCode = err.status;

          switch (statusCode) {
            default:
              alert(
                "An unexpected server error has occured. Please try again momentarily."
              );
              this.cancelNewJobPosting();

              break;
          }
        });
    },
    // Cancel the creation of a new job
    cancelNewJobPosting() {
      this.$emit("cancel", false);
    }
  }
};
</script>


<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.card    
  margin: 1.25em 0 1.25em 0
  box-shadow: 0 14px 28px rgba(0,0,0,0.19), 0 10px 10px rgba(0,0,0,0.22)

a
  color: #6F0000

.filters
  margin: 1em

.addJobPosition
  float: right
  width: 50%

input:invalid 
  border-color: #800000;
  border-width: 3px;

</style>


