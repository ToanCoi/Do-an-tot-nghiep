<template>
  <div class="restaurant__sell">
    <div
      v-for="(floor, floorIndex) in allTable"
      :key="floorIndex"
      class="sell-floor ml-2 mt-3"
    >
      <div class="text-bold">Tầng {{ floor[0].floor }}</div>
      <div class="floor-table">
        <div
          v-for="(table, tableIndex) in floor"
          :key="tableIndex"
          class="table-info mr-4 mt-2"
          :class="{
            active: table.table_status == Enumeration.TableStatus.InProgress,
          }"
          @click="openOrderForm(table)"
        >
          <div class="table-name">{{ table.table_name }}</div>
          <div class="table-status pt-1">
            <div
              class="circle-status mr-1"
              :class="{
                active:
                  table.table_order_status ==
                  Enumeration.TableOrderStatus.InProgress,
                broken: table.table_status == Enumeration.TableStatus.Broken,
              }"
            ></div>
            <div class="status-text">
              {{
                getTextTableStatus(table.table_status, table.table_order_status)
              }}
            </div>
          </div>
          <div
            class="maxsize-text pt-1"
            v-show="table.table_status != Enumeration.TableStatus.Broken"
          >
            {{ table.max_size }} ghế
          </div>
        </div>
      </div>
    </div>
  </div>

  <restaurant-order-form ref="OrderForm" @refreshData="refreshData"></restaurant-order-form>
</template> 

<script>
import { reactive, ref, getCurrentInstance, onMounted } from "vue";
import { restaurantSellData } from "./RestaurantSell";
import { DxTabPanel } from "devextreme-vue/tab-panel";
import Enumeration from "../../../js/common/Enumeration.js";
import CommonFn from "../../../js/common/CommonFn.js";
import TableAPI from "../../../api/views/setup/TableAPI";
import RestaurantOrderForm from "./RestaurantOrderForm.vue";
import OrderAPI from "../../../api/views/sell/OrderAPI";

export default {
  components: {
    DxTabPanel,
    RestaurantOrderForm,
  },

  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const { menuTabItems } = restaurantSellData();

    const allTable = reactive([]);

    onMounted(() => {
      proxy.getTableData();
    });

    /**
     * Lấy dữ liệu bàn
     */
    function getTableData() {
      proxy.$store.commit("SHOW_LOADER", true);
      OrderAPI.getTables()
        .then((res) => {
          if (res.data && res.data.length > 0) {
            // Chia các bàn theo tầng
            let groupFloor = Object.values(
              res.data.reduce((acc, cur) => {
                acc[cur.floor] ??= [];
                acc[cur.floor].push(cur);
                return acc;
              }, {})
            );

            Object.assign(allTable, groupFloor);
          }
        })
        .catch(() => {
          proxy.$store.commit("SHOW_LOADER", false);
        });

      proxy.$store.commit("SHOW_LOADER", false);
    }

    /**
     * Mở form order
     */
    function openOrderForm(table) {
      proxy.$refs.OrderForm.openForm(table);
    }

    /**
     * Lấy tên hiển thị trạng thái bàn
     */
    function getTextTableStatus(tableStatus, tableOrderStatus) {
      let str = "";

      if (tableStatus == Enumeration.TableStatus.Broken) {
        str = CommonFn.getEnumValue(tableStatus, "TableStatus");
      } else {
        str = CommonFn.getEnumValue(tableOrderStatus, "TableOrderStatus");
      }

      return str;
    }

    function refreshData() {
      getTableData();
    }

    return {
      menuTabItems,
      allTable,
      Enumeration,
      CommonFn,
      getTableData,
      openOrderForm,
      getTextTableStatus,
      refreshData
    };
  },
};
</script>

<style lang="scss">
@import "./RestaurantSell.scss";
</style>