<template>
<div>
  <br>
  <v-flex xs10 md10 lg4 offset-xs1 offset-sm3 offset-md1 offset-lg4>
    <v-card >
      <v-card-title primary-title>
        <span>
          <h2>Getting Started</h2>
          Please complete registration.
        </span>
      </v-card-title>
      
      <v-card-text>

        <v-form 
          ref="form"
          v-model="valid"
          lazy-validation
        >
          <v-text-field
            v-model="firstName"
            :counter="10"
            :rules="firstNameRules"
            label="First Name"
            required
          ></v-text-field>

          <v-text-field
            v-model="lastName"
            :counter="10"
            :rules="lastNameRules"
            label="Last Name"
            required
          ></v-text-field>

          <v-text-field
            v-model="streetAddress"
            :rules="streetAddressRules"
            label="Street Address"
            required
          ></v-text-field>

          <v-text-field
            v-model="city"
            :rules="cityRules"
            label="City"
            required
          ></v-text-field>

          <v-select
            v-model="state"
            :rules="stateRules"
            :items="states"
            label="State"
            required
          ></v-select>

          <v-text-field
            v-model="country"
            :rules="countryRules"
            label="Country"
            required
          ></v-text-field>

          <v-btn
            color="success"
            @click="sendUserRegistrationInfo"
          >
            Submit
          </v-btn>

        </v-form>
      </v-card-text>  
    </v-card>
  </v-flex>
  <br>
</div>
</template>

<script>
import axios from 'axios';

export default {
  data: () => ({
    valid: true,
    firstName: '',
    firstNameRules: [
      v => !!v || 'First Name is required',
      v => (v && v.length <= 10) || 'Name must be less than 10 characters'
    ],
    lastName: '',
    lastNameRules: [
      v => !!v || 'Last Name is required',
      v => (v && v.length <= 10) || 'Name must be less than 10 characters'
    ],
    streetAddress: '',
    streetAddressRules: [
      v => !!v || 'Street Address is required',
    ],
    city: '',
    cityRules: [
      v => !!v || 'City is required',
    ],
    state: '',
    stateRules: [
      v => !!v || 'State is required',
    ],
    country: '',
    countryRules: [
      v => !!v || 'Country is required',
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
    ],
  }    
  ),

  methods: {
    async sendUserRegistrationInfo () {
      if (this.$refs.form.validate()) {
        await axios.put('https://api.broadwaybuilder.xyz/user/completeregistration', {
          firstName: this.firstName,
          lastName: this.lastName,
          streetAddress: this.streetAddress,
          city: this.city,
          stateProvince: this.state,
          country: this.country
        }, {
          headers: {
            'Authorization': `Bearer ${window.localStorage.getItem('token')}`
          }
        });

        this.$router.push({name:'home'});
      }
    }
  }
}
</script>