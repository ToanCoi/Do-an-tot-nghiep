<template>
  <header class="header-page" id="header-page">
    <div class="header__left">
      <div class="page-icon" @click="makeNavSmall" v-show="bigNav">
        <div class="header__icon-small-nav"></div>
      </div>
      <div class="header__branch">
        <span class="header__branch-name text-uppercase">Nhà hàng NVTOAN</span>
        <div class="page-icon">
          <div class="header__icon-branch-work"></div>
        </div>
      </div>
      <!-- <Dropdown
        :customData="yearDropdown"
        :model="currentSelectedYear"
        @valueChanged="valueChanged"
      /> -->
    </div>
    <div class="header__right">
      <div class="page-icon">
        <div class="header__icon-notify"></div>
      </div>
      <div class="header__user" @click="showManagerAccount = true">
        <div class="header__icon-user"></div>
        <div class="user__name text-semibold">{{ currentUser.user_name }}</div>
        <div class="page-icon">
          <div class="header__icon-user-menu"></div>
        </div>
        <div
          v-show="showManagerAccount"
          @focusout="showManagerAccount = false"
          class="user__manager"
        >
          <div class="user__logout">
            <span class="text-semibold" @click="logout">Đăng xuất</span>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>

<script>
// import Dropdown from "../../components/Dropdown.vue";
export default {
  components: {
    // Dropdown,
  },
  data() {
    return {
      bigNav: true,
      currentSelectedYear: null,
      yearDropdown: {
        displayValues: [],
        keys: [],
        width: "calc(var(--column-width) * 0.8)",
        height: "32px",
      },
      showManagerAccount: false,
      currentUser: this.$store.getters.user
    };
  },
  mounted() {
    this.getYearsForDropdown();
  },
  methods: {
    /**
     * Hàm gọi cha để thu nhỏ navbar
     */
    makeNavSmall() {
      this.$emit("makeNavSmall");
      this.bigNav = false;
    },

    /**
     * Hàm hiện icon khi navbar to
     */
    makeNavBig() {
      this.bigNav = true;
    },

    /**
     * Lấy giá trị năm thêm vào dropdown
     */
    getYearsForDropdown() {
      this.currentSelectedYear = new Date().getFullYear();

      this.yearDropdown.defaultValue = this.currentSelectedYear.toString();

      for (
        let i = this.currentSelectedYear - 5;
        i < this.currentSelectedYear + 3;
        i++
      ) {
        this.yearDropdown.keys.push(i);
      }

      this.yearDropdown.displayValues = this.yearDropdown.keys.map(String);
    },

    /**
     * Hàm xử lý khi chọn năm khác ở dripdown
     */
    valueChanged(value) {
      this.$store.commit("CHANGE_YEAR", value);
    },

    logout() {
      localStorage.clear();

      this.$router.push({ name: "Login" });
    },
  },
};
</script>

<style lang="scss" scoped>
@import url("../../../assets/css/layout/header.scss");
</style>