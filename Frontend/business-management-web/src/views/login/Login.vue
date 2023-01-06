<template>
  <div class="container">
    <div class="screen">
      <div class="screen__content">
        <div class="login">
          <div class="text-bold">Hệ thống quản lý nhà hàng</div>
          <div class="login__field">
            <font-awesome-icon icon="fa-solid fa-user" />
            <input
              type="text"
              class="login__input"
              placeholder="User name / Email"
              v-model="accountUser.username"
            />
          </div>
          <div class="login__field">
            <font-awesome-icon icon="fa-solid fa-lock" />
            <input
              type="password"
              class="login__input"
              placeholder="Password"
              v-model="accountUser.password"
            />
          </div>
          <button class="button login__submit" @click="loginApp">
            <span class="button__text">Đăng nhập</span>
            <font-awesome-icon icon="fa-solid fa-chevron-right" />
          </button>
        </div>
      </div>
      <div class="screen__background">
        <span
          class="screen__background__shape screen__background__shape4"
        ></span>
        <span
          class="screen__background__shape screen__background__shape3"
        ></span>
        <span
          class="screen__background__shape screen__background__shape2"
        ></span>
        <span
          class="screen__background__shape screen__background__shape1"
        ></span>
      </div>
    </div>
  </div>
</template>

<script>
import { getCurrentInstance, reactive } from "@vue/runtime-core";
import AccountAPI from "../../api/views/login/AccountAPI";

export default {
  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const accountUser = reactive({
      username: null,
      password: null,
    });

    function loginApp() {
      AccountAPI.login(accountUser.username, accountUser.password)
        .then((res) => {
          if (res.data && res.data.code == 200) {
            proxy.$store.commit("changeToken", res.data.data.token ?? null);
            proxy.$store.commit("changeUser", res.data.data.user ?? null);

            proxy.$router.push({ name: "Sell" });
          }
        })
        .catch((e) => {});
    }

    return {
      loginApp,
      accountUser,
    };
  },
};
</script>

<style scoped>
@import url("./Login.scss");
</style>