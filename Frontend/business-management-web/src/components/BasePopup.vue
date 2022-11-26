<template>
    <div class="popup__wrapper">
    <div class="popup">
      <div class="popup__content">
        <slot name="content"></slot>
      </div>
      <div class="popup__footer">
        <slot name="footer"></slot>
      </div>
    </div>
  </div>
</template>

<script>
export default {
    
}
</script>

<style scoped>
@import url('../assets/css/components/popup.css');
</style>