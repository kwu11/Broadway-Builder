<template>
  <div>
    <h1>Create A Production</h1>
    <div class="field">
      <label class="label">Theater ID</label>
      <div class="control">
        <input class="input" v-model="production.theaterid">
      </div>
    </div>

    <div class="field">
      <label class="label">Production Name</label>
      <div class="control">
        <input class="input" v-model="production.productionname">
      </div>
    </div>

    <div class="field">
      <label class="label">Director First Name</label>
      <div class="control">
        <input class="input" v-model="production.directorFirstName">
      </div>
    </div>

    <div class="field">
      <label class="label">Director Last Name</label>
      <div class="control">
        <input class="input" v-model="production.directorLastName">
      </div>
    </div>

    <div class="field">
      <label class="label">Street</label>
      <div class="control">
        <input class="input" v-model="production.street">
      </div>
    </div>

    <div class="field">
      <label class="label">City</label>
      <div class="control">
        <input class="input" v-model="production.city">
      </div>
    </div>

    <div class="field">
      <label class="label">State / Province</label>
      <div class="control">
        <input class="input" v-model="production.stateProvince">
      </div>
    </div>

    <div class="field">
      <label class="label">Country</label>
      <div class="control">
        <input class="input" v-model="production.country">
      </div>
    </div>

    <div class="field">
      <label class="label">Zip Code</label>
      <div class="control">
        <input class="input" v-model="production.zipcode">
      </div>
    </div>

    <div class="field is-grouped">
      <div class="control">
        <button v-on:click="createProduction" class="button is-link">Submit</button>
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
  data() {
    return {
      production: {
        theaterId: "",
        productionName: "",
        directorFirstName: "",
        directorLastName: "",
        street: "",
        city: "",
        stateProvince: "",
        country: "",
        zipcode: ""
      }
    };
  },
  methods: {
    async createProduction() {
      await axios
        .post(
          "https://api.broadwaybuilder.xyz/production/create",
          this.production
        )
        .then(alert("Production Successfully Created"));
    }
  }
};
</script>
