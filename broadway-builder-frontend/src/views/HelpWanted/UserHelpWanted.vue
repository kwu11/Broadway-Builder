<template>
  <div class="UserHelpWanted">
    <h1>
      <strong>Job Opportunities</strong> | Insert Theater Name Here
    </h1>
    <div class="columns">
      <div class="column is-2 is-narrow">
        <!-- <ResumeUpload/> -->
        <JobFilter :jobPostings="jobs" @filtered="filterJobPostings"/>
      </div>
      <div class="column is-10">
        <DisplayJobPostings :jobPostings="jobs" :hasPermission="false" :filters="filters"/>
      </div>
    </div>
  </div>
</template>

<script>
// import ResumeUpload from "@/components/HelpWanted/ResumeUpload.vue";
import DisplayJobPostings from "@/components/HelpWanted/DisplayJobPostings.vue";
import JobFilter from "@/components/HelpWanted/JobFilter.vue";
import axios from "axios";

export default {
  name: "UserHelpWanted",
  components: {
    // ResumeUpload,
    DisplayJobPostings,
    JobFilter
  },
  props: ["hasPermission"],
  data() {
    return {
      jobs: [],
      filters: []
    };
  },
  methods: {
    filterJobPostings(JobFilters) {
      this.filters = JobFilters;
    },
    async getAllJobPostings() {
      // Obtain all jobs from the database
      await axios
        .get("https://api.broadwaybuilder.xyz/helpwanted/1", {
          params: {
            startingPoint: 0,
            numberOfItems: 3
          }
        })
        .then(response => (this.jobs = response.data));

      for (var i = 0; i < this.jobs.length; i++) {
        // Appends a "show" attribute to display more details about the job
        this.$set(this.jobs[i], "show", false);
        this.$set(this.jobs[i], "edit", false);
      }

      this.jobs.reverse();
    }
  },
  async mounted() {
    // On load, get all jobs from the database
    this.getAllJobPostings();
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

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
  transition: all 0.3s ease 0s;
  align: center

.button:hover
  background-color: #6F0000;
  box-shadow: 0px 15px 20px rgba(0, 0, 0, 0.4);
  color: #fff;
  transform: translateY(-7px);
</style>
