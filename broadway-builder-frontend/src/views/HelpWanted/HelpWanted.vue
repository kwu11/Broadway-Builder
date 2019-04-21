<template>
  <div class="AdminHelpWanted">
    <h1>
      <strong>Job Opportunities</strong>
      | {{ theater.TheaterName }}
    </h1>
    <div class="columns">
      <div class="column is-2 is-narrow">
        <!-- These are the buttons to ADD A JOB and to VIEW RESUMES
        VIEW RESUMES is still a work in progress...-->
        <div id="buttons" v-if="hasPermission === true">
          <a class="button is-rounded is-medium" v-on:click="addJobButton">Post A New Job</a>
          <a class="button is-rounded is-medium">View Resumes</a>
        </div>
        <div id="buttons" v-else>
          <!-- Upload resume functionality -->
          <div class="file has-name is-boxed">
            <label class="file-label">
              <input class="file-input" type="file" ref="file" v-on:change="onFileChange()">
              <span class="file-cta">
                <span class="file-label">Upload your resume</span>
              </span>
              <span class="file-name">{{ file.name }}</span>
            </label>
          </div>
        </div>

        <!-- This is the JOB FILTER checkboxes (Still a work in progress) -->
        <JobFilter :jobPostings="jobs" @filtered="filterJobPostings"/>
      </div>
      <div class="column is-10">
        <!-- This is the card that allows an admin to enter information for a new job -->
        <AddJobPostings
          v-if="addJob === true"
          :hasPermission="true"
          @add="newJobPostingSuccess"
          @cancel="cancelNewJobPosting"
        />

        <!-- This displays all jobs stored in the database as cards on the page -->
        <DisplayJobPostings
          v-if="hasPermission"
          :jobPostings="jobs"
          :hasPermission="true"
          :filters="filters"
        />
        <DisplayJobPostings
          v-else
          :jobPostings="jobs"
          :hasPermission="false"
          :filters="filters"
          :file="file"
        />
        <h1 v-if="jobs.length === 0">No job postings available</h1>
        <nav v-else class="pagination" is-medium role="navigation" aria-label="pagination">
          <a
            class="pagination-previous"
            v-if="currentPage != minPage"
            v-on:click="prevPage()"
          >Previous</a>
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
    </div>
  </div>
</template>

<script>
import AddJobPostings from "@/components/HelpWanted/AddJobPostings.vue";
import DisplayJobPostings from "@/components/HelpWanted/DisplayJobPostings.vue";
import JobFilter from "@/components/HelpWanted/JobFilter.vue";
import axios from "axios";

export default {
  name: "AdminHelpWanted",
  props: ["theater", "hasPermission"],
  components: {
    AddJobPostings,
    DisplayJobPostings,
    JobFilter
  },
  data() {
    return {
      // This array stores the jobs obtained from the database.
      jobs: [],
      // Stores the current page the user is on
      currentPage: 1,
      // The minimum number of pages
      minPage: 1,
      // The maximum number of pages (this will change)
      maxPage: 1,
      // Starting point to start getting jobs
      startingPoint: 0,
      // The amount of jobs to retrive
      numberOfItems: 3,
      // Boolean value to display new job posting inputs
      addJob: false,
      // Filter applied to a job
      filters: [],
      file: ""
    };
  },
  methods: {
    // Function to display the new job posting inputs
    addJobButton() {
      this.addJob = true;
    },
    // When a new job is created, updates the current jobs list
    newJobPostingSuccess(newJob) {
      // TODO: Update the method to refresh contents
      this.addJob = false;
      this.jobs.unshift(newJob);
      this.getAllJobPostings();
    },
    // Cancels the creation of a new job
    cancelNewJobPosting(canceled) {
      this.addJob = canceled;
    },
    // Removes a job from the jobs list
    removeJobPosting(updatedJobPostings) {
      this.jobs = updatedJobPostings;
    },
    filterJobPostings(jobFilters) {
      this.filters = jobFilters;
    },
    choosePage(page) {
      this.currentPage = page + 1;
      this.getJobPostings();
    },
    prevPage() {
      this.currentPage -= 1;
      this.getJobPostings();
    },
    nextPage() {
      this.currentPage += 1;
      this.getJobPostings();
    },
    onFileChange() {
      this.file = this.$refs.file.files[0];
    },
    async getJobPostings() {
      // Obtain all jobs from the database within a range
      await axios
        .get(
          "https://api.broadwaybuilder.xyz/helpwanted/" +
            this.theater.TheaterID,
          {
            params: {
              // Algorithm to get the starting index to query from
              startingPoint: this.numberOfItems * (this.currentPage - 1),
              // The number of items starting at the startingPoint
              numberOfItems: this.numberOfItems
            }
          }
        )
        // Set the jobs to the queryed job posting selection
        .then(response => (this.jobs = response.data));

      for (var i = 0; i < this.jobs.length; i++) {
        // Appends a "show" attribute to display more details about the job
        this.$set(this.jobs[i], "show", false);
        // Appends a "edit" attribute to check if a job is being editted
        this.$set(this.jobs[i], "edit", false);
      }
    },
    // Gets the max pages to set the pagination to
    async getMaxPage() {
      await axios
        .get("https://api.broadwaybuilder.xyz/helpwanted/length", {
          params: {
            theaterid: this.theater.TheaterID
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
  },
  mounted() {
    // On initial load, get initial jobs
    this.getJobPostings();
    // Get the numer of total job postings
    this.getMaxPage();
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.AdminHelpWanted
  margin: 2em 2em

#buttons
  text-align: center
  margin-top: 1em

h1 
  margin: 1em
  font-size: 30px

a
  margin-bottom: 0.5em

.columns
  margin: 1em

.card    
  margin: 1.25em 0 1.25em 0
  box-shadow: 0 14px 128px rgba(0,0,0,0.19), 0 10px 10px rgba(0,0,0,0.22)

.button
  box-shadow: 0px 8px 15px rgba(0, 0, 0, 0.1);
  transition: all 0.2s ease 0s;
  align: center

.button:hover
  background-color: #6F0000;
  box-shadow: 0px 15px 20px rgba(0, 0, 0, 0.4);
  color: #fff;
  transform: translateY(-7px);
  font-weight: bold
</style>
