<template>
  <div class="content sell__content">
    <div class="tab__panel">
      <div class="tab__header d-flex">
        <div
          class="item-tabs"
          :class="{ active: selectedTab == 1 }"
          @click="selectTab(1)"
        >
          Bàn tại quán
        </div>
        <div
          class="item-tabs"
          :class="{ active: selectedTab == 2 }"
          @click="selectTab(2)"
        >
          Đơn mang về
        </div>
        <div
          class="item-tabs"
          :class="{ active: selectedTab == 3 }"
          @click="selectTab(3)"
        >
          Đơn ship
        </div>
      </div>
      <div class="tab__content">
        <div
          v-for="(item, index) in tableNumber"
          :key="index"
          class="table-info mr-4"
          :class="{ active: item.Status == Enumeration.TableStatus.InProgress }"
        >
          <div class="table-name">Table {{ item.Number }}</div>
          <div class="table-status">
            <div
              class="circle-active mr-1"
              v-if="item.Status == Enumeration.TableStatus.InProgress"
            ></div>
            <div class="status-text">
              {{ CommonFn.getEnumValue(item.Status, "TableStatus") }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template> 

<script>
import { reactive, ref, getCurrentInstance } from "vue";
import { contentSellData } from "./ContentSellData";
import { DxTabPanel } from "devextreme-vue/tab-panel";
import Enumeration from "../../js/common/Enumeration.js";
import CommonFn from "../../js/common/CommonFn.js";

export default {
  components: {
    DxTabPanel,
  },

  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const { menuTabItems } = contentSellData();
    const selectedTab = ref(1);

    const tableNumber = reactive([
      { Status: Enumeration.TableStatus.New, Number: 1 },
      { Status: Enumeration.TableStatus.InProgress, Number: 2 },
      { Status: Enumeration.TableStatus.InProgress, Number: 3 },
      { Status: Enumeration.TableStatus.New, Number: 4 },
    ]);

    /**
     * select tab
     * @param {*} tab
     */
    function selectTab(tab) {
      proxy.selectedTab = tab;
    }

    return {
      menuTabItems,
      selectedTab,
      selectTab,
      tableNumber,
      Enumeration,
      CommonFn,
    };
  },
};
</script>

<style lang="scss">
@import "./ContentSell.scss";
</style>