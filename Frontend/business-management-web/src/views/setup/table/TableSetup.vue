<template>
  <div class="table__setup">
    <div class="table__header">
      <div class="text-bold">Thiết lập bàn</div>
      <div class="title__function">
        <div
          class="btn btn-circle btn-primary"
          @click="showEmployeeForm = false"
        >
          Thêm mới
        </div>
      </div>
    </div>

    <div class="table__content">
      <Table
        ref="Table"
        :customData="tableGrid"
        @dbClickRow="openForm"
        @clickDefaultFunction="openForm"
        @clickFunctionItem="clickFunctionItem"
        @clickPageNum="changePageNum"
        @changePageSize="changePageSize"
        @changeListSelectedRow="changeListSelectedRow"
      />
    </div>
  </div>
</template> 
    
<script>
import { reactive, ref, getCurrentInstance } from "vue";
import { tableSetupData } from "./TableSetupData";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Table from "../../../components/table/Table.vue";

export default {
  components: { Table },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const { tableGrid } = tableSetupData();

    /**
     * Hàm filter dữ liệu
     */
    async function getDataFilter(filterValue) {
      this.employeeTable.filterValue = filterValue;
      this.employeeTable.currentPageNum = 1;

      this.getDataServer();
    }

    return {
      Enumeration,
      CommonFn,
      Resource,
      tableGrid,
      getDataFilter,
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