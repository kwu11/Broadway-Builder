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

          <v-btn
            color="info"
            v-on:click="editTheaterInfo"
            @click="close"
          >
            Submit
          </v-btn>

          <v-btn
            color="warning"
            @click="close"
          >
            Cancel
          </v-btn>
        </v-form>
      </v-card-text>  
    </v-card>
  </v-flex>
  <!-- <transition name="modal-fade">
    <div class="modal-backdrop">
      <div
        class="modal"
        role="dialog"
        aria-labelledby="modalTitle"
        aria-describedby="modalDescription"
      >
        <header class="modal-header" id="modalTitle">
          <slot name="header">
            Edit Theater
            <button
              type="button"
              class="btn-close"
              @click="close"
              aria-label="Close modal"
            >x</button>
          </slot>
        </header>
        <section class="modal-body" id="modalDescription">
          <slot name="body">
            <div class="field">
              <label class="label">Theater Name</label>
              <div class="control">
                <input class="input" v-model="theater.TheaterName">
              </div>
            </div>
            <div class="field">
              <label class="label">Company Name</label>
              <div class="control">
                <input class="input" v-model="theater.CompanyName">
              </div>
            </div>
            <div class="field">
              <label class="label">Street Address</label>
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
            <div class="field">
              <label class="label">Phone Number</label>
              <div class="control">
                <input class="input" v-model="theater.PhoneNumber">
              </div>
            </div>
          </slot>
        </section>
        <footer class="modal-footer">
          <slot name="footer">
            <button v-on:click="editTheaterInfo" type="button" class="btn-green" @click="close" aria-label="Close modal">Submit</button>
          </slot>
        </footer>
      </div>
    </div>
  </transition> -->
  </div> 
</template>

<script>
import axios from "axios";

export default {
  name: "TheatersTableModal",
  props: {
      passedTheater: Object
  },
  data() {
    return {
        valid: true,
        theater: {
            TheaterID: this.passedTheater.TheaterID,
            TheaterName: this.passedTheater.TheaterName,
            CompanyName: this.passedTheater.CompanyName,
            StreetAddress: this.passedTheater.StreetAddress,
            City: this.passedTheater.City,
            State: this.passedTheater.State,
            Country: this.passedTheater.Country,
            PhoneNumber: this.passedTheater.PhoneNumber
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
    }
  },
  methods: {
    async editTheaterInfo() {
      await axios
      .put("https://api.broadwaybuilder.xyz/theater/updateTheater",this.theater)
      .then(response => console.log(response));
    },
    close() {
      this.$emit("close");
    }
  }
};
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: rgba(0, 0, 0, 0.3);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  background: #ffffff;
  box-shadow: 2px 2px 20px 1px;
  overflow-x: auto;
  display: flex;
  flex-direction: column;
}

.modal-header,
.modal-footer {
  padding: 15px;
  display: flex;
}

.modal-header {
  border-bottom: 1px solid #eeeeee;
  color: #4aae9b;
  justify-content: space-between;
}

.modal-footer {
  border-top: 1px solid #eeeeee;
  justify-content: flex-end;
}

.modal-body {
  position: relative;
  padding: 20px 10px;
}

.btn-close {
  border: none;
  font-size: 20px;
  padding: 20px;
  cursor: pointer;
  font-weight: bold;
  color: #4aae9b;
  background: transparent;
}

.btn-green {
  color: white;
  background: #4aae9b;
  border: 1px solid #4aae9b;
  border-radius: 2px;
}
.modal-fade-enter,
.modal-fade-leave-active {
  opacity: 0;
}

.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.5s ease;
}
</style>