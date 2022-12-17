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
        :customData="dishGrid"
        @dbClickRow="showFormDetail"
        @clickDefaultFunction="showFormDetail"
        @clickFunctionItem="clickFunctionItem"
        @clickPageNum="changePageNum"
        @changePageSize="changePageSize"
        @changeListSelectedRow="changeListSelectedRow"
        @refreshData="refreshData"
      />
    </div>

    <DishSetupForm ref="FormDetail" @refreshData="refreshData" />
  </div>
</template> 
    
<script>
import { reactive, ref, getCurrentInstance, nextTick, onMounted } from "vue";
import { dishSetupData } from "./DishSetupData";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Table from "../../../components/table/Table.vue";
import DishSetupForm from "./DishSetupForm.vue";
import DishAPI from "../../../api/views/setup/DishAPI";

export default {
  components: { Table, DishSetupForm },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const { dishGrid } = dishSetupData();

    onMounted(() => {
      proxy.getData();
    });

    async function getData() {
      proxy.$store.commit("SHOW_LOADER", true);

      await DishAPI.getPagingData({
        PageSize: proxy.dishGrid.pageSize,
        CurrentPage: proxy.dishGrid.currentPageNum,
        FilterValue: proxy.dishGrid.filterValue,
        FilterColumn: proxy.dishGrid.filterColumn,
      })
        .then((response) => {
          proxy.dishGrid.gridData = response.data.data;
          proxy.dishGrid.totalRecord = response.data.totalRecord;
          proxy.dishGrid.totalPage = response.data.totalPage;
        })
        .catch(() => {
          proxy.showErrorPopup = true;
          proxy.dishGrid.gridData = [];
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
      if(!data.dish_id) {
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

    /**
     * Hàm thay đổi page num
     */
     function changePageNum(pageNum) {
      dishGrid.currentPageNum = pageNum;
      proxy.getData();
    }

    /**
     * Hàm thay đổi page size
     */
     function changePageSize(pageSize) {
      dishGrid.pageSize = pageSize;

      //Reset page nubmer
      dishGrid.currentPageNum = 1;

      //Lấy lại dữ liệu
      proxy.getData();
    }

    /**
     * Hàm thay đổi list bản ghi được chọn
     */
    function changeListSelectedRow(list) {
      dishGrid.currentSelectedRows = JSON.parse(JSON.stringify(list));
    }


    return {
      Enumeration,
      CommonFn,
      Resource,
      dishGrid,
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

  @import "./DishSetup.scss";
}
</style>