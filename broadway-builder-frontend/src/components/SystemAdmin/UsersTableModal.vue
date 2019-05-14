<template>
<div>
  <transition name="modal-fade">
    <div class="modal-backdrop">
      <div
        class="modal"
        role="dialog"
        aria-labelledby="modalTitle"
        aria-describedby="modalDescription"
      >
        <header class="modal-header" id="modalTitle">
          <slot name="header">
            Edit User
            <button
              type="button"
              class="btn-close"
              @click="close"
              aria-label="Close modal"
            >x</button>
          </slot>
        </header>
        <section class="modal-body" id="modalDescription">
          <slot name="body">
            <div class="field">
              <label class="label">First Name</label>
              <div class="control">
                <input class="input" v-model="user.FirstName">
              </div>
            </div>
            <div class="field">
              <label class="label">Last Name</label>
              <div class="control">
                <input class="input" v-model="user.LastName">
              </div>
            </div>
            <div class="field">
              <label class="label">Street Address</label>
              <div class="control">
                <input class="input" v-model="user.StreetAddress">
              </div>
            </div>
            <div class="field">
              <label class="label">City</label>
              <div class="control">
                <input class="input" v-model="user.City">
              </div>
            </div>
            <div class="field">
              <label class="label">StateProvince</label>
              <div class="control">
                <input class="input" v-model="user.StateProvince">
              </div>
            </div>
            <div class="field">
              <label class="label">Country</label>
              <div class="control">
                <input class="input" v-model="user.Country">
              </div>
            </div>
          </slot>
        </section>
        <footer class="modal-footer">
          <slot name="footer">
            <button v-on:click="editUserInfo" type="button" class="btn-green" @click="close" aria-label="Close modal">Submit</button>
          </slot>
        </footer>
      </div>
    </div>
  </transition>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "UsersTableModal",
  props: {
      passedUser: Object
  },
  data() {
    return {
        user: {
            UserId: this.passedUser.UserId,
            Username: this.passedUser.Username,
            FirstName: this.passedUser.FirstName,
            LastName: this.passedUser.LastName,
            StreetAddress: this.passedUser.StreetAddress,
            City: this.passedUser.City,
            StateProvince: this.passedUser.StateProvince,
            Country: this.passedUser.Country
        }
    }
  },
  methods: {
    async editUserInfo() {
      await axios
      .put("https://api.broadwaybuilder.xyz/user/updateUser",this.user)
      .then(response => console.log(response));
    },
    close() {
      this.$emit("close");
    }
  }
};
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: rgba(0, 0, 0, 0.3);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  background: #ffffff;
  box-shadow: 2px 2px 20px 1px;
  overflow-x: auto;
  display: flex;
  flex-direction: column;
}

.modal-header,
.modal-footer {
  padding: 15px;
  display: flex;
}

.modal-header {
  border-bottom: 1px solid #eeeeee;
  color: #4aae9b;
  justify-content: space-between;
}

.modal-footer {
  border-top: 1px solid #eeeeee;
  justify-content: flex-end;
}

.modal-body {
  position: relative;
  padding: 20px 10px;
}

.btn-close {
  border: none;
  font-size: 20px;
  padding: 20px;
  cursor: pointer;
  font-weight: bold;
  color: #4aae9b;
  background: transparent;
}

.btn-green {
  color: white;
  background: #4aae9b;
  border: 1px solid #4aae9b;
  border-radius: 2px;
}
.modal-fade-enter,
.modal-fade-leave-active {
  opacity: 0;
}

.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.5s ease;
}
</style>