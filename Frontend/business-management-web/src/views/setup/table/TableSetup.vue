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
        @refreshData="refreshData"
      />
    </div>

    <TableSetupForm ref="FormDetail" @refreshData="refreshData" />
  </div>
</template> 
    
<script>
import { reactive, ref, getCurrentInstance, nextTick, onMounted } from "vue";
import { tableSetupData } from "./TableSetupData";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Table from "../../../components/table/Table.vue";
import TableSetupForm from "./TableSetupForm.vue";
import TableAPI from "../../../api/views/setup/TableAPI";

export default {
  components: { Table, TableSetupForm },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const { tableGrid } = tableSetupData();

    onMounted(() => {
      proxy.getData();
    });

    async function getData() {
      proxy.$store.commit("SHOW_LOADER", true);

      await TableAPI.getPagingData({
        PageSize: proxy.tableGrid.pageSize,
        CurrentPage: proxy.tableGrid.currentPageNum,
        FilterValue: proxy.tableGrid.filterValue,
        FilterColumn: proxy.tableGrid.filterColumn,
      })
        .then((response) => {
          proxy.tableGrid.gridData = response.data.data;
          proxy.tableGrid.totalRecord = response.data.totalRecord;
          proxy.tableGrid.totalPage = response.data.totalPage;
        })
        .catch(() => {
          proxy.showErrorPopup = true;
          proxy.tableGrid.gridData = [];
          proxy.$store.commit("SHOW_LOADER", false);
        });

      proxy.$store.commit("SHOW_LOADER", false);

      proxy.$refs.Table.resetCurrentSelectedRows();
    }

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
    function showFormDetail(data) {
      if(!data.table_id) {
        data = '';
      }
      
      nextTick(() => {
        proxy.$refs.FormDetail.openForm(data);
      });
    }

    /**
     * refresh dữ liệu
     */
    function refreshData() {
      proxy.getData();
    }

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
      getData,
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