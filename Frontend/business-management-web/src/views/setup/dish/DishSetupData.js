import { reactive, ref, getCurrentInstance } from "vue";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";

export const dishSetupData = () => {
    
    const dishGrid = reactive({
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
                enumName: "DishStatus",
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
        dishGrid,
    }
}