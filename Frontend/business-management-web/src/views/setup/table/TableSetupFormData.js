import { reactive, ref, getCurrentInstance } from "vue";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Enumeration from "../../../js/common/Enumeration.js";

export const useTableSetupFormData = () => {

    const dataForm = reactive({
        //input
        floorInput: {
            inputFieldName: "floor",
            labelText: "Tầng",
            width: "calc(var(--column-width) * 1.5 - 10px)",
            height: "35px",
            isRequired: true,
            inputType: "text",
            isUnique: true,
            maxLength: 50,
        },
        tableNameInput: {
            inputFieldName: "table_name",
            labelText: "Tên bàn",
            width: "calc(var(--column-width) * 3 - 10px)",
            height: "35px",
            isRequired: true,
            inputType: "text",
            maxLength: 50,
        },
        maxSizeInput: {
            inputFieldName: "max_size",
            labelText: "Số ghế tối đa",
            width: "calc(var(--column-width) * 4.5 + 2px)",
            height: "35px",
            inputType: "text",
            maxLength: 50,
        },
        

        showForm: false,
        showConfirmPopup: false,
        showErrorPopup: false,
        customerBoxSelected: false,
        providerBoxSelected: false,
        employee: {},
        originData: {},
        employeeId: null,
        saveValidate: false,
        allInputValid: true,
        allUniue: true,
        formType: null,
        errorMessage: "",
        uniqueError: false,
    });


    return {
        dataForm,
    }
}