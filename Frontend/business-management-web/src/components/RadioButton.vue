<template>
  <div class="radio">
    <label class="text-semibold">{{ customData.labelText }}</label>
    <div class="radio__list" :style="{ height: customData.height }">
      <div
        class="radio-item"
        v-for="(item, index) in customData.items"
        :key="index"
      >
        <input
          type="radio"
          :id="item.value"
          :name="customData.inputFieldName"
          :value="item.value"
          v-model="cloneModel"
        />
        <label :for="item.value">{{ item.text }}</label>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    customData: {
      type: Object,
      required: true,
    },
    model: {
      required: true,
    },
  },
  created() {
    this.cloneModel = this.model ? this.model : this.customData.defaultValue;
  },
  data() {
    return {
      cloneModel: null,
    };
  },
  watch: {
    model: function (val) {
      this.cloneModel = val;
    },
    cloneModel: function (val) {
      this.$emit("updateValueInput", this.customData.inputFieldName, val);
    },
  },
};
</script>

<style scoped>
@import url("../assets/css/components/radio-button.css");
</style>