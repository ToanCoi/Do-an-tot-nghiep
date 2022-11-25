<template>
  <div class="content">
    <Table
      ref="Table"
      :customData="employeeTable"
      @dbClickRow="openForm"
      @clickDefaultFunction="openForm"
      @clickFunctionItem="clickFunctionItem"
      @clickPageNum="changePageNum"
      @changePageSize="changePageSize"
      @changeListSelectedRow="changeListSelectedRow"
    />
  </div>
</template>

<script>
import { reactive, ref, getCurrentInstance } from "vue";
import { contentProductData } from "./ContentProductData.js";
import Enumeration from "../../js/common/Enumeration.js";
import Resource from "../../js/common/Resource.js";
import CommonFn from "../../js/common/CommonFn.js";
import Table from "../../components/table/Table.vue";

export default {
  components: { Table },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const employeeTable = reactive({
      width: "100%",
      gridHeight: "calc(100vh - var(--header-height) - 86px - 60px - 16px)",
      tableHeight:
        "calc(100vh - var(--header-height) - 86px - 60px - 16px - 46px)",
      column: [
        {
          selectBoxColumn: true,
        },
        {
          columnName: "STT",
          dataType: Resource.DataTypeColumn.OrderNumber,
        },
        {
          columnName: "Tên món",
          fieldName: "dish_name",
        },
        {
          columnName: "Loại",
          fieldName: "dish_type",
        },
        {
          columnName: "Đơn giá",
          fieldName: "price",
          dataType: Resource.DataTypeColumn.Number,
        },
        {
          columnName: "Trạng thái",
          fieldName: "dish_status",
          dataType: Resource.DataTypeColumn.Enum,
          enumName: "DishStatus"
        },
        {
          columnName: "Chức năng",
          functionColumn: true,
        },
      ],
      functions: ["Nhân bản", "Xóa", "Ngưng sử dụng"],
      defaultFunction: "Sửa",
      gridData: null,
      currentSelectedRows: [],
      idFieldName: "dish_id",
      showPaging: true,
      pageSize: 10,
      currentPageNum: 1,
      totalPage: 1,
      maxPageNumDisplay: 5,
      totalRecord: 0,
      filterValue: null,
    });
    return {
        employeeTable
    };
  },
};
</script>

<style lang="scss">
@import "./ContentProduct.scss";
</style>