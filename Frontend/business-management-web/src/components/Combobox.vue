<template>
  <div
    class="tooltip-error"
    :style="{ '--scale': scaleTooltip, '--tooltip-message': tooltipMsg }"
  >
    <DxSelectBox
      ref="DxDateBox"
      :height="customData.height"
      :width="customData.width"
      :searchEnabled="true"
      :placeholder="customData.defaultValue"
      noDataText="Không có dữ liệu hợp lệ"
      :dataSource="customData.displayValues"
      :searchTimeout="0"
      :isValid="isValid"
      :onInput="onInput"
      :onFocusOut="onFocusOut"
      :onValueChanged="onValueChanged"
      class="dxCombobox field-input"
      v-model="currentValue"
    />
  </div>
  <!-- <Tooltip :customData="errorMessage"> -->
  <!-- <DxSelectBox
    ref="DxDateBox"
    :height="customData.height"
    :width="customData.width"
    :searchEnabled="true"
    :placeholder="customData.defaultValue"
    noDataText="Không có dữ liệu hợp lệ"
    :dataSource="customData.displayValues"
    :searchTimeout="0"
    :isValid="isValid"
    :onInput="onInput"
    :onFocusOut="onFocusOut"
    :onValueChanged="onValueChanged"
    class="dxCombobox field-input"
    v-model="currentValue"
  /> -->
  <!-- </Tooltip> -->
</template>


<script>
import DxSelectBox from "devextreme-vue/select-box";
// import Tooltip from "./Tooltip.vue";
import CommonFn from "../js/common/CommonFn";

export default {
  components: {
    DxSelectBox,
    // Tooltip,
  },
  props: {
    customData: {
      type: Object,
      required: true,
    },
    saveValidate: {
      default: false,
    },
    model: {},
  },
  data() {
    return {
      currentValue: "",
      isValid: true,
      value: "",
      errorMessage: "",
      tooltipMsg: "",
      scaleTooltip: 0,
    };
  },
  created() {
    this.currentValue =
      this.customData.displayValues[this.customData.keys.indexOf(this.model)];
  },
  watch: {
    model: function (val) {
      this.currentValue =
        this.customData.displayValues[this.customData.keys.indexOf(val)];
    },
    saveValidate: function (val) {
      //Nếu ấn nút save thì validate
      if (val) {
        this.onFocusOut();
      }
    },
  },
  methods: {
    /**
     * Xử lý sự kiện khi người dùng đang nhập vào
     */
    onInput(e) {
      let currentText = e.component.option("text");

      if (currentText) {
        this.errorMessage = "";
      }

      if (this.customData.displayValues.includes(currentText)) {
        this.currentValue = currentText;
        this.isValid = true;
        this.scaleTooltip = 0;
      } else {
        this.isValid = false;
      }
    },

    /**
     * Xử lý sự kiện khi blur ra ngoài
     */
    onFocusOut() {
      //Nếu là ấn nút save thì validate
      if (!this.currentValue && this.saveValidate) {
        this.isValid = false;

        this.errorMessage = CommonFn.getRequiredErrorMsg(
          this.customData.labelText
        );

        //Hiện tooltip lỗi
        this.tooltipMsg = "'" + this.errorMessage + "'";
        this.scaleTooltip = 1;

        //Nếu người dùng không nhập nữa mà lỗi thì truyền ra thông báo lỗi
        this.$emit("invalidData", this.errorMessage);
      } else {
        this.isValid = true;
        this.scaleTooltip = 0;
      }
    },

    /**
     * Hàm xử lý khi thay chọn giá trị
     */
    onValueChanged() {
      this.isValid = true;
      this.scaleTooltip = 0;

      //Lấy index của giá trị hiện tại
      let index = this.customData.displayValues.indexOf(this.currentValue);

      index = index != -1 ? index : null;

      //truyền key ra ngoài để thay đổi
      this.$emit(
        "valueChanged",
        this.customData.inputFieldName,
        this.customData.keys[index]
      );
    },
  },
};
</script>

<style>
@import url("../assets/css/components/combobox.css");
@import url("../assets/css/common/tooltip-error.css");
</style>