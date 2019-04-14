<template>
  <div>
    <div class="columns" v-for="(job, index) in filteredValues" v-bind:key="index">
      <!-- This coloumn just displays a brief description for the job posting -->
      <div class="column is-6">
        <div class="card">
          <header class="card-header">
            <p class="card-header-title">
              <strong id="Title">{{ job.Title }}</strong>
            </p>
          </header>
          <div class="card-content">
            <div class="content">
              <p id="Position">
                <strong>Job Type: &nbsp;</strong>
                <u>{{ job.JobType }}</u>
                <br>
                <strong>Position: &nbsp;</strong>
                <u>{{ job.Position }}</u>
                <br>
              </p>
              <strong>Description</strong>
              <p id="Description">{{ job.Description }}</p>
              <em id="DatePosted">
                Posted
                <strong>{{ calculateDateDifference(job.DateCreated) }}</strong> day(s) ago
              </em>
            </div>
            <div class="content"></div>
          </div>
          <footer class="card-footer">
            <a class="card-footer-item" v-on:click="job.show = !job.show">View</a>
          </footer>
        </div>
      </div>

      <!-- This column shows additional information about the job posting -->
      <div class="column is-6">
        <div class="card" v-if="job.show">
          <header class="card-header">
            <p class="card-header-title">
              <input class="input" type="text" v-model="job.Title" v-if="job.edit">
              <strong id="Title" v-else>{{ job.Title }}</strong>
            </p>
            <a
              v-on:click="job.show = false"
              v-if="!job.edit"
              class="card-header-icon"
              aria-label="more options"
            >
              <span class="icon">
                <FontAwesomeIcon icon="times"/>
              </span>
            </a>
          </header>
          <div class="card-content">
            <div class="content">
              <strong>Description</strong>
              <textarea class="textarea" v-model="job.Description" v-if="job.edit"></textarea>
              <p id="Description" v-else>{{ job.Description }}</p>
            </div>
            <div class="content">
              <strong>Hours</strong>
              <textarea class="textarea" v-model="job.Hours" v-if="job.edit"></textarea>
              <p id="Hours" v-else>{{ job.Hours }}</p>
            </div>
            <div class="content">
              <strong>Requirements</strong>
              <textarea class="textarea" v-model="job.Requirements" v-if="job.edit"></textarea>
              <p id="Requirements" v-else>{{ job.Requirements }}</p>
            </div>
          </div>

          <footer class="card-footer" v-if="hasPermission">
            <a class="card-footer-item" v-if="!job.edit" v-on:click="editJobPosting(job)">Edit</a>
            <a
              class="card-footer-item"
              v-if="job.edit"
              v-on:click="finishEditing(job)"
            >Finish Editing</a>
            <a
              class="card-footer-item"
              v-if="!job.edit"
              v-on:click="removeJobPosting(job, index)"
            >Delete</a>
          </footer>
          <footer class="card-footer" v-else>
            <a class="card-footer-item">Accept Job</a>
          </footer>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: ["jobPostings", "hasPermission", "filters"],
  data() {
    return {
      categories: ["Description", "Hours", "Requirements"],
      jobs: this.jobPostings,
      jobFilters: this.filters
    };
  },
  computed: {
    filteredValues() {
      if (!this.filters.length) return this.jobPostings;

      return this.jobPostings.filter(
        job =>
          this.filters.includes(job.Position) ||
          this.filters.includes(job.JobType)
      );
    }
  },
  methods: {
    editJobPosting(job) {
      job.edit = true;
    },
    finishEditing(job) {
      axios
        .put("https://api.broadwaybuilder.xyz/helpwanted/edittheaterjob", {
          HelpWantedId: job.HelpWantedId,
          TheaterId: job.TheaterId,
          DateCreated: job.DateCreated,
          Position: job.Position,
          Title: job.Title,
          Description: job.Description,
          Hours: job.Hours,
          Requirements: job.Requirements,
          JobType: job.JobType
        })
        .then(response => console.log("Job Updated!", response));

      job.edit = false;
    },
    async removeJobPosting(job, index) {
      // Removes a job posting from the database
      await axios
        .delete(
          "https://api.broadwaybuilder.xyz/helpwanted/deletetheaterjob/" +
            job.HelpWantedId
        )
        .then(
          this.jobPostings.splice(index, 1),
          this.$emit("removed", this.jobPostings),
          (job.show = false)
        );
    },
    showDetails(job) {
      job.show = true;
    },
    calculateDateDifference(datePosted) {
      var dateCreated = new Date(Date.parse(datePosted));
      var dateToday = new Date();
      var dateDifference = Math.floor(
        (dateToday.getTime() - dateCreated.getTime()) / (1000 * 60 * 60 * 24)
      );

      if (dateDifference === -1) dateDifference = 0;

      return dateDifference;
    }
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.card    
  margin: 1.25em 0 1.25em 0
  box-shadow: 0 14px 75px rgba(0,0,0,0.19), 0 10px 10px rgba(0,0,0,0.22)
  transition: all 0.5s ease 0s;

.card-header-icon
  color: #6F0000

.card-footer-item
  transition: all 0.2s ease 0s;

.card-footer-item:hover
  transform: translateY(-5px);
  font-weight: bold
  color: #6F0000

a 
  color: #6F0000

#Title
  font-size: 20px
  text-decoration: underline
  

</style>


