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
                  :model="dataForm.employee.employeeCode"
                  @invalidData="dataForm.invalidData"
                  @updateValueInput="dataForm.updateValueInput"
                  @getOriginData="dataForm.getOriginData"
                  @checkUnique="dataForm.checkUnique"
                  :ref="dataForm.floorInput.inputFieldName"
                />
              </div>
              <div class="form-item">
                <FieldInputLabel
                  MustValidate="true"
                  :saveValidate="dataForm.saveValidate"
                  :customData="dataForm.tableNameInput"
                  :model="dataForm.employee.employeeCode"
                  @invalidData="dataForm.invalidData"
                  @updateValueInput="dataForm.updateValueInput"
                  @getOriginData="dataForm.getOriginData"
                  @checkUnique="dataForm.checkUnique"
                  :ref="dataForm.tableNameInput.inputFieldName"
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
                  :model="dataForm.employee.employeeCode"
                  @invalidData="dataForm.invalidData"
                  @updateValueInput="dataForm.updateValueInput"
                  @getOriginData="dataForm.getOriginData"
                  @checkUnique="dataForm.checkUnique"
                  :ref="dataForm.maxSizeInput.inputFieldName"
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
            class="btn btn-default text-semibold"
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
  </form>
</template>

<script>
import { reactive, ref, getCurrentInstance } from "vue";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import { useTableSetupFormData } from "./TableSetupFormData";
import Tooltip from "../../../components/Tooltip.vue";

export default {
  components: { Tooltip },

  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const { dataForm } = useTableSetupFormData();

    /**
     * Hàm mở form
     */
    function openForm() {
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

    return {
      Enumeration,
      CommonFn,
      Resource,
      dataForm,
      openForm,
      closeForm,
      dataChanged,
    };
  },
};
</script>

<style lang="scss">
@import "../../../assets/css/common/form.scss";
</style>