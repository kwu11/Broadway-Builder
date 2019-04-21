<template>
  <div>
    <p>Enter the information for the theater you wish to EDIT</p>
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
        <button v-on:click="editTheater" class="button is-link">Submit</button>
      </div>
      <div class="control">
        <button v-on:click="cancelEditTheater" class="button is-text">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "EditTheaterTryAgain",
  props: {
    ID: null
  },
  data() {
    return {
      theater: {
        TheaterID: this.ID,
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
    async editTheater() {
      await axios
        .put("https://api.broadwaybuilder.xyz/theater/updateTheater",this.theater)
        .then(response => {console.log(response)});
    },
    cancelEditTheater() {
      this.$emit("cancelEditTheater", false);
    }
  }
};
</script>
