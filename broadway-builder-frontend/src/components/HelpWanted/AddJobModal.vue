<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title"><strong>{{ modalTitle }}</strong></p>
        <button class="delete" aria-label="close" @click="$emit('cancel')"></button>
      </header>
      <section class="modal-card-body">

        <!-- Select forms for job type and job positions -->
        <div class="field">
          <div class="field-body">
            <div class="field">
              <label class="label">Job Type</label>
              <div class="control">
                <div class="select">
                  <select v-model="newJob.JobType">
                    <option v-for="(jobType, index) in jobTypes" :key="index">{{ jobType }}</option>
                  </select>
                </div>
              </div>
            </div>

            <div class="field">
              <label class="label">Job Position</label>
              <div class="control">
                <div class="select">
                  <select v-model="newJob.Position">
                    <option v-for="(jobPosition, index) in jobPositions" :key="index">{{ jobPosition }}</option>
                  </select>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="field">
          <label class="label">Subject / Title</label>
          <div class="control">
            <input class="input" type="text" placeholder="Enter a subject or a title" v-model="newJob.Title">
          </div>
        </div>
        <div class="field">
          <label class="label">Description</label>
          <div class="control">
            <textarea class="textarea" placeholder="Enter a description for the job" v-model="newJob.Description"></textarea>
          </div>
        </div>
        <div class="field">
          <label class="label">Schedule / Hours</label>
          <div class="control">
            <textarea class="textarea" placeholder="Enter the preferred schedule for working hours" v-model="newJob.Hours"></textarea>
          </div>
        </div>
        <div class="field">
          <label class="label">Requirements</label>
          <div class="control">
            <textarea class="textarea" placeholder="Enter the requirements and experience needed for the job" v-model="newJob.Requirements"></textarea>
          </div>
        </div>

      </section>

      <footer class="modal-card-foot">
        <button class="button is-success" @click="submitJob()">Add</button>
        <button class="button" @click="$emit('cancel')">Cancel</button>
      </footer>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: ["theaterId"],
  data() {
    return {
      modalTitle: "Post A New Job",
      newJob: {
        Title: "",
        Description: "",
        Hours: "",
        Requirements: "",
        Position: "",
        JobType: "",
        TheaterID: this.theaterId
      },
      jobTypes: ["Full Time", "Part Time", "Seasonal"],
      jobPositions: [
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
      ]
    };
  },
  methods: {
    async submitJob() {
      // Sends a new job posting to the database
      await axios
        .post(
          "https://api.broadwaybuilder.xyz/helpwanted/createtheaterjob",
          this.newJob
        )
        .then(response => alert(response.data))
        .catch(err => alert(err));
    }
  }
};
</script>

<style lang="sass" scoped>
@import '../../../node_modules/bulma/bulma.sass'

.modal-background
  background-color: rgba(0, 0, 0, 0.3);
  
</style>