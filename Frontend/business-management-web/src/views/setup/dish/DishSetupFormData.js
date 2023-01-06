import { reactive, ref, getCurrentInstance } from "vue";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Enumeration from "../../../js/common/Enumeration.js";

export const useDishSetupFormData = () => {

    const dataForm = reactive({
        //input
        dishNameInput: {
            inputFieldName: "dish_name",
            labelText: "Tên món",
            width: "calc(var(--column-width) * 3.5 + 10px)",
            height: "35px",
            isRequired: true,
            inputType: "text",
            maxLength: 50,
        },
        dishTypeComboBox: {
            inputFieldName: "dish_type",
            labelText: "Đơn vị",
            defaultValue: "",
            displayValues: [
                Resource.DishType.Appetizer,
                Resource.DishType.MainDish,
                Resource.DishType.Drink,
                Resource.DishType.Dessert
            ],
            keys: [
                Enumeration.DishType.Appetizer,
                Enumeration.DishType.MainDish,
                Enumeration.DishType.Drink,
                Enumeration.DishType.Dessert
            ],
            width: "calc(var(--column-width) * 2)",
            height: "37px",
        },
        dishPriceInput: {
            inputFieldName: "price",
            labelText: "Giá",
            width: "calc(var(--column-width) * 1.5)",
            height: "35px",
            dataType: "Number",
            maxLength: 50,
        },
        dishDescriptionInput: {
            inputFieldName: "description",
            labelText: "Mô tả",
            width: "calc(var(--column-width) * 5.5 + 5px)",
            height: "50px",
            inputType: "textArea",
            maxLength: 50,
        },


        showForm: false,
        showConfirmPopup: false,
        showErrorPopup: false,
        customerBoxSelected: false,
        providerBoxSelected: false,
        dishData: {},
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