import Vue from "vue";
import Router from "vue-router";
import Home from "./views/Home.vue";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      name: "home",
      component: Home
    },
    {
      path: "/theaters",
      name: "theaters",
      component: () => import("./views/ViewAllTheaters.vue")
    },
    {
      path: "/theater/:TheaterName",
      name: "theater",
      props: true,
      component: () => import("./views/TheaterProfile.vue")
    },
    {
      path: "/adminaccount/{userID}",
      name: "adminaccount",
      component: () => import("./views/AdminAccount.vue")
    },
    {
      path: "/sysadminaccount/{userID}",
      name: "sysadminaccount",
      component: () => import("./views/SysAdminAccount.vue")
    },
    {
      path: "/theater/{theaterid}/helpwanted",
      name: "adminhelpwanted",
      component: () => import("./views/HelpWanted/AdminHelpWanted.vue")
    },
    {
      path: "/theater/{theaterid}/helpwanted/apply",
      name: "userhelpwanted",
      component: () => import("./views/HelpWanted/UserHelpWanted.vue")
    },
    {
      path: "/theater/:TheaterID/userproductioninfo",
      name: "userproductioninfo",
      props: true,
      component: () => import("./views/ProductionInfo/UserProductionInfo.vue")
    },
    {
      path: "/theater/{theaterid}/adminproductioninfo",
      name: "adminproductioninfo",
      component: () => import("./views/ProductionInfo/AdminProductionInfo.vue")
    },
    {
      path: "/theater/{theaterid}/adminproductioninfo/{productionid}",
      name: "adminpicturewheel",
      component: () => import("./views/ProductionInfo/AdminProductionInfo.vue")
    }
  ]
});
