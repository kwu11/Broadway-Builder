<template>
  <div>
    <div class="columns" v-for="(job, index) in filteredValues" v-bind:key="index">
      <!-- This coloumn just displays a brief description for the job posting -->
      <div class="column is-12">
        <div class="card">
          <header class="card-header">
            <p class="card-header-title">
              <input class="input" type="text" v-model="job.Title" v-if="job.edit">
              <strong id="Title" v-else>{{ job.Title }}</strong>
            </p>
            <a
              v-on:click="job.show = false; job.edit = false"
              v-if="job.show"
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
              <p id="Position">
                <strong>Job Type: &nbsp;</strong>
                <u>{{ job.JobType }}</u>
                <br>
                <strong>Position: &nbsp;</strong>
                <u>{{ job.Position }}</u>
                <br>
              </p>
            </div>
            <!-- Job Description -->
            <div class="content">
              <strong>Description</strong>
              <textarea class="textarea" v-model="job.Description" v-if="job.edit"></textarea>
              <p
                v-else-if="!job.edit && !job.show"
              >{{ formatLongText(job.Description, maxTextLength, textTail) }}</p>
              <p v-if="job.show">{{ job.Description }}</p>
            </div>
            <!-- Job Hours -->
            <div class="content" v-if="job.show">
              <strong>Hours</strong>
              <textarea class="textarea" v-model="job.Hours" v-if="job.edit"></textarea>
              <p id="Hours" v-else>{{ job.Hours }}</p>
            </div>
            <!-- Job Requirements -->
            <div class="content" v-if="job.show">
              <strong>Requirements</strong>
              <textarea class="textarea" v-model="job.Requirements" v-if="job.edit"></textarea>
              <p id="Requirements" v-else>{{ job.Requirements }}</p>
            </div>
            <!-- Job Date Created -->
            <em id="DatePosted">
              Posted
              <strong>{{ calculateDateDifference(job.DateCreated) }}</strong> day(s) ago
            </em>
          </div>
          <!-- Card options for interacting with a job -->
          <footer class="card-footer" v-if="!job.show">
            <a class="card-footer-item" v-on:click="job.show = true">View</a>
          </footer>
          <footer class="card-footer" v-if="permission && job.show">
            <a class="card-footer-item" v-if="!job.edit" v-on:click="editJobPosting(job)">Edit</a>
            <a
              class="card-footer-item"
              v-if="job.edit"
              v-on:click="finishEditing(job)"
            >Finish Editing</a>
            <a
              class="card-footer-item"
              v-if="!job.edit"
              v-on:click="deleteConfirmation = true"
            >Delete</a>
          </footer>
          <footer class="card-footer" v-else-if="!permission && job.show">
            <a class="card-footer-item">Accept Job</a>
          </footer>
          <div class="modal" v-if="deleteConfirmation">
            <div class="modal-background"></div>
            <div class="modal-content">
              <!-- Any other Bulma elements you want -->
            </div>
            <button class="modal-close is-large" aria-label="close"></button>
          </div>
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
      maxTextLength: 340,
      textTail: "...",
      jobs: this.jobPostings,
      jobFilters: this.filters,
      permission: this.hasPermission,
      deleteConfirmation: false
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
    formatLongText(text, length, tail) {
      // Create new div element
      var node = document.createElement("div");
      // Add the text to the newly created div element
      node.innerHTML = text;
      // Get the text content to be potentially modified
      var content = node.textContent;
      // If the content length is too long slice it and append a tail to the text
      // Else, just return the content unmodified
      return content.length > length
        ? content.slice(0, length) + tail
        : content;
    },
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

nav
  background-image: white
  font-family: 'Roboto'

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

#shortDescription
  margin-bottom: 100px

#Title
  font-size: 20px
  text-decoration: underline
  

</style>


