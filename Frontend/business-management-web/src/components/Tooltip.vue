<template>
  <div
    class="tooltip"
    @mouseenter="displayTooltip"
    @mouseleave="disableTooltip"
  >
    <slot></slot>
    <span
      class="tooltip__content"
      :style="[
        { left: left },
        { right: right },
        { bottom: bottom },
        { top: top },
      ]"
      style="left: 196px"
      v-show="showTooltip && customData.length > 0"
      >{{ customData }}</span
    >
  </div>
</template>

<script>
export default {
  props: {
    customData: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      showTooltip: false,
      left: null,
      right: null,
      bottom: null,
      top: null,
    };
  },
  methods: {
    /**
     * Hàm xác định vị trí tooltip
     * NVTOAN 10/07/2021
     */
    async displayTooltip(e) {
      let tooltip = e.target.closest(".tooltip");
      let tooltipSize = tooltip.getBoundingClientRect();

      await setTimeout(() => {
        this.showTooltip = true;
      }, 0.1);

      setTimeout(() => {
        //Lấy tooltip content
        let tooltipContent = tooltip.querySelector(".tooltip__content");

        let contentSize = tooltipContent.getBoundingClientRect();

        if (e.pageX + contentSize.width + 20 < window.innerWidth) {
          this.left = e.pageX - tooltipSize.x + 20 + "px";
        } else {
          this.right = e.pageX - tooltipSize.x + 10 + "px";
        }

        if (e.pageY + contentSize.height + 10 < window.innerHeight) {
          this.top = e.pageY - tooltipSize.y + 10 + "px";
        } else {
          this.bottom = e.pageY - tooltipSize.y + 10 + "px";
        }
      }, 0.1);
    },

    /**
     * Hàm tắt tooltip
     * NVTOAN 12/07/2021
     */
    disableTooltip() {
      this.showTooltip = false;
      this.left = null;
      this.right = null;
      this.bottom = null;
      this.top = null;
    }
  },
};
</script>

<style>
.tooltip {
  position: relative;
  outline: none;
}

.tooltip__content {
  position: absolute;
  white-space: nowrap;
  background: rgb(56, 55, 55);
  padding: 5px;
  z-index: 10000;
  font-family: 'Notosans-regular';
  font-weight: 300 !important;
  color: #fff;
  font-size: 12px;
}
</style>