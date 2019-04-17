<template>
<body>
  <div class="adminaccount">
    <div class="container">
      <div class="columns">
        <div class="column is-3">
          <aside class="menu">
            <p style="padding-top: 15px" class="menu-label">Administration</p>
            <ul class="menu-list">
              <li>
                <a v-on:click="editTheaterComp">Edit Profile</a>
                <a>Manage Theater Productions</a>
                <ul>
                  <li>
                    <a v-on:click="createProductionForTheater">Create Production</a>
                  </li>
                  <li>
                    <a>Display Productions</a>
                  </li>
                  <li>
                    <a>Edit Production</a>
                  </li>
                  <li>
                    <a>Delete Production</a>
                  </li>
                </ul>
                <a>Manage Users</a>
                <ul>
                  <li>
                    <a>Staff</a>
                  </li>
                  <li>
                    <a>User Collected Data</a>
                  </li>
                  <li>
                    <a>Blacklist</a>
                  </li>
                </ul>
              </li>
            </ul>
            <p class="menu-label">Financial</p>
            <ul class="menu-list">
              <li>
                <a>Ticket Sales</a>
              </li>
            </ul>
            <p class="menu-label">Help Wanted</p>
            <ul class="menu-list">
              <li>
                <router-link to="/theater/{theaterid}/helpwanted">Job Postings</router-link>
              </li>
            </ul>
            <p class="menu-label">Production Info</p>
            <ul class="menu-list">
              <li>
                <router-link to="/theater/{theaterid}/adminproductioninfo">Edit Past Production Info</router-link>
              </li>
            </ul>
          </aside>
        </div>
        <div class="column is-9">
          <section class="hero is-info welcome is-small">
            <div class="hero-body">
              <div class="container">
                <h1 class="title">Hello, Admin.</h1>
                <h2 class="subtitle">I hope you are having a great day!</h2>
              </div>
            </div>
          </section>
          <section class="info-tiles">
            <div class="tile is-ancestor has-text-centered">
              <div class="tile is-parent">
                <article class="tile is-child box">
                  <p class="title">43</p>
                  <p class="subtitle">Theater Members</p>
                </article>
              </div>
              <div class="tile is-parent">
                <article class="tile is-child box">
                  <p class="title">59</p>
                  <p class="subtitle"># of Reviews</p>
                </article>
              </div>
              <div class="tile is-parent">
                <article class="tile is-child box">
                  <p class="title">4.7</p>
                  <p class="subtitle">Avg. Show Rating</p>
                </article>
              </div>
              <div class="tile is-parent">
                <article class="tile is-child box">
                  <p class="title">19</p>
                  <p class="subtitle">Applications</p>
                </article>
              </div>
            </div>
          </section>
          <div class="columns">
            <div class="column is-6">
              <div class="card events-card">
                <header class="card-header">
                  <p class="card-header-title">Ticket Selling</p>
                  <a href="#" class="card-header-icon" aria-label="more options">
                    <span class="icon">
                      <i class="fa fa-angle-down" aria-hidden="true"></i>
                    </span>
                  </a>
                </header>
                <div class="card-table">
                  <div class="content is-fullwidth">
                    <table class="table is-striped is-scrollable">
                      <tbody height="200px">
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                        <tr>
                          <td width="5%">
                            <i class="fa fa-bell-o"></i>
                          </td>
                          <td>Lorum ipsum dolem aire</td>
                          <td>
                            <a class="button is-small is-primary" href="#">Edit</a>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
                <footer class="card-footer">
                  <a href="#" class="card-footer-item">View All</a>
                </footer>
              </div>
            </div>
            <EditTheater v-if="editTheater === true" v-bind:theater="theater"/>
            <CreateProduction v-if="createProduction === true" v-bind:theater="theater"/>
          </div>
        </div>
      </div>
    </div>
  </div>
</body>
</template>

<script>
import EditTheater from "@/components/Admin/EditTheater.vue";
import CreateProduction from "@/components/Admin/CreateProduction.vue";
export default {
  name: "AdminAccount",
  components: {
    EditTheater,
    CreateProduction
  },
  data() {
    return {
      editTheater: false,
      createProduction: false,
      theater: {
        TheaterName: "Theater",
        CompanyName: "Company",
        StreetAddress: "123 Testfield Way",
        City: "Long Beach",
        State: "CA",
        Country: "USA",
        PhoneNumber: "5555555555"
      }
    };
  },
  mounted() {},
  methods: {
    editTheaterComp() {
      this.editTheater = !this.editTheater;
    },
    createProductionForTheater() {
      this.createProduction = !this.createProduction;
    }
  }
};
</script>

<style lang="sass">
.table
  &.is-scrollable
    tbody
      overflow-y: scroll
      width: 100%
      position: absolute
.card
  &.events-card
    background-color: white
    -webkit-box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1)
    box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1)
    color: #4a4a4a

a:hover
  font-weight: bold
</style>
