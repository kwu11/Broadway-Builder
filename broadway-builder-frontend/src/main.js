import Vue from "vue";
import VueTelInput from "vue-tel-input";
import App from "./App.vue";
import router from "./router";
import User from './User.js';
import Vuetify from "vuetify";
import "es6-promise/auto";
import "vuetify/dist/vuetify.min.css";
import "vue-tel-input/dist/vue-tel-input.css";
import { library } from "@fortawesome/fontawesome-svg-core";
import { faTimes } from "@fortawesome/free-solid-svg-icons";
import { faCloudUploadAlt } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import vGallery from "v-gallery";
library.add(faTimes);
library.add(faCloudUploadAlt);

Vue.component("FontAwesomeIcon", FontAwesomeIcon);

Vue.use(Vuetify);
Vue.use(VueTelInput);
Vue.use(vGallery);
Vue.config.productionTip = false;


new Vue({
  router,
  async created() {
    await User.init();
  },
  render: h => h(App)
}).$mount("#app");
