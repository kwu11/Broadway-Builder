import axios from "axios";
import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

const state = {
  roles: [],
  isTheaterAdmin: false,
  isSysAdmin: false,
  isGeneralUser: false,
  token: window.localStorage.getItem('token'),
};

const mutations = {
  setIsTheaterAdmin: (state, payload) => {
    state.isTheaterAdmin = payload;
  },
  setIsSysAdmin: (state, payload) => {
    state.isSysAdmin = payload;
  },
  setIsGeneralUser: (state, payload) => {
    state.isGeneralUser = payload;
  },
  setToken: (state, payload) => {
    state.token = payload;
  },
  setRoles: (state, payload) => {
    state.roles = payload;
  }
};

const actions = {
  async updateUserInfo({ commit, state }) {
    let token = window.localStorage.getItem('token');
    if (token === undefined || token === '') {
      token = null;
    }

    commit('setToken', token);

    if (state.token === null) {
      commit('setRoles', []);
      commit('setIsSysAdmin', false);
      commit('setIsTheaterAdmin', false);
      commit('setIsGeneralUser', false);
    } else {
      await axios
        .get("https://api.broadwaybuilder.xyz/user/getrole", {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        })
        .then(response => {
          commit('setRoles', response.data);
        })
        .catch(() => { });
  
      if (state.roles.includes('SysAdmin')) {
        commit('setIsSysAdmin', true);
      } 
      if (state.roles.includes('TheaterAdmin')) {
        commit('setIsTheaterAdmin', true);
      }
      if (state.roles.includes('GeneralUser')) {
        commit('setIsGeneralUser', true);
      }
    }
  }
};

export default new Vuex.Store({
  state,
  actions,
  mutations
})