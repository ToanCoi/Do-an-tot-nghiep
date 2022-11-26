<template>
  <div
    class="input-label tooltip-error"
    :style="{ '--scale': scaleTooltip, '--tooltip-message': tooltipMsg }"
  >
    <label :for="customData.inputFieldName" class="text-semibold"
      >{{ customData.labelText }}
      <span v-if="customData.isRequired">
        <span style="color: red">*</span>
      </span>
    </label>
    <!-- <Tooltip :customData="errorMessage"> -->
    <template>
      <DxDateBox
        v-if="customData.dataType == 'Date'"
        v-model="cloneModel"
        displayFormat="dd/MM/yyyy"
        :height="customData.height"
        placeholder="dd/mm/yyyy"
        :useMaskBehavior="true"
        :width="customData.width"
        :showClearButton="true"
        class="dxDateBox"
      />
      <input
        v-else-if="customData.dataType == 'Number'"
        autocomplete="off"
        :style="[{ width: customData.width }, { height: customData.height }]"
        class="field-input"
        :class="{ invalid: invalidInput }"
        :placeholder="customData.placeholder"
        :id="customData.inputFieldName"
        @focus="focus"
        @blur="blur"
        @keydown="preventTextNaN($event)"
        ref="input"
        v-model="cloneModel"
      />
      <textarea
        v-else-if="customData.inputType == 'textArea'"
        :rows="customData.row"
        autocomplete="off"
        :style="[{ width: customData.width }]"
        class="field-input"
        :class="{ invalid: invalidInput }"
        :placeholder="customData.placeholder"
        :id="customData.inputFieldName"
        @focus="focus"
        @blur="blur"
        ref="input"
        v-model="cloneModel"
      />
      <input
        v-else
        autocomplete="off"
        :style="[{ width: customData.width }, { height: customData.height }]"
        class="field-input"
        :class="{ invalid: invalidInput }"
        :placeholder="customData.placeholder"
        :id="customData.inputFieldName"
        @focus="focus"
        @blur="blur"
        ref="input"
        v-model="cloneModel"
      />
    </template>
    <!-- </Tooltip> -->
  </div>
</template>

<script>
import DxDateBox from "devextreme-vue/date-box";
import Resource from "../js/common/Resource";
import moment from "moment";
import CommonFn from "../js/common/CommonFn";
import Enumeration from "../js/common/Enumeration";
// import Tooltip from "../components/Tooltip.vue";

export default {
  components: {
    DxDateBox,
    // Tooltip,
  },
  props: {
    customData: {
      type: Object,
      require: true,
    },
    model: {
      default: null,
    },
    saveValidate: {
      default: false,
    },
  },
  data() {
    return {
      errorMessage: "",
      tooltipMsg: "",
      invalidInput: false,
      valueUnique: true,
      cloneModel: "",
      focused: false,
      typed: false,
      originData: null,
      showTooltip: false,
      scaleTooltip: 0,
      money: {
        decimal: "",
        thousands: ".",
        prefix: "",
        suffix: " (VND) ",
        precision: 0,
        masked: false,
      },
    };
  },
  created() {
    this.cloneModel = this.model;
  },
  watch: {
    cloneModel: function (val) {
      //Nếu người dùng chưa nhấn gì thì truyền ra dữ liệu gốc
      if (!this.focused) {
        this.originData = val;
        this.$emit("getOriginData", this.customData.inputFieldName, val);
      }

      //Nếu người dùng đã từng gõ thì đánh dấu là đã gõ để validate
      if (val != this.originData) {
        this.typed = true;
      }

      //Nếu là required thì validate khi đang nhập
      if (this.customData.isRequired && this.typed && val != null) {
        this.invalidInput = !this.validateRequired(val);
      }

      //Nếu người dùng đang nhập thì check max length bỏ cảnh báo đỏ và format dữ liệu
      if (val) {
        if (
          this.customData.maxLength &&
          val.length >= this.customData.maxLength
        ) {
          this.displayMaxLengthError();
        } else {
          this.invalidInput = false;
          this.scaleTooltip = 0;
          this.errorMessage = "";

          //Format dữ liệu dựa theo kiểu dữ liệu tương ứng
          val = this.formatData(val);
        }
      }

      //Nếu xóa hết trong khi trường đó không bắt buộc nhập thì xóa hết lỗi
      if (!val && !this.customData.isRequired) {
        this.invalidInput = false;
        this.scaleTooltip = 0;
        this.errorMessage = "";
      }

      //Thay đổi model gốc ra ngoài
      this.$emit("updateValueInput", this.customData.inputFieldName, val);
    },
    model: {
      deep: true,
      immediate: true,
      handler(val) {
        this.cloneModel = JSON.parse(JSON.stringify(val));
      },
    },
    saveValidate(val) {
      if (val) {
        this.validate();
      }
    },
  },
  methods: {
    /**
     * Hàm xử lý sự kiện focus
     */
    focus() {
      this.focused = true;
      this.$refs.input.select();
    },
    /**
     * Hàm xử lý sự kiện khi không focus vào input nữa (check validate)
     */
    blur() {
      if (this.typed) {
        this.validate();
      }
    },

    /**
     * Hàm ngăn không cho nhập không phải số
     */
    preventTextNaN(event) {
      //Nếu phím nhập vào không phải số thì không cho nhập
      if (
        !isFinite(event.key) &&
        event.keyCode != 8 &&
        event.keyCode != 190 &&
        event.keyCode != 9 &&
        event.keyCode != 37 &&
        event.keyCode != 39
      ) {
        event.preventDefault();
      }
    },

    /**
     * Hàm format dữ liệu để hiển thị theo kiểu dữ liệu tương ứng
     */
    formatData(val) {
      switch (this.customData.dataType) {
        case Resource.DataTypeColumn.Number:
          return val.toString().replace(/[^\d]/gi, "");
        case Resource.DataTypeColumn.Date:
          return (this.cloneModel = moment(this.cloneModel).format(
            "yyyy-MM-DD"
          ));
        case Resource.DataTypeColumn.Name:
          return CommonFn.formatName(val);
        default:
          return val;
      }
    },

    /**
     * Hàm validate dữ liệu
     */
    validate() {
      let value = this.cloneModel,
        isValid = true;

      if (this.customData.isRequired) {
        isValid = this.validateRequired(value);
      }

      if (isValid && value) {
        isValid = this.validateDataType(value);
      }

      if (!isValid) {
        this.invalidInput = true;
      }
    },

    /**
     * Hàm validate required
     */
    validateRequired(value) {
      if (!value || value.length <= 0) {
        //Thông điệp lỗi
        this.errorMessage = CommonFn.getRequiredErrorMsg(
          this.customData.labelText
        );
        this.displayTooltip();

        //Dữ liệu chưa hợp lệ
        this.$emit(
          "invalidData",
          this.errorMessage,
          Enumeration.ErrorType.Require
        );

        return false;
      }

      return true;
    },

    /**
     * Hàm gọi cha để validate unique
     */
    async validateUnique(value) {
      //gọi cha truy vấn api xem có dữ liệu hay chưa
      await this.$emit("checkUnique", this.customData.inputFieldName, value);

      return true;
    },

    /**
     * Hàm đánh dấu giá trị nhập không unique
     */
    async valueNotUnique() {
      //Đánh dấu không unque để hiện lỗi
      this.invalidInput = true;

      //Thông điệp lỗi
      let valueMsg = this.customData.labelText + "<" + this.cloneModel + ">";
      this.errorMessage = CommonFn.getUniqueErrorMsg(valueMsg);
      this.displayTooltip();

      //Dữ liệu chưa hợp lệ
      await this.$emit(
        "invalidData",
        this.errorMessage,
        Enumeration.ErrorType.Unique
      );
    },

    /**
     * Hàm validate theo kiểu dữ liệu
     */
    validateDataType(value) {
      let res = true;

      switch (this.customData.dataType) {
        case Resource.DataTypeColumn.Email:
          res = this.validateEmail(value);
          break;
      }

      if (!res) {
        this.errorMessage = CommonFn.getDataTypeErrorMsg(
          this.customData.labelText
        );
        this.displayTooltip();

        //Dữ liệu chưa hợp lệ
        this.$emit(
          "invalidData",
          this.errorMessage,
          Enumeration.ErrorType.DataType
        );
      }

      return res;
    },

    /**
     * Validate Email
     */
    validateEmail(value) {
      let regex =
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

      return regex.test(value);
    },

    /**
     * Hàm hiện lỗi max length
     */
    displayMaxLengthError() {
      //Hiện lỗi
      this.invalidInput = true;

      //Tạo thông điệp lỗi
      this.errorMessage = CommonFn.getMaxLengthErrorMsg(
        this.customData.labelText,
        this.customData.maxLength
      );
      this.displayTooltip();

      //Thông báo lỗi ra ngoài
      this.$emit(
        "invalidData",
        this.errorMessage,
        Enumeration.ErrorType.MaxLength
      );
    },

    /**
     * Hàm hiển thị tooltip
     */
    displayTooltip() {
      this.tooltipMsg = "'" + this.errorMessage + "'";
      this.scaleTooltip = 1;
      this.invalidInput = true;
    },
  },
};
</script>

<style scoped>
@import url("../assets/css/components/filed-input-label.css");
@import url("../assets/css/common/tooltip-error.css");

.tooltip-error::before,
.tooltip-error::after {
  top: 30px;
}
</style>