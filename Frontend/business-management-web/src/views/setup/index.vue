<template>
  <dynamic-tab :tabItems="setupTabs" @changeTab="changeTab">
    <table-setup v-if="currentTab == 'table'"></table-setup>
    <dish-setup v-else-if="currentTab == 'dish'"></dish-setup>
  </dynamic-tab>
</template> 
  
<script>
import DynamicTab from "../../components/layout/main/DynamicTab.vue";
import { reactive, ref, getCurrentInstance } from "vue";
import Enumeration from "../../js/common/Enumeration.js";
import Resource from "../../js/common/Resource.js";
import CommonFn from "../../js/common/CommonFn.js";
import Table from "../../components/table/Table.vue";
import TableSetup from "./table/TableSetup.vue";
import DishSetup from "./dish/DishSetup.vue";

export default {
  components: { DynamicTab, DishSetup, TableSetup },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const setupTabs = reactive([
      {
        order: 1,
        name: "Bàn",
        tabKey: "table",
      },
      {
        order: 2,
        name: "Món ăn",
        tabKey: "dish",
      },
    ]);

    const currentTab = ref('table');

    /**
     * thay đổi tab
     * @param {*} tab
     */
    function changeTab(tab) {
      proxy.currentTab = tab.tabKey;
    }

    return {
      Enumeration,
      CommonFn,
      Resource,
      setupTabs,
      changeTab,
      currentTab,
    };
  },
};
</script>
  
<style lang="scss">
</style>