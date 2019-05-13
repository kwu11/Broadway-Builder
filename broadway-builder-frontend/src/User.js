import axios from "axios";

export default {
  roles: [],
  isTheaterAdmin: false,
  isSysAdmin: false,
  isGeneralUser: false,
  token: window.localStorage.getItem('token'),
  async getRoles() {

    await axios
      .get("https://api.broadwaybuilder.xyz/user/user/getuserrole", {
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

    if (this.roles.contains('SysAdmin')) {
      this.isSysAdmin = true;
    } else if (this.roles.contains('TheaterAdmin')) {
      this.isTheaterAdmin = true;
    } else if (this.roles.contains('GeneralUser')) {
      this.isGeneralUser = true;
    }
  }
};