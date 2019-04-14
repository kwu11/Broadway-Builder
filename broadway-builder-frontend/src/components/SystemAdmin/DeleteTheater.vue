<template>
  <div>
    <p>Enter the information for the theater you wish to DELETE</p>
    <div class="field">
      <div class="control">
        <label class="label">Theater ID</label>
        <input class="input" v-model="theater.TheaterID">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Theater Name</label>
        <input class="input" v-model="theater.TheaterName">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Company Name</label>
        <input class="input" v-model="theater.CompanyName">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Street Address</label>
        <input class="input" v-model="theater.StreetAddress">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">City</label>
        <input class="input" v-model="theater.City">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">State</label>
        <input class="input" v-model="theater.State">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Country</label>
        <input class="input" v-model="theater.Country">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Phone Number</label>
        <input class="input" v-model="theater.PhoneNumber">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <button v-on:click="deleteTheater" class="button is-link">Submit</button>
      </div>
      <div class="control">
        <button v-on:click="cancelDeleteTheater" class="button is-text">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "DeleteTheater",
  data() {
    return {
      theater: {
        TheaterID: null,
        TheaterName: "",
        CompanyName: "",
        StreetAddress: "",
        City: "",
        State: "",
        Country: "",
        PhoneNumber: ""
      }
    };
  },
  methods: {
    async deleteTheater() {
      await axios
        .delete("https://api.broadwaybuilder.xyz/theater/deleteTheater",{data: this.theater})
        .then(response => {console.log(response)});
    },
    cancelDeleteTheater() {
      this.$emit("cancelDeleteTheater", false);
    }
  }
};
</script>
