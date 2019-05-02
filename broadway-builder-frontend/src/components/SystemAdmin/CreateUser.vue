<template>
  <div>
    <p> Enter the information for the USER you wish to CREATE </p>
    <div class="field">
      <div class="control">
        <label class="label">User Name</label>
        <input class="input" v-model="user.Username">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">First Name</label>
        <input class="input" v-model="user.FirstName">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Last Name</label>
        <input class="input" v-model="user.LastName">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Age</label>
        <input class="input" v-model="user.Age">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Birth Year</label>
        <input class="input" v-model="user.Year">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Birth Month</label>
        <input class="input" v-model="user.Month">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Birth Day</label>
        <input class="input" v-model="user.Day">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Street Address</label>
        <input class="input" v-model="user.StreetAddress">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">City</label>
        <input class="input" v-model="user.City">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">StateProvince</label>
        <input class="input" v-model="user.StateProvince">
      </div>
    </div>
    <div class="field">
      <div class="control">
        <label class="label">Country</label>
        <input class="input" v-model="user.Country">
      </div>
    </div>
    <div class="field">
          <div class="control">
            <button v-on:click="createUser" class="button is-link">Submit</button>
          </div>
          <div class="control">
            <button v-on:click="cancelUserCreation" class="button is-text">Cancel</button>
          </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "CreateUser",
  data() {
    return {
      Year: null,
      Month: null,
      Day: null,
      user: {
        Username: "",
        FirstName: "",
        LastName: "",
        Age: null,
        DateOfBirth: null,
        StreetAddress: "",
        City: "",
        StateProvince: "",
        Country: "",
        isEnabled: true
      }
    };
  },
  methods: {
    async createUser() {
        var dummy = new Date(this.Year, this.Month, this.Day);
        // var dateTimeDay = dummy.getDate();
        // var dateTimeMonth = dummy.getMonth();
        // var dateTimeYear = dummy.getFullYear();
        // var dateTimeHours = dummy.getHours();
        // var dateTimeMinutes = dummy.getMinutes();
        // var dateTimeSeconds = dummy.getSeconds();
        // var dateTimeTime = dateTimeDay + "/" + dateTimeMonth + "/" + dateTimeYear + " " + dateTimeHours + ":" + dateTimeMinutes + ":" + dateTimeSeconds;
        // this.user.DateOfBirth = this.DateTime.ParseExact(dateTimeTime, "dd/MM/yyyy HH:mm:ss", this.CultureInfo.InvariantCulture);
        this.user.DateOfBirth = dummy.toJSON();
    
      var infoIsComplete = true;
      for(var key in this.user)
      {
        if(this.user[key] == null || this.user[key] == "") infoIsComplete = false;
      }
      if (infoIsComplete)
      {
        await axios
          .post("http://localhost:64512/user/createuser", this.user)
          .then(response => alert(response.data));
      }
      else
      {
        alert("ALL FIELDS MUST BE FILLED");
      }
    },
    cancelUserCreation() {
      this.$emit("cancelCreateUser", false);
    }   
    
  }
};
</script>


