<template>
  <Table
    ref="Table"
    :customData="dishGrid"
    @dbClickRow="openForm"
    @clickDefaultFunction="openForm"
    @clickFunctionItem="clickFunctionItem"
    @clickPageNum="changePageNum"
    @changePageSize="changePageSize"
    @changeListSelectedRow="changeListSelectedRow"
  />
</template> 
    
<script>
import { reactive, ref, getCurrentInstance } from "vue";
import { dishSetupData } from "./DishSetupData";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import Table from "../../../components/table/Table.vue";

export default {
  components: { Table },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const { dishGrid } = dishSetupData();

    return {
      Enumeration,
      CommonFn,
      Resource,
      dishGrid,
    };
  },
};
</script>
  
  <style lang="scss">
@import "./DishSetup.scss";
</style>