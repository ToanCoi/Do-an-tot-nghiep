<template>
  <div class="nav-wrapper" :class="{ 'nav-small': smallNav }">
    <nav class="nav-bar" nav-type="big" id="nav-bar">
      <div class="nav__header">
        <div class="page-icon" v-show="!smallNav">
          <a href="#" :class="{ 'nav__icon-option': !smallNav }"></a>
        </div>
        <div class="page-icon nav__toggle" v-show="smallNav">
          <div
            :class="{ 'nav__icon-toggle': smallNav }"
            @click="makeNavBig"
          ></div>
        </div>
        <a href="#" class="logo" v-show="!smallNav">
          <img
            src="/src/assets/images/Logo_Module_TiengViet_White.66947422.svg"
            alt=""
          />
        </a>
      </div>
      <div class="nav__list">
        >
        <router-link
          v-for="(item, index) in customData"
          :key="index"
          :customData="item.itemName"
          :to="item.routerLink"
          class="nav__list-item"
          :class="{
            'nav__item-selected': item.routerLink == $route.path,
          }"
        >
          <Tooltip :customData="item.itemName">
            <div class="page-icon">
              <div
                class="item__icon"
                :class="
                  item.routerLink == $route.path
                    ? item.iconClass + '-active'
                    : item.iconClass
                "
              ></div>
            </div>
          </Tooltip>
          <span class="nav__item-name">{{ item.itemName }}</span>
        </router-link>
      </div>
    </nav>
  </div>
</template>

<script>
import Tooltip from "../Tooltip.vue";
export default {
  components: {
    Tooltip,
  },
  props: {
    customData: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      smallNav: false,
      currentHoverMenu: null,
    };
  },
  created() {},
  methods: {
    /**
     * Hàm mở rộng navbar
     */
    makeNavBig() {
      this.smallNav = false;
      this.$emit("makeNavBig");
    },

    /**
     * Hàm thu nhỏ navbar
     */
    makeNavSmall() {
      this.smallNav = true;
    },
  },
};
</script>

<style lang="scss" scoped>
@import url("../../assets/css/layout/navbar.scss");
</style>