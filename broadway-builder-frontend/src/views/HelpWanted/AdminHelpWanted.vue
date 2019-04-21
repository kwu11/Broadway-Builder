<template>
  <div class="AdminHelpWanted">
    <h1>
      <strong>Job Opportunities</strong> | Insert Theater Name Here
    </h1>
    <div class="columns">
      <div class="column is-2 is-narrow">
        <!-- These are the buttons to ADD A JOB and to VIEW RESUMES
        VIEW RESUMES is still a work in progress...-->
        <div id="buttons">
          <a class="button is-rounded is-medium" v-on:click="addJobButton">Post A New Job</a>
          <a class="button is-rounded is-medium">View Resumes</a>
        </div>

        <!-- This is the JOB FILTER checkboxes (Still a work in progress) -->
        <JobFilter :jobPostings="jobs" @filtered="filterJobPostings"/>
      </div>
      <div class="column is-10">
        <!-- This is the card that allows an admin to enter information for a new job -->
        <AddJobPostings
          v-if="addJob === true"
          @add="newJobPostingSuccess"
          @cancel="cancelNewJobPosting"
        />

        <!-- This displays all jobs stored in the database as cards on the page -->
        <DisplayJobPostings :jobPostings="jobs" :hasPermission="true" :filters="filters"/>

        <nav class="pagination" is-medium role="navigation" aria-label="pagination">
          <a
            class="pagination-previous"
            v-if="currentPage != minPage"
            v-on:click="currentPage -= 1"
          >Previous</a>
          <a class="pagination-previous" disabled v-else>Previous</a>
          <a
            class="pagination-next"
            v-if="currentPage != maxPage"
            v-on:click="currentPage += 1"
          >Next page</a>
          <a class="pagination-next" disabled v-else>Next page</a>

          <ul class="pagination-list">
            <li v-for="(page, index) in 5" :key="index">
              <a
                v-if="page === currentPage"
                class="pagination-link is-current"
                aria-label="Page 1"
                aria-current="page"
              >{{ index + 1 }}</a>
              <a
                v-else
                v-on:click="changePage(index)"
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
  components: {
    AddJobPostings,
    DisplayJobPostings,
    JobFilter
  },
  data() {
    return {
      // This array stores the jobs obtained from the database.
      jobs: [],
      currentPage: 1,
      minPage: 1,
      maxPage: 20,
      startingPoint: 0,
      numberOfItems: 5,
      // Boolean value to display new job posting inputs
      addJob: false,
      filters: []
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
    changePage(page) {
      this.currentPage = page + 1;
    },
    async getJobPostings() {
      // Obtain all jobs from the database
      await axios
        .get("https://api.broadwaybuilder.xyz/helpwanted/1", {
          params: {
            startingPoint: 0,
            numberOfItems: 3
          }
        })
        .then(
          response => ((this.jobs = response.data), console.log(response.data))
        );

      for (var i = 0; i < this.jobs.length; i++) {
        // Appends a "show" attribute to display more details about the job
        this.$set(this.jobs[i], "show", false);
        // Appends a "edit" attribute to check if a job is being editted
        this.$set(this.jobs[i], "edit", false);
      }
    }
  },
  mounted() {
    // On initial load, get all jobs from the database
    this.getJobPostings();
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.AdminHelpWanted
  margin: 2em 2em

#buttons
  text-align: center

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
