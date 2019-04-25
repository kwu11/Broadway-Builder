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
            <a @click="job.show = false; job.edit = false" v-if="job.show" class="card-header-icon" aria-label="more options">
              <span class="icon">
                <FontAwesomeIcon icon="times" />
              </span>
            </a>
          </header>
          <div class="card-content">
            <div class="content">
              <p id="Position">
                <strong>Job Type: &nbsp;</strong>
                <u>{{ job.JobType }}</u>
                &nbsp;
                <strong>Position: &nbsp;</strong>
                <u>{{ job.Position }}</u>
                <br>
              </p>
            </div>
            <!-- Job Description -->
            <div class="content">
              <strong>Description</strong>
              <textarea class="textarea" v-model="job.Description" v-if="job.edit"></textarea>
              <p v-else-if="job.show">{{ job.Description }}</p>
              <p v-else-if="!job.edit && !job.show">{{ formatLongText(job.Description, maxTextLength, textTail) }}</p>

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
            <a class="card-footer-item" @click="job.show = true">View More Info</a>
            <a class="card-footer-item" v-if="permission" @click="viewResumes = true; helpWantedId = job.HelpWantedId">View Applicants</a>
          </footer>
          <footer class="card-footer" v-if="permission && job.show">
            <a class="card-footer-item" v-if="!job.edit" @click="editJobPosting(job)">Edit</a>
            <a class="card-footer-item" v-if="job.edit" @click="finishEditing(job)">Finish Editing</a>
            <a class="card-footer-item" v-if="!job.edit" @click="deleteConfirmation = true; helpWantedId = job.HelpWantedId; jobIndex = index">Delete</a>
          </footer>
          <footer class="card-footer" v-else-if="!permission && job.show">
            <a class="card-footer-item" @click="applyToJob()">Apply</a>
          </footer>
        </div>
      </div>

      <ResumeModal v-if="viewResumes" :helpWantedId="helpWantedId" @cancel="viewResumes = false" />
      <DeleteJobModal v-if="deleteConfirmation" @cancel="deleteConfirmation = false" />

    </div>
  </div>
</template>

<script>
import axios from "axios";

import ResumeModal from "@/components/HelpWanted/ResumeModal.vue";
import DeleteJobModal from "@/components/HelpWanted/DeleteConfirmationModal.vue";

export default {
  props: ["jobPostings", "hasPermission", "filters", "file"],
  components: {
    ResumeModal,
    DeleteJobModal
  },
  data() {
    return {
      categories: ["Description", "Hours", "Requirements"],
      maxTextLength: 340,
      textTail: "...",
      jobs: this.jobPostings,
      jobFilters: this.filters,
      permission: this.hasPermission,
      deleteConfirmation: false,
      viewResumes: false,
      helpWantedId: 0,
      jobIndex: 0,
      userid: 1
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
        .then(alert("Job Posting Updated!"));

      job.edit = false;
    },
    applyToJob() {},
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

acceptJob:hover
  font-weight: normal
  text-decoration: none
  transition: none


a 
  color: #6F0000

#shortDescription
  margin-bottom: 100px

#Title
  font-size: 20px
  text-decoration: underline
  

</style>


