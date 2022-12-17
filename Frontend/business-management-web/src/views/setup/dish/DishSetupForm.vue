<template>
  <form v-if="dataForm.showForm">
    <div class="form dish-form">
      <div class="form__title">
        <div class="form__title-left">
          <div class="form__title-text">
            <span class="text-semibold">Thông tin món ăn</span>
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
        <div class="form__dish-img d-flex">
          <div class="form-group">
            <div class="form-row">
              <div class="form-col d-flex">
                <div class="form-item">
                  <FieldInputLabel
                    MustValidate="true"
                    :saveValidate="dataForm.saveValidate"
                    :customData="dataForm.dishNameInput"
                    :model="dataForm.dishData.dish_name"
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
              <div class="form-col d-flex">
                <div class="form-item">
                  <label class="text-semibold"
                    >Loại <span style="color: red">*</span></label
                  >
                  <div MustValidate="true">
                    <ComboBox
                      tabindex="0"
                      :saveValidate="saveValidate"
                      style="margin-top: 4px"
                      :customData="dataForm.dishTypeComboBox"
                      :model="dataForm.dishData.dish_type"
                      @valueChanged="updateValueInput"
                      @invalidData="invalidData"
                    />
                  </div>
                </div>
                <div class="form-item">
                  <FieldInputLabel
                    MustValidate="true"
                    :saveValidate="dataForm.saveValidate"
                    :customData="dataForm.dishPriceInput"
                    :model="dataForm.dishData.price"
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
          <div class="dish-img" @click="changeDishImage">
            <span :class="{ 'd-none': dataForm.dishData.dish_img }"
              >Chọn ảnh</span
            >
            <input
              type="file"
              accept="image/*"
              @change="previewFile($event)"
              ref="ImgInput"
              class="d-none"
            />
            <img
              :src="dataForm.dishData.dish_img"
              alt=""
              width="150"
              height="150"
              min-width="150"
              min-height="150"
              :class="{ 'd-none': !dataForm.dishData.dish_img }"
            />
          </div>
        </div>
        <div class="form-group">
          <div class="form-row">
            <div class="form-col">
              <div class="form-item">
                <FieldInputLabel
                  MustValidate="true"
                  :saveValidate="dataForm.saveValidate"
                  :customData="dataForm.dishDescriptionInput"
                  :model="dataForm.dishData.description"
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
import { useDishSetupFormData } from "./DishSetupFormData";
import Tooltip from "../../../components/Tooltip.vue";
import BasePopup from "../../../components/BasePopup.vue";
import Combobox from "../../../components/Combobox.vue";
import DishAPI from "../../../api/views/setup/DishAPI";

export default {
  components: { Tooltip, BasePopup, Combobox },

  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const { dataForm } = useDishSetupFormData();

    /**
     * Hàm mở form
     */
    async function openForm(dish) {
      if (dish) {
        await DishAPI.getById(dish.dish_id)
          .then((res) => {
            if (res.status == 200) {
              Object.assign(dataForm.dishData, res.data);

              if (
                dataForm.dishData.dish_img &&
                dataForm.dishData.dish_img.length > 0
              ) {
                dataForm.dishData.dish_img =
                  "data:image/jpeg;base64," + dataForm.dishData.dish_img;
              }

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
      dataForm.dishData[key] = value;
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
        dataForm.dishData = {};
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

        // Tạo dataForm để lưu cả ảnh
        var dataSend = new FormData();
        for (var key in dataForm.dishData) {
          if (key != "dish_img") {
            dataSend.append(key, dataForm.dishData[key]);
          }
        }

        for (var pair of dataSend.entries()) {
          console.log(pair[0] + ", " + pair[1]);
        }
        switch (dataForm.formType) {
          //Nếu là form thêm
          case Enumeration.FormMode.Add:
            await DishAPI.insert(dataSend)
              .then((response) => {
                if (response.data.code == 200) {
                  //Hiển thị toast message
                  proxy.$store.commit("SHOW_TOAST", {
                    toastType: Resource.Toast.Success,
                    toastMessage: Resource.Message.AddSuccess,
                  });
                }

                dataForm.dishData = {};
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
            await DishAPI.update(dataForm.dishId, dataSend)
              .then((response) => {
                if (response.data.code == 200) {
                  //Hiển thị toast message
                  proxy.$store.commit("SHOW_TOAST", {
                    toastType: Resource.Toast.Success,
                    toastMessage: Resource.Message.EditSuccess,
                  });
                }

                dataForm.dishData = {};
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

    /**
     * click vào ô thay đổi ảnh
     */
    function changeDishImage() {
      proxy.$refs.ImgInput.click();
    }

    /**
     * xem trước ảnh
     */
    function previewFile(e) {
      dataForm.dishData.img_file = e.target.files[0];

      dataForm.dishData.dish_img = window.URL.createObjectURL(
        dataForm.dishData.img_file
      );
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
      previewFile,
      changeDishImage,
    };
  },
};
</script>
  
<style lang="scss" scoped>
@import "../../../assets/css/common/form.scss";
@import "./DishSetupForm.scss";
</style>