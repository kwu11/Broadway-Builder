<template>
<div>
  <v-layout row>
    <v-flex xs12>
      <h1>User Analytics</h1>
    </v-flex>
  </v-layout>

  <v-layout row>
    <v-flex xs3>
      <v-select @change="updateCharts()"
        :items="months"
        label="Month"
        v-model="month"
      ></v-select>
    </v-flex>
    <v-flex xs3>
      <v-select @change="updateCharts()"
        :items="years"
        label="Year"
        v-model="year"
      ></v-select>
    </v-flex>
  </v-layout>

  <v-layout row>

    <v-flex xs6>
      <GChart
        type="ColumnChart"
        :data="successfulLoginData"
        :options="successfulLoginChartOptions"
        @ready="reflowChart"
      />
    </v-flex>

    <v-flex xs6>
      <GChart
        type="ColumnChart"
        :data="sessionLengthData"
        :options="sessionLengthChartOptions"
        @ready="reflowChart"
      />
    </v-flex>

  </v-layout>

    <v-layout row>

    <v-flex xs6>
      <GChart
        type="ColumnChart"
        :data="successfulVsFailedLoginData"
        :options="successfulVsFailedLoginChartOptions"
        @ready="reflowChart"
      />
    </v-flex>

    <!-- <v-flex xs6>
      <GChart
        type="ColumnChart"
        :data="featureUsageData"
        :options="featureUsageChartOptions"
      />
    </v-flex> -->

  </v-layout>
</div>
</template>

<script>
import axios from "axios";
import { GChart } from "vue-google-charts";
export default {
  data() {
    return {
      month: undefined,
      year: undefined,
      months: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
      years: [2018, 2019],
      successfulLoginData: [],
      successfulLoginChartOptions: {
        'title': 'Number of Successful Logins',
        'legend': 'bottom',
        colors:['#ffdb58', '#ffb347', '#ADD8E6']
      },
      sessionLengthData: [],
      sessionLengthChartOptions: {
        'title': 'Session Length',
        'legend': 'bottom',
         colors:['#ffdb58', '#ffb347', '#ADD8E6']
      },
      successfulVsFailedLoginData: [],
      successfulVsFailedLoginChartOptions: {
        'title': 'Successful vs Failed Logins',
        'legend': 'bottom',
        colors: ['#77dd77', '#ff6961']
        
      },
      featureUsageData: [],
      featureUsageChartOptions: {
        'title': 'Feature Usage',
        'legend': 'bottom'
      }
    };
  },
  methods: {
    reflowChart(chart, google) {
      console.log(google);
    },
    formatMonthAndYear(month, year) {
      let monthStr = '';
      switch (month) {
        case 1:
          monthStr = 'January';
          break;
        case 2:
          monthStr = 'Febuary';
          break;
        case 3:
          monthStr = 'March';
          break;
        case 4:
          monthStr = 'April';
          break;
        case 5:
          monthStr = 'May';
          break;
        case 6:
          monthStr = 'June';
          break;
        case 7:
          monthStr = 'July';
          break;
        case 8:
          monthStr = 'August';
          break;
        case 9:
          monthStr = 'September';
          break;
        case 10:
          monthStr = 'October';
          break;
        case 11:
          monthStr = 'November';
          break;
        case 12:
          monthStr = 'December';
          break;
      }

      return `${monthStr} ${year}`;
    },
    getSuccessfulLoginData() {
      axios.get(`https://api.broadwaybuilder.xyz/api/useranalytics/numberofsuccessfullogins/${this.month}/${this.year}`)
        .then(response => {
          this.successfulLoginData = [
            ["Month and Year", "Min", "Avg", "Max"],
            [this.formatMonthAndYear(this.month, this.year), response.data.MinSuccessfulLogins, response.data.AverageSuccessfulLogins, response.data.MaxSuccessfulLogins]
          ];
        });
    },
    getSessionLengthData() {
      axios.get(`https://api.broadwaybuilder.xyz/api/useranalytics/sessionlength/${this.month}/${this.year}`)
        .then(response => {
          this.sessionLengthData = [
            ["Month and Year", "Min", "Avg", "Max"],
            [this.formatMonthAndYear(this.month, this.year), response.data.MinSessionLength, response.data.AverageSessionLength, response.data.MaxSessionLength]
          ];
        });
    },
    getSuccessfulVsFailedLoginData() {
      axios.get(`https://api.broadwaybuilder.xyz/api/useranalytics/successfulvsfailedlogins/${this.month}/${this.year}`)
        .then(response => {
          this.successfulVsFailedLoginData = [
            ["Month and Year", "Successful", "Failed"],
            [this.formatMonthAndYear(this.month, this.year), response.data.SuccessfulLogins, response.data.FailedLogins]
          ];
        });
    },
    // getFeatureUsageData() {
    //   axios.get(`https://api.broadwaybuilder.xyz/api/useranalytics/featureusage/${this.month}/${this.year}`)
    //     .then(response => {
    //       this.featureUsageLoginData = [
    //         ["Month and Year", "Productions", "Theaters", "Help Wanted"],
    //         [this.formatMonthAndYear(this.month, this.year), response.data.Productions, response.data.Theaters, response.data.HelpWanted]
    //       ];
    //     });
    // },
    updateCharts() {
      this.getSuccessfulLoginData();
      this.getSessionLengthData();
      this.getSuccessfulVsFailedLoginData();
      //this.getFeatureUsageData();
    }
  },
  components: {
    GChart
  },
  created() {
    const currentDate = new Date();
    this.month = currentDate.getMonth();
    this.year = currentDate.getFullYear();
    this.updateCharts();
  }
};
</script>
