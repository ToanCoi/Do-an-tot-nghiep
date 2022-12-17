import { reactive, ref, getCurrentInstance } from "vue";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";

export const dishSetupData = () => {

    const dishGrid = reactive({
        width: "100%",
        gridHeight: "calc(100vh - var(--header-height) - 39px - 67px - 60px)",
        tableHeight:
            "calc(100% - 60px - 46px)",
        searchPlaceholder: 'Tìm kiếm theo tên món, trạng thái',
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
                enumName: "DishStatus",
            },
            {
                columnName: "Chức năng",
                functionColumn: true,
            },
        ],
        functions: ["Xóa", "Ngưng Bán"],
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
        dishGrid,
    }
}