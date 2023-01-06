<template>
    <dynamic-tab :tabItems="setupTabs" @changeTab="changeTab">
      <RestaurantSell v-if="currentTab == 'restaurant'"></RestaurantSell>
    </dynamic-tab>
  </template> 
    
  <script>
  import DynamicTab from "../../components/layout/main/DynamicTab.vue";
  import { reactive, ref, getCurrentInstance } from "vue";
  import Enumeration from "../../js/common/Enumeration.js";
  import Resource from "../../js/common/Resource.js";
  import CommonFn from "../../js/common/CommonFn.js";
  import RestaurantSell from "./restaurant/RestaurantSell.vue";
  
  export default {
    components: { DynamicTab, RestaurantSell },
  
    setup: (props) => {
      const { proxy } = getCurrentInstance();
  
      const setupTabs = reactive([
        {
          order: 1,
          name: "Bàn tại quán",
          tabKey: "restaurant",
        },
        {
          order: 2,
          name: "Đơn mang về",
          tabKey: "takeaway",
        },
        {
          order: 3,
          name: "Đơn ship",
          tabKey: "shipping",
        },
      ]);
  
      const currentTab = ref('restaurant');
  
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