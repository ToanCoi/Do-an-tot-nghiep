<template>
    <div class="invoice__form" v-if="showForm">
sdffsdf
    </div>
</template>

<script>
import { reactive, ref, getCurrentInstance, computed } from "vue";

export default {
    setup: (props) => {
        const showForm = ref(false);

        function openForm() {
            showForm.value = true;
        }

        return {
            showForm,
            openForm
        }
    }
}
</script>

<style>
@import url("./RestaurantSellInvoice.scss");
</style>