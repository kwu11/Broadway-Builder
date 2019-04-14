<template>
  <div>
    <p> Enter the information for the theater you wish to CREATE </p>
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
    </div><div class="field">
      <div class="control">
        <label class="label">Street Address</label>
        <input class="input" v-model="theater.StreetAddress">
      </div>
    </div><div class="field">
      <div class="control">
        <label class="label">City</label>
        <input class="input" v-model="theater.City">
      </div>
    </div><div class="field">
      <div class="control">
        <label class="label">State</label>
        <input class="input" v-model="theater.State">
      </div>
    </div><div class="field">
      <div class="control">
        <label class="label">Country</label>
        <input class="input" v-model="theater.Country">
      </div>
    </div><div class="field">
      <div class="control">
        <label class="label">Phone Number</label>
        <vue-tel-input v-model="theater.PhoneNumber"
                        @onInput="onInput"
                  :preferredCountries="['us', 'can']">
   </vue-tel-input>            
      </div>
    </div>
    <div class="field">
          <div class="control">
            <button v-on:click="createTheater" class="button is-link">Submit</button>
          </div>
          <div class="control">
            <button v-on:click="cancelTheaterCreation" class="button is-text">Cancel</button>
          </div>
        </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "CreateTheater",
  data() {
    return {
      theater: {
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
    number: '',
    isValid: '',
    country: '',
    async createTheater() {
      await axios
      .post("https://api.broadwaybuilder.xyz/theater/createtheater", this.theater)
      .then(response => {console.log(response)});
    },
    cancelTheaterCreation() {
      this.$emit("cancelCreateTheater", false);
    },
    onInput({number, isValid, country}) {
      //console.log(number, isValid, country);
    }

    
    
  }
};
</script>


