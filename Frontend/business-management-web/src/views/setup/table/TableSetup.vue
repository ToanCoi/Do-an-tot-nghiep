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

    <!-- Popup confirm xóa một bản ghi -->
    <BasePopup
      v-show="deleteData.showDeletePopup && !deleteData.dataDeleteConflict"
    >
      <template #content>
        <div class="page-icon popup-icon">
          <div class="popup__icon-warning"></div>
        </div>
        <div class="popup__text">
          Bạn có thực sự muốn xóa Bàn {{ deleteData.deleteString }} không?
        </div>
      </template>
      <template #footer>
        <div
          class="btn btn-default btn-cancel"
          @click="deleteData.showDeletePopup = false"
        >
          Không
        </div>
        <div class="btn btn-primary" @click="deleteSelectedRecord">Có</div>
      </template>
    </BasePopup>
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
import BasePopup from "../../../components/BasePopup.vue";

export default {
  components: { Table, TableSetupForm, BasePopup },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const { tableGrid } = tableSetupData();

    const deleteData = reactive({
      showDeletePopup: false,
      showMultipleDeletePopup: false,
      showErrorPopup: false,
      showNoRecordPopup: false,
      tableDelete: {},
      dataDeleteConflict: false,
      conflictMsg: "",
      deleteString: "",
    });

    onMounted(() => {
      proxy.getData();
    });

    async function getData() {
      proxy.$store.commit("SHOW_LOADER", true);

      await TableAPI.getPagingData({
        PageSize: tableGrid.pageSize,
        CurrentPage: tableGrid.currentPageNum,
        FilterValue: tableGrid.filterValue,
        FilterColumn: tableGrid.filterColumn,
        Sort: tableGrid.sort
      })
        .then((response) => {
          tableGrid.gridData = response.data.data;
          tableGrid.totalRecord = response.data.totalRecord;
          tableGrid.totalPage = response.data.totalPage;
        })
        .catch(() => {
          proxy.showErrorPopup = true;
          tableGrid.gridData = [];
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
      if (!data.table_id) {
        data = "";
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

    function clickFunctionItem(fn, item) {
      switch (fn) {
        case "Xóa":
          deleteData.deleteString = "<" + item.table_name + ">";
          Object.assign(deleteData.tableDelete, item);
          deleteData.showDeletePopup = true;
          break;
      }
    }

    /**
     * Xoá bản ghi đã chọn
     */
    async function deleteSelectedRecord() {
      proxy.$store.commit("SHOW_LOADER", true);

      if (!deleteData.dataDeleteConflict) {
        await TableAPI.delete(deleteData.tableDelete.table_id)
          .then(async (response) => {
            if (response.status != 204) {
              //Tắt popup
              deleteData.showDeletePopup = false;

              //Reload dữ liệu
              await proxy.getData();

              //Hiển thị toast message
              proxy.$store.commit("SHOW_TOAST", {
                toastType: "success",
                toastMessage: Resource.Message.DeleteSuccess,
              });
            }
          })
          .catch(() => {
            deleteData.showErrorPopup = true;
          });
      }

      proxy.$store.commit("SHOW_LOADER", false);
    }

    /**
     * Hàm thay đổi page num
     */
    function changePageNum(pageNum) {
      tableGrid.currentPageNum = pageNum;
      proxy.getData();
    }

    /**
     * Hàm thay đổi page size
     */
    function changePageSize(pageSize) {
      tableGrid.pageSize = pageSize;

      //Reset page nubmer
      tableGrid.currentPageNum = 1;

      //Lấy lại dữ liệu
      proxy.getData();
    }

    /**
     * Hàm thay đổi list bản ghi được chọn
     */
    function changeListSelectedRow(list) {
      tableGrid.currentSelectedRows = JSON.parse(JSON.stringify(list));
    }

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
      deleteData,
      deleteSelectedRecord,
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