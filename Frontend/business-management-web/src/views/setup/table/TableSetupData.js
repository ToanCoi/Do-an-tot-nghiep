import { reactive, ref, getCurrentInstance } from "vue";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Enumeration from "../../../js/common/Enumeration.js";

export const tableSetupData = () => {

    const tableGrid = reactive({
        width: "100%",
        gridHeight: "calc(100vh - var(--header-height) - 86px - 60px - 16px)",
        tableHeight:
            "calc(100vh - var(--header-height) - 86px - 60px - 16px - 46px)",
        searchPlaceholder: 'Tìm kiếm theo số bàn, tầng, trạng thái',
        column: [
            {
                selectBoxColumn: true,
            },
            {
                columnName: "STT",
                dataType: Resource.DataTypeColumn.OrderNumber,
            },
            {
                columnName: "Tầng",
                fieldName: "floor",
            },
            {
                columnName: "Số bàn",
                fieldName: "table_name",
            },
            {
                columnName: "Số ghế tối đa",
                fieldName: "max_size",
            },
            {
                columnName: "Trạng thái",
                fieldName: "table_status",
                dataType: Resource.DataTypeColumn.Enum,
                enumName: "TableStatus",
            },
            {
                columnName: "Chức năng",
                functionColumn: true,
            },
        ],
        functions: ["Nhân bản", "Xóa", "Ngưng sử dụng"],
        defaultFunction: "Sửa",
        gridData: [{
            floor: 1,
            table_name: 2,
            max_size: 4,
            table_status: Enumeration.TableStatus.InProgress
        },
        {
            floor: 1,
            table_name: 2,
            max_size: 4,
            table_status: Enumeration.TableStatus.New
        },
        {
            floor: 1,
            table_name: 2,
            max_size: 4,
            table_status: Enumeration.TableStatus.InProgress
        },
        {
            floor: 1,
            table_name: 2,
            max_size: 4,
            table_status: Enumeration.TableStatus.InProgress
        }],
        currentSelectedRows: [],
        idFieldName: "dish_id",
        showPaging: true,
        pageSize: 10,
        currentPageNum: 1,
        totalPage: 1,
        maxPageNumDisplay: 5,
        totalRecord: 4,
        filterValue: null,
    });


    return {
        tableGrid,
    }
}