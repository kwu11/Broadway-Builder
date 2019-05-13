<template>
  <div></div>
</template>





<script>
import axios from "axios";
import User from '@/User.js';
//axios.get()

export default {
  name: "Login",
  methods: {
    getParameterByName: (name, url) => {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }
  },
  async mounted() {
    const token = this.getParameterByName('token', window.location.href);
    window.localStorage.setItem('token', token);

    User.init();

    await axios
      .get("https://api.broadwaybuilder.xyz/user/registrationstatus", {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      .then(response => {
        if (!response.data) {
          this.$router.push({name:'registration'});
        } else {
          this.$router.push({name:'home'});
        }
      });
  }
};
</script>