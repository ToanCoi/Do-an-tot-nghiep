<template>
  <div class="wrapper" id="app" :class="{ 'nav-small': smallNav }">
    <Navbar ref="Navbar" :customData="navbarData" @makeNavBig="makeNavBig" />
    <Main ref="Main" @makeNavSmall="makeNavSmall" />
    <div class="loader" v-show="showLoader">
      <Loader />
    </div>
    <ToastMessage ref="ToastMessage" :customData="customToast" />
    <!-- Popup thông báo lỗi phía server -->
    <BasePopup class="popup-error" v-show="errorPopupData.showErrorPopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div
            :class="
              errorPopupData.type == Resource.Popup.Warning
                ? 'popup__icon-warning'
                : 'popup__icon-danger'
            "
          ></div>
        </div>
        <div class="popup__text">
          {{ errorPopupData.errorMessage }}
        </div>
      </template>
      <template #footer>
        <div class="footer__btn d-flex flex-center" style="width: 100%">
          <div
            class="btn btn-primary"
            @click="errorPopupData.showErrorPopup = false"
          >
            {{
              errorPopupData.type == Resource.Popup.Warning ? "Đồng ý" : "Đóng"
            }}
          </div>
        </div>
      </template>
    </BasePopup>
  </div>
</template>
  
  <script>
import Navbar from "../components/layout/TheNavbar.vue";
import Main from "../components/layout/TheMain.vue";
import Loader from "../components/Loader.vue";
import ToastMessage from "../components/ToastMessage.vue";
import BasePopup from "../components/BasePopup.vue";
import Resource from "../js/common/Resource";

export default {
  name: "MainLayout",
  components: {
    Navbar,
    Main,
    Loader,
    ToastMessage,
    BasePopup,
  },
  data() {
    return {
      Resource: Resource,
      smallNav: false,
      overlayShow: false,
      errorPopupData: {},
      navbarData: [
        {
          iconClass: "nav__icon-sale",
          itemName: "Bán hàng",
          routerLink: "/sell",
        },
        {
          iconClass: "nav__icon-dashboard",
          itemName: "Nhân viên",
          routerLink: "",
        },
        {
          iconClass: "nav__icon-cash",
          itemName: "Tiền mặt",
          routerLink: "",
        },
        {
          iconClass: "nav__icon-deposit",
          itemName: "Tiền gửi",
          routerLink: "",
        },
        {
          iconClass: "nav__icon-invoice",
          itemName: "Quản lý hóa đơn",
          routerLink: "",
        },
        {
          iconClass: "nav__icon-stock",
          itemName: "Kho",
          routerLink: "",
        },
        {
          iconClass: "nav__icon-assets",
          itemName: "Tài sản",
          routerLink: "",
        },
        {
          iconClass: "nav__icon-tools",
          itemName: "Thiết lập",
          routerLink: "/setup",
        },
        {
          iconClass: "nav__icon-report",
          itemName: "Báo cáo",
          routerLink: "",
        },
        {
          iconClass: "nav__icon-finance",
          itemName: "Phân tích tài chính",
          routerLink: "",
        },
      ],
    };
  },
  computed: {
    customToast() {
      return this.$store.getters.toastData;
    },
    showLoader() {
      return this.$store.getters.showLoader;
    },
    customErrorPopup() {
      return this.$store.getters.errorPopupData;
    },
  },
  watch: {
    customToast: {
      deep: true,
      handler() {
        this.$refs.ToastMessage.showToast();
      },
    },
    customErrorPopup: {
      deep: true,
      handler(val) {
        this.errorPopupData = JSON.parse(JSON.stringify(val));
      },
    },
  },
  methods: {
    /**
     * Hàm thu nhỏ navbar
     */
    makeNavSmall() {
      this.$refs.Navbar.makeNavSmall();
      this.smallNav = true;
    },

    /**
     * Hàm thông báo cho component con mở rộng navbar
     */
    makeNavBig() {
      this.$refs.Main.makeNavBig();
      this.smallNav = false;
    },
  },
};
</script>
  
  <style lang="scss">
@import "/src/assets/css/variable.scss";
@import url("/src/assets/css/common/reset.css");
@import "/src/assets/css/common/global.scss";
@import "/src/assets/css/common/common.scss";
@import "/src/assets/css/common/button.scss";

.nav-small {
  --nav-width: 52px;
}

.wrapper {
  display: flex;
  width: 100%;
}

.loader {
  position: fixed;
  top: 0;
  bottom: 0;
  right: 0;
  left: var(--nav-width);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 999;
  background-color: rgba($color: #fff, $alpha: 0.5);
}

/* popup */
</style>