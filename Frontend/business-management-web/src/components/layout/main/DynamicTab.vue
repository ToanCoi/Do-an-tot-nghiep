<template>
  <div class="content">
    <div class="tab__panel">
      <div class="tab__header d-flex">
        <div
          v-for="(tab, index) in tabItems"
          :key="index"
          class="item-tabs"
          :class="{ active: selectedTab == tab.order }"
          @click="selectTab(tab)"
        >
          {{ tab.name }}
        </div>
      </div>
      <div class="tab__content">
        <slot></slot>
      </div>
    </div>
  </div>
</template> 

<script>
import { reactive, ref, getCurrentInstance, defineProps } from "vue";
import { useRoute, useRouter } from "vue-router";

export default {
  components: {},
  props: {
    tabItems: {
      type: Array,
    },
  },

  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const selectedTab = ref(1);
    const route = useRoute();

    /**
     * select tab
     * @param {*} tab
     */
    function selectTab(tab) {
      proxy.selectedTab = tab.order;

      let url = route.matched[0].path.replace(":key?", tab.tabKey);

      history.pushState({ prv: location.href }, "", url);

      proxy.$emit("changeTab", tab);
    }

    return {
      selectedTab,
      selectTab,
    };
  },
};
</script>
    
<style lang="scss">
@import "/src/assets/css/variable.scss";

.content {
  height: calc(100% - var(--header-height));
  background-color: $light-grey;

  .tab__header {
    padding-left: 4px;
    border-bottom: 1px solid #c1c4cc;
    padding-top: 16px;
  }
}
</style>