<template>
  <DxSelectBox
    :height="customData.height"
    :width="customData.width"
    :dataSource="customData.sources"
    :value="customData.sources[0].value"
    display-expr="display"
    value-expr="value"
    @value-change="onValueChanged"
    class="dropdown"
    v-model="currentValue"
  />
</template>


<script>
import DxSelectBox from "devextreme-vue/select-box";

export default {
  components: {
    DxSelectBox,
  },
  props: {
    customData: {
      type: Object,
      required: true,
    },
    model: {},
  },
  created() {
    this.currentValue = this.customData.sources[0].value;
  },
  data() {
    return {
      currentValue: "",
    };
  },
  methods: {
    /**
     * Hàm xử lý khi thay chọn giá trị
     */
    onValueChanged(e) {

      //truyền key ra ngoài để thay đổi
      this.$emit("valueChanged", e.value);
    },
  },
};
</script>

<style>
@import url("../assets/css/components/dropdown.css");
</style>