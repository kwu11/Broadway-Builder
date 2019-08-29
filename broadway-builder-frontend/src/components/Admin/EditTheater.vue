<template>
  <div class="EditTheater">
    <div class="field">
      <label class="label">Theater ID</label>
      <div class="control">
        <input class="input" v-model="theater.TheaterID">
      </div>
    </div>
    <div class="field">
      <label class="label">Company Name</label>
      <div class="control">
        <input class="input" v-model="theater.CompanyName">
      </div>
    </div>

    <div class="field">
      <label class="label">Theater Name</label>
      <div class="control">
        <input class="input" v-model="theater.TheaterName">
      </div>
    </div>

    <div class="field">
      <label class="label">Phone Number</label>
      <div class="control">
        <input class="input" v-model="theater.PhoneNumber">
      </div>
    </div>

    <div class="field">
      <label class="label">Street</label>
      <div class="control">
        <input class="input" v-model="theater.StreetAddress">
      </div>
    </div>

    <div class="field">
      <label class="label">City</label>
      <div class="control">
        <input class="input" v-model="theater.City">
      </div>
    </div>

    <div class="field">
      <label class="label">State</label>
      <div class="control">
        <input class="input" v-model="theater.State">
      </div>
    </div>

    <div class="field">
      <label class="label">Country</label>
      <div class="control">
        <input class="input" v-model="theater.Country">
      </div>
    </div>

    <div class="field is-grouped">
      <div class="control">
        <button v-on:click="editTheaterInfo" class="button is-link">Submit</button>
      </div>
      <div class="control">
        <button class="button is-text">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "EditTheater",
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
    editTheaterInfo() {
      axios
      .put("https://api.broadwaybuilder.xyz/theater/updateTheater",{ data: this.theater })
      .then(response => {console.log(response)});
    }
  }
};
</script>
