<!--Basic modal for inputting information to create a new production.-->
<template>
  <div>
    <h1>Create A Production</h1>
    <div class="field is-horizontal">
      <div class="field-label">
        <label class="label">Theater ID</label>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control is-expanded">
            <input class="input" v-model="production.theaterId">
          </p>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-label">
        <label class="label">Production Name</label>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control is-expanded">
            <input class="input" v-model="production.productionName">
          </p>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-label">
        <label class="label">Director</label>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control is-expanded">
            <input class="input" placeholder="First Name" v-model="production.directorFirstName">
          </p>
        </div>
        <div class="field">
          <p class="control is-expanded">
            <input class="input" placeholder="Last Name" v-model="production.directorLastName">
          </p>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-label">
        <label class="label">Address</label>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control is-expanded">
            <input class="input" placeholder="Street" v-model="production.street">
          </p>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-body">
        <div class="field">
          <p class="control is-expanded">
            <input class="input" placeholder="City" v-model="production.city">
          </p>
        </div>
        <div class="field">
          <p class="control is-expanded">
            <input class="input" placeholder="State/Province" v-model="production.stateProvince">
          </p>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-body">
        <div class="field">
          <p class="control is-expanded">
            <input class="input" placeholder="Zipcode" v-model="production.zipcode">
          </p>
        </div>
        <div class="field">
          <p class="control is-expanded">
            <input class="input" placeholder="Country" v-model="production.country">
          </p>
        </div>
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
