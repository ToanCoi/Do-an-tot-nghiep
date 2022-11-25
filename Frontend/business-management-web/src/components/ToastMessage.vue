<template>
  <transition name="slide">
    <div class="toast" :class="customData.toastType" v-show="isShow">
      <div class="toast__icon">
        <div
          :class="{
            'toast__icon-success': customData.toastType == Resource.Toast.Success,
            'toast__icon-warning':
              customData.toastType == Resource.Toast.Warning,
            'toast__icon-danger':
              customData.toastType == Resource.Toast.Error,
          }"
        ></div>
      </div>
      <div class="toast__content">{{ customData.toastMessage }}</div>
      <div class="toast__cancel" @click="isShow = false">
        <div class="toast__icon-cancel"></div>
      </div>
    </div>
  </transition>
</template>

<script>
import Resource from '../js/common/Resource';
export default {
  props: {
    customData: {
      type: Object,
      required: true,
    },
  },
  data() {
      return {
          isShow: false,
          Resource: Resource
      }
  },
  methods: {
      /**
       * Hàm hiện toast messenger
       */
      showToast() {
          var me = this;

          this.isShow = true;
          setTimeout(function() {
              me.isShow = false;
          }, 3000);
      }
  }
};
</script>

<style scoped>
@import url('../assets/css/components/toast.css');

.slide-leave-active,
.slide-enter-active {
  transition: all 0.3s;
}

.slide-enter,
.slide-leave-to {
  transform: translateX(100%);
  opacity: 0;
}
</style>