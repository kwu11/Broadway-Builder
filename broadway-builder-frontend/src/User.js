import axios from "axios";

export default {
  roles: [],
  isTheaterAdmin: false,
  isSysAdmin: false,
  isGeneralUser: false,
  token: window.localStorage.getItem('token'),
  async getRoles() {

    await axios
      .get("https://api.broadwaybuilder.xyz/user/getrole", {
        headers: {
          'Authorization': `Bearer ${this.token}`
        }
      })
      .then(response => {
        this.roles = response.data;
      });
  },
  async getUserId() {

  },
  async init() {
    await this.getRoles();
    if (this.roles.includes('SysAdmin')) {
      this.isSysAdmin = true;
    } 
    if (this.roles.includes('TheaterAdmin')) {
      this.isTheaterAdmin = true;
    }
    if (this.roles.includes('GeneralUser')) {
      this.isGeneralUser = true;
    }
  }
};