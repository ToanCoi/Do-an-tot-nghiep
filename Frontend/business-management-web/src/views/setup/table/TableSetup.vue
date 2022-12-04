<template>
  <div class="table__setup">
    <div class="table__header">
      <div class="text-bold">Thiết lập bàn</div>
      <div class="title__function">
        <div class="btn btn-circle btn-primary" @click="showFormDetail">
          Thêm mới
        </div>
      </div>
    </div>

    <div class="table__content">
      <Table
        ref="Table"
        :customData="tableGrid"
        @dbClickRow="showFormDetail"
        @clickDefaultFunction="showFormDetail"
        @clickFunctionItem="clickFunctionItem"
        @clickPageNum="changePageNum"
        @changePageSize="changePageSize"
        @changeListSelectedRow="changeListSelectedRow"
      />
    </div>

    <TableSetupForm ref="FormDetail" @refreshData="refreshData" />
  </div>
</template> 
    
<script>
import { reactive, ref, getCurrentInstance, nextTick } from "vue";
import { tableSetupData } from "./TableSetupData";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Table from "../../../components/table/Table.vue";
import TableSetupForm from "./TableSetupForm.vue";

export default {
  components: { Table, TableSetupForm },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const { tableGrid } = tableSetupData();

    /**
     * Hàm filter dữ liệu
     */
    async function getDataFilter(filterValue) {
      proxy.employeeTable.filterValue = filterValue;
      proxy.employeeTable.currentPageNum = 1;

      proxy.getDataServer();
    }

    /**
     * mở form detail
     */
    function showFormDetail(event, data) {
      if (data) {
      } else {
        nextTick(() => {
          proxy.$refs.FormDetail.openForm("");
        });
      }
    }

    /**
     * refresh dữ liệu
     */
    function refreshData() {}

    function clickFunctionItem() {}

    function changePageNum() {}

    function changePageSize() {}

    function changeListSelectedRow() {}

    return {
      Enumeration,
      CommonFn,
      Resource,
      tableGrid,
      getDataFilter,
      showFormDetail,
      refreshData,
      clickFunctionItem,
      changePageNum,
      changePageSize,
      changeListSelectedRow,
    };
  },
};
</script>
  
<style lang="scss">
.table__setup {
  width: 100%;

  @import "./TableSetup.scss";
}
</style>