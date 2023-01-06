<template>
  <form v-if="dataForm.showForm">
    <div class="form">
      <div class="form__title">
        <div class="form__title-left">
          <div class="form__title-text">
            <span class="text-semibold">Thông tin bàn</span>
          </div>
        </div>
        <div class="form__title-right d-flex">
          <Tooltip :customData="'Trợ giúp'">
            <div class="page-icon">
              <div class="form__icon-help"></div>
            </div>
          </Tooltip>
          <Tooltip :customData="'Đóng'">
            <div class="page-icon" @click="closeForm">
              <div class="form__icon-cancel"></div>
            </div>
          </Tooltip>
        </div>
      </div>
      <div class="form__content" ref="FormData">
        <div class="form-group">
          <div class="form-row">
            <div class="form-col d-flex">
              <div class="form-item">
                <FieldInputLabel
                  MustValidate="true"
                  :saveValidate="dataForm.saveValidate"
                  :customData="dataForm.floorInput"
                  :model="dataForm.tableData.floor"
                  @invalidData="invalidData"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                  @checkUnique="checkUnique"
                  :ref="inputFieldName"
                />
              </div>
              <div class="form-item">
                <FieldInputLabel
                  MustValidate="true"
                  :saveValidate="dataForm.saveValidate"
                  :customData="dataForm.tableNameInput"
                  :model="dataForm.tableData.table_name"
                  @invalidData="invalidData"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                  @checkUnique="checkUnique"
                  :ref="inputFieldName"
                />
              </div>
            </div>
          </div>
          <div class="form-row">
            <div class="form-col">
              <div class="form-item">
                <FieldInputLabel
                  MustValidate="true"
                  :saveValidate="dataForm.saveValidate"
                  :customData="dataForm.maxSizeInput"
                  :model="dataForm.tableData.max_size"
                  @invalidData="invalidData"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                  @checkUnique="checkUnique"
                  :ref="inputFieldName"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="form__footer">
        <div
          class="btn btn-default text-semibold"
          tabindex="0"
          @click="dataForm.showForm = false"
          @keyup.enter="dataForm.showForm = false"
        >
          Hủy
        </div>
        <div class="btn-group d-flex">
          <div
            class="btn btn-primary text-semibold"
            tabindex="0"
            @click="saveAndOut"
            @keyup.enter="saveAndOut"
          >
            Cất
          </div>
          <div
            class="btn btn-primary text-semibold"
            tabindex="0"
            @click="saveAndAdd"
            @keyup.enter="saveAndAdd"
          >
            Cất và thêm
          </div>
        </div>
      </div>
    </div>

    <!-- Popup thông báo lỗi -->
    <BasePopup class="popup-error" v-show="dataForm.showErrorPopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div
            :class="
              dataForm.uniqueError
                ? 'popup__icon-warning'
                : 'popup__icon-danger'
            "
          ></div>
        </div>
        <div class="popup__text">
          {{ dataForm.errorMessage }}
        </div>
      </template>
      <template #footer>
        <div
          class="footer__btn d-flex"
          :class="dataForm.uniqueError ? 'flex-end' : 'flex-center'"
        >
          <div class="btn btn-primary" @click="closeErrorPopup">
            {{ dataForm.uniqueError ? "Đồng ý" : "Đóng" }}
          </div>
        </div>
      </template>
    </BasePopup>
  </form>
</template>

<script>
import { reactive, ref, getCurrentInstance } from "vue";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import { useTableSetupFormData } from "./TableSetupFormData";
import Tooltip from "../../../components/Tooltip.vue";
import BasePopup from "../../../components/BasePopup.vue";
import TableAPI from "../../../api/views/setup/TableAPI";

export default {
  components: { Tooltip, BasePopup },

  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const { dataForm } = useTableSetupFormData();

    /**
     * Hàm mở form
     */
    async function openForm(table) {
      if (table) {
        await TableAPI.getById(table.table_id)
          .then((res) => {
            if (res.status == 200) {
              Object.assign(dataForm.tableData, res.data);

              dataForm.formType = Enumeration.FormMode.Edit;
            } else {
              //Nếu không lấy được dữ liệu từ db
              dataForm.allInputValid = false;
              dataForm.errorMessage = Resource.Message.ServerError;
              dataForm.showErrorPopup = true;
            }
          })
          .catch((ex) => {
            //Nếu không lấy được dữ liệu từ db
            dataForm.allInputValid = false;
            dataForm.errorMessage = Resource.Message.ServerError;
            dataForm.showErrorPopup = true;
          });
      } else {
        dataForm.formType = Enumeration.FormMode.Add;
      }
      dataForm.showForm = true;
    }

    /**
     * Hàm đóng form
     */
    function closeForm() {
      if (proxy.dataChanged()) {
        dataForm.showConfirmPopup = true;
      } else {
        dataForm.showForm = false;
      }
    }

    /**
     * Kiểm tra dữ liệu đã thay đổi chưa
     */
    function dataChanged() {
      return false;
    }

    /**
     * Hàm update dữ liệu khi người dùng thay đổi
     * NVTOAN 06/07/2021
     */
    function updateValueInput(key, value) {
      dataForm.tableData[key] = value;
    }

    /**
     * Cất và thêm - không đóng form
     */
    async function saveAndAdd() {
      dataForm.saveValidate = true;
      await save();

      //Nếu thêm thành công
      if (dataForm.allInputValid) {
        dataForm.saveValidate = false;
        dataForm.tableData = {};
      }
    }

    async function saveAndOut() {
      await save();

      //Nếu thêm thành công
      if (dataForm.allInputValid) {
        dataForm.showForm = false;
      }
    }

    /**
     * Cất dữ liệu
     */
    async function save() {
      //Biến đánh dấu để hiện popup
      dataForm.saveValidate = true;

      //Trước khi lưu cần validate hết
      await validateAll();

      if (dataForm.allInputValid) {
        proxy.$store.commit("SHOW_LOADER", true);

        switch (dataForm.formType) {
          //Nếu là form thêm
          case Enumeration.FormMode.Add:
            await TableAPI.insert(dataForm.tableData)
              .then((response) => {
                if (response.data.code == 200) {
                  //Hiển thị toast message
                  proxy.$store.commit("SHOW_TOAST", {
                    toastType: Resource.Toast.Success,
                    toastMessage: Resource.Message.AddSuccess,
                  });
                }

                dataForm.tableData = {};
                proxy.$emit("refreshData");
              })
              .catch((e) => {
                proxy.$store.commit("SHOW_LOADER", false);

                //Nếu không lưu được thì thông báo lỗi
                dataForm.allInputValid = false;
                dataForm.errorMessage = e.response.data.data.status[0]
                  ? e.response.data.data.status[0]
                  : Resource.Message.ServerError;
                dataForm.showErrorPopup = true;
              });
            break;
          //Nếu là form sửa
          case Enumeration.FormMode.Edit:
            await TableAPI.update(dataForm.tableData)
              .then((response) => {
                if (response.data.code == 200) {
                  //Hiển thị toast message
                  proxy.$store.commit("SHOW_TOAST", {
                    toastType: Resource.Toast.Success,
                    toastMessage: Resource.Message.EditSuccess,
                  });
                }
                
                dataForm.tableData = {};
                proxy.$emit("refreshData");
              })
              .catch(() => {
                proxy.$store.commit("SHOW_LOADER", false);

                //Nếu không lưu được thì thông báo lỗi
                dataForm.allInputValid = false;
                dataForm.errorMessage = Resource.Message.ServerError;
                dataForm.showErrorPopup = true;
              });
            break;
        }
        proxy.$store.commit("SHOW_LOADER", false);
      }
    }

    /**
     * Hàm validate tất cả dữ liệu trước khi thêm
     */
    async function validateAll() {
      //Reset
      dataForm.allInputValid = true;
      dataForm.allUniue = true;

      //Lấy tất cả trường cần validate
      let elValidate = proxy.$refs.FormData.querySelectorAll("[MustValidate]");

      //Validate tất cả trường
      for (let i = 0; i < elValidate.length; i++) {
        await elValidate[i].querySelector(".field-input").focus();
        await elValidate[i].querySelector(".field-input").blur();
      }

      // await this.checkUnique("EmployeeCode", this.employee.employeeCode);

      // if (!this.allUniue) {
      //   let errorMessage = CommonFn.getUniqueErrorMsg(
      //     "Mã nhân viên <" + this.employee.employeeCode + ">"
      //   );

      //   await this.invalidData(errorMessage, Enumeration.ErrorType.Unique);
      // }
    }

    /**
     * Đánh dấu dữ liệu chưa hợp lệ
     */
    function invalidData(errorMessage, typeError) {
      dataForm.allInputValid = false;

      //Nếu là ấn nút save và chưa có lỗi nào trước đó thì lấy lỗi đầu tiên để hiển thị
      if (!dataForm.errorMessage && dataForm.saveValidate) {
        dataForm.errorMessage = errorMessage;

        //Tùy vào các loại lỗi thì hiển thị popup tương ứng
        switch (typeError) {
          case Enumeration.ErrorType.Require:
          case Enumeration.ErrorType.DataType:
          case Enumeration.ErrorType.MaxLength:
            dataForm.uniqueError = false;
            break;
          case Enumeration.ErrorType.Unique:
            dataForm.uniqueError = true;
            break;
        }

        dataForm.showErrorPopup = true;
      }
    }

    /**
     * Hàm thực hiện đóng popup lỗi
     */
    function closeErrorPopup() {
      dataForm.showErrorPopup = false;

      //Reset lại giá trị all valid
      dataForm.allInputValid = true;

      //Reset lại thông điệp lỗi
      dataForm.errorMessage = "";

      //reset biến đánh dấu ấn save
      dataForm.saveValidate = false;
    }

    return {
      Enumeration,
      CommonFn,
      Resource,
      dataForm,
      openForm,
      closeForm,
      dataChanged,
      invalidData,
      closeErrorPopup,
      validateAll,
      save,
      updateValueInput,
      saveAndAdd,
      saveAndOut,
    };
  },
};
</script>

<style lang="scss">
@import "../../../assets/css/common/form.scss";
</style>