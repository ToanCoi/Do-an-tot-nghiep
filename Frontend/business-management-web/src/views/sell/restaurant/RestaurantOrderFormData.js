import { reactive, ref, getCurrentInstance } from "vue";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Enumeration from "../../../js/common/Enumeration.js";

export const useRestaurantOrderFormData = () => {

    const dataForm = reactive({
        //input
        customerNameInput: {
            inputFieldName: "customer_name",
            labelText: "Tên khách hàng",
            width: "calc(var(--column-width) * 2 + 10px)",
            height: "30px",
            inputType: "text",
            maxLength: 50,
            isRequired: true,
        },
        customerPhoneInput: {
            inputFieldName: "customer_phone",
            labelText: "SĐT",
            width: "calc(var(--column-width) * 2 + 10px)",
            height: "30px",
            dataType: "Number",
            maxLength: 20,
            isRequired: true,
        },
        showForm: false,
        showConfirmPopup: false,
        showErrorPopup: false,
        customerBoxSelected: false,
        providerBoxSelected: false,
        orderData: {
            dishes: []
        },
        originData: {},
        dishId: null,
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