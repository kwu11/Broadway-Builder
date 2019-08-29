<template>
  <div>
    <br>
  <v-flex xs10 md10 lg4 offset-xs1 offset-sm3 offset-md1 offset-lg4>
    <v-card >
      <v-card-title primary-title>
        <span>
          <h2>Theater Information</h2>
        </span>
      </v-card-title>
      
      <v-card-text>

        <v-form 
          ref="form"
          v-model="valid"
          lazy-validation
        >
          <v-text-field
            v-model="theater.TheaterName"
            :counter="10"
            :rules="TheaterNameRules"
            label="Theater Name"
            required
          ></v-text-field>

          <v-text-field
            v-model="theater.CompanyName"
            :counter="10"
            :rules="CompanyNameRules"
            label="Company Name"
            required
          ></v-text-field>

          <v-text-field
            v-model="theater.StreetAddress"
            :rules="StreetAddressRules"
            label="Street Address"
            required
          ></v-text-field>

          <v-text-field
            v-model="theater.City"
            :rules="CityRules"
            label="City"
            required
          ></v-text-field>

          <v-select
            v-model="theater.State"
            :rules="StateRules"
            :items="states"
            label="State"
            required
          ></v-select>

          <v-text-field
            v-model="theater.Country"
            :rules="CountryRules"
            label="Country"
            required
          ></v-text-field>

          <v-text-field
            v-model="theater.PhoneNumber"
            :rules="PhoneNumberRules"
            label="Phone Number"
            required
          ></v-text-field>

          <v-btn
            color="info"
            v-on:click="createTheater"
          >
            Submit
          </v-btn>
        </v-form>
      </v-card-text>  
    </v-card>
  </v-flex>
    <!-- <p> Enter the information for the theater you wish to CREATE </p>
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
        </div> -->
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "CreateTheater",
  data() {
    return {
      valid: true,
      theater: {
        TheaterName: "",
        CompanyName: "",
        StreetAddress: "",
        City: "",
        State: "",
        Country: "",
        PhoneNumber: ""
      },
      TheaterNameRules: [
          v => !!v || 'First Name is required',
          v => (v && v.length <= 10) || 'Name must be less than 10 characters'
        ],
        CompanyNameRules: [
          v => !!v || 'Last Name is required',
          v => (v && v.length <= 10) || 'Name must be less than 10 characters'
        ],
        StreetAddressRules: [
          v => !!v || 'Street Address is required',
        ],
        CityRules: [
          v => !!v || 'City is required',
        ],
        StateRules: [
          v => !!v || 'State is required',
        ],
        CountryRules: [
          v => !!v || 'Country is required',
        ],
        PhoneNumberRules: [
          v => !!v || 'Phone Number is required',
        ],
        states: [
          "AL",
          "AK",
          "AZ",
          "AR",
          "CA",
          "CO",
          "CT",
          "DE",
          "FL",
          "GA",
          "HI",
          "ID",
          "IL",
          "IN",
          "IA",
          "KS",
          "KY",
          "LA",
          "ME",
          "MD",
          "MA",
          "MI",
          "MN",
          "MS",
          "MO",
          "MT",
          "NE",
          "NV",
          "NH",
          "NJ",
          "NM",
          "NY",
          "NC",
          "ND",
          "OH",
          "OK",
          "OR",
          "PA",
          "RI",
          "SC",
          "SD",
          "TN",
          "TX",
          "UT",
          "VT",
          "VA",
          "WA",
          "WV",
          "WI",
          "WY"
        ]
    };
  },
  methods: {
    async createTheater() {
      var infoIsComplete = true;
      for(var key in this.theater)
      {
        if(this.theater[key] == null || this.theater[key] == "") infoIsComplete = false;
      }
      if (infoIsComplete)
      {
        await axios
          .post("https://api.broadwaybuilder.xyz/theater/createtheater", this.theater)
          .then(response => alert(response.data));
      }
      else
      {
        alert("ALL FIELDS MUST BE FILLED");
      }
    },
    cancelTheaterCreation() {
      this.$emit("cancelCreateTheater", false);
    }      
  }
};
</script>


