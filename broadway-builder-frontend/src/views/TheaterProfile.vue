<template>
  <!-- <div class="theaterprofile">
    <div class="hero-body">
      <h1>{{theater.TheaterName}} |</h1>
      {{theater.CompanyName}}
      <div class="container has-text-centered">
        <div class="columns is-vcentered">
          <div class="column is-6 is-half">
            <h1 class="title is-2">Ticket Selling</h1>
            <h2 class="subtitle is-4">Coming soon to a browser near you!</h2>
          </div>

          <div class="column is-5 is-half">
            <figure class="image is-4by3">
              <img src="@/assets/download.png" class="backpic" alt="Custom Pic">
              <figure class="image is-128x128">
                <img src="@/assets/logo.png" class="profpic is-rounded" alt="Profile Pic">
              </figure>
            </figure>
          </div>
        </div>
      </div>
    </div> -->

  <!-- <div class="hero-foot">
    <div class="columns is-mobile is-centered">
      <div class="field is-grouped is-grouped-multiline">
        <div class="control">
          <span v-on:click="goToPictures(theater)" class="button is-danger is-rounded is-medium">Past Productions</span>
        </div>
        <div class="control">
          <span class="button is-danger is-rounded is-medium">Information / Contact Us</span>
        </div>
        <div class="control">
          <span class="button is-danger is-rounded is-medium" v-on:click="goToHelpWanted(theater, permission)">Help Wanted</span>
        </div>
        
        <input type="radio" name="permission" id="one" :value="true" v-model="permission">
        <label for="permission">Admin</label>
        <br>
        <input type="radio" name="permission" id="two" :value="false" v-model="permission">
        <label for="permission">User</label>
        <br>
        <span>Picked: {{ permission }}</span>
      </div>
    </div>
  </div>
  </div> -->
  <v-app id="theater-profile">
    <v-container fluid grid-list-md>
      <v-layout row wrap>
        <v-flex d-flex xs12 sm3 md5>
          <v-card>
            <v-container>
              <v-img src="https://picsum.photos/510/300?random"></v-img>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex d-flex xs12 sm9 md7 class="theater-info">
          <v-container class="theater-info">
            <h1 class="font-weight-medium">{{ theater.TheaterName }}</h1>
            <h2 class="font-weight-light">{{ lorem }}</h2>
            <v-divider></v-divider>
            <div id="theater-actions">
              <v-btn @click="goToPictures(theater)" depressed large>Past Productions</v-btn>
              <v-btn v-if="isTheaterAdmin" @click="goToHelpWanted(theater, true)" depressed large>Job Opportunities Admin </v-btn>
              <v-btn @click="goToHelpWanted(theater, false)" depressed large>Job Opportunities User</v-btn>
            </div>

          </v-container>
        </v-flex>
      </v-layout>
    </v-container>
  </v-app>

</template>

<script>
import User from "@/User.js";

export default {
  name: "TheaterProfile",
  data() {
    return {
      lorem: `Lorem ipsum dolor sit amet, mel at clita quando. Te sit oratio vituperatoribus, nam ad ipsum posidonium mediocritatem, explicari dissentiunt cu mea. Repudiare disputationi vim in, mollis iriure nec cu, alienum argumentum ius ad. Pri eu justo aeque torquatos.`,
      show: false,
      theater: this.$route.params.theater,
      permission: true,
      isTheaterAdmin: User.isTheaterAdmin
    };
  },
  methods: {
    goToPictures(theater) {
      this.$router.push({
        name: "userproductioninfo",
        params: {
          TheaterID: theater.TheaterID
        }
      });
    },
    goToHelpWanted(theater, permission) {
      this.$router.push({
        name: "helpwanted",
        params: {
          theater: theater,
          hasPermission: permission
        }
      });
    }
  }
};
</script>


<style lang="css" scoped>
#theater-profile {
  margin: 2em 8em;
  padding-bottom: 0;
  max-height: 575px;
}

#theater-actions {
  margin-top: 1em;
}

.theater-info {
  height: 100px;
}

.button {
  background-image: linear-gradient(to right, #6f0000, #200122);
  font-family: "Roboto";
  font-weight: bold;
  font-size: 1.5em;
  color: #dedede;
  text-shadow: 1.5px 1.5px black;
}

h1 {
  font-size: 3em;
}

.backpic {
  position: absolute;
  top: 10px;
  left: 10px;
  z-index: 1;
}
.profpic {
  position: absolute;
  top: 25px;
  left: 25px;
  z-index: 2;
}

.card-footer-item {
  color: black;
}

.button {
  box-shadow: 0px 8px 15px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease 0s;
}

.button:hover {
  background-color: #6f0000;
  box-shadow: 0px 15px 20px rgba(0, 0, 0, 0.4);
  color: #fff;
  transform: translateY(-7px);
}
</style>
