<template>
  <form v-if="dataForm.showForm">
    <div class="form restaurant__form-order">
      <div class="form__title">
        <div class="form__title-left">
          <div class="form__title-text">
            <span class="text-semibold">Thông tin đặt đồ</span>
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
        <div class="form__dish">
          <div class="dish-search"></div>
          <div class="dish__list-all">
            <div class="dish-type">
              <div
                v-for="(typeStr, key) in Resource.DishType"
                :key="key"
                @click="currentDishType = Enumeration.DishType[key]"
                class="type-text"
                :class="{
                  active: currentDishType == Enumeration.DishType[key],
                }"
              >
                <span>{{ typeStr }}</span>
              </div>
            </div>
            <div class="dish__list-type">
              <div
                class="dish-item"
                v-for="(dish, index) in groupDish[currentDishType]"
                :key="index"
                @click="addDish(dish)"
              >
                <img
                  :src="'data:image/jpeg;base64,' + dish.dish_img"
                  width="100"
                  height="100"
                  min-width="100"
                  min-height="100"
                />
                <div class="dish-name">{{ dish.dish_name }}</div>
                <div class="dish-price">
                  {{
                    CommonFn.convertOriginData(
                      dish.price,
                      Resource.DataTypeColumn.Number,
                      null
                    )
                  }}
                  VND
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="form__order">
          <div class="form__customer pt-1 pl-1">
            <div class="form-group">
              <div class="form-row">
                <div class="form-item">
                  <FieldInputLabel
                    MustValidate="true"
                    :saveValidate="true"
                    :customData="dataForm.customerNameInput"
                    :model="dataForm.orderData.customer_name"
                    @invalidData="invalidData"
                    @updateValueInput="updateValueInput"
                    @getOriginData="getOriginData"
                    :ref="inputFieldName"
                  />
                </div>
                <div class="form-item">
                  <FieldInputLabel
                    MustValidate="true"
                    :saveValidate="true"
                    :customData="dataForm.customerPhoneInput"
                    :model="dataForm.orderData.customer_phone"
                    @invalidData="invalidData"
                    @updateValueInput="updateValueInput"
                    @getOriginData="getOriginData"
                    :ref="inputFieldName"
                  />
                </div>
              </div>
            </div>
          </div>
          <div class="form__dish-order">
            <table class="pl-1">
              <tr>
                <th class="text-left pl-1">Tên món</th>
                <th class="text-center">Số lượng</th>
                <th class="text-right">Thành tiền</th>
                <th></th>
              </tr>
              <tr
                class="body"
                v-for="(dishOrder, index) in dataForm.orderData.dishes"
                :key="index"
              >
                <td class="dish__name pl-1">{{ dishOrder.dish_name }}</td>
                <td>
                  <div class="dish__quantity">
                    <div
                      class="quantity-change"
                      @click="
                        dishOrder.quantity == 1
                          ? removeDishItem(dishOrder)
                          : dishOrder.quantity--
                      "
                    >
                      -
                    </div>
                    <span>{{ dishOrder.quantity }}</span>
                    <div class="quantity-change" @click="dishOrder.quantity++">
                      +
                    </div>
                  </div>
                </td>
                <td class="text-right">
                  {{
                    CommonFn.convertOriginData(
                      dishOrder.quantity * dishOrder.price,
                      Resource.DataTypeColumn.Number
                    )
                  }}
                </td>
                <td class="pr-1">
                  <div class="dish__delete" @click="removeDishItem(dishOrder)">
                    <div class="dish__delete-icon"></div>
                  </div>
                </td>
              </tr>
              <tr
                class="row-summary"
                v-if="dataForm.orderData.dishes.length > 0"
              >
                <td class="text-bold pl-1">Tổng:</td>
                <td></td>
                <td class="text-right text-bold">
                  {{
                    CommonFn.convertOriginData(
                      summaryMoney,
                      Resource.DataTypeColumn.Number
                    )
                  }}
                </td>
                <td></td>
              </tr>
            </table>
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
            class="btn btn-primary text-semibold"
            tabindex="0"
            @click="orderDish"
            @keyup.enter="orderDish"
          >
            Đặt món
          </div>
          <div
            class="btn btn-primary text-semibold"
            tabindex="0"
            @click="paidDish"
            @keyup.enter="paidDish"
          >
            Thanh toán
          </div>
        </div>
      </div>
    </div>
  </form>

  <restaurant-sell-invoice-vue ref="Invoice" ></restaurant-sell-invoice-vue>
</template>

<script>
import { reactive, ref, getCurrentInstance, computed } from "vue";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import { useRestaurantOrderFormData } from "./RestaurantOrderFormData";
import Tooltip from "../../../components/Tooltip.vue";
import BasePopup from "../../../components/BasePopup.vue";
import Combobox from "../../../components/Combobox.vue";
import DishAPI from "../../../api/views/setup/DishAPI";
import OrderAPI from "../../../api/views/sell/OrderAPI";
import RestaurantSellInvoiceVue from "./RestaurantSellInvoice.vue";

export default {
  components: { Tooltip, BasePopup, Combobox, RestaurantSellInvoiceVue },

  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const { dataForm, dishGrid } = useRestaurantOrderFormData();
    const groupDish = reactive({});
    const currentDishType = ref(1);
    const summaryMoney = computed(() => {
      let res = 0;
      if (dataForm.orderData.dishes.length > 0) {
        res = dataForm.orderData.dishes.reduce(
          (sum, x) => (sum += x.price * x.quantity),
          res
        );
      }
      return res;
    });

    /**
     * Lấy món ăn
     */
    function getDishes() {
      proxy.$store.commit("SHOW_LOADER", true);
      DishAPI.getAll()
        .then((res) => {
          if (res && res.data.length > 0) {
            // phân loại món ăn ra
            let data = res.data
              .sort((a, b) => parseFloat(a.dish_type) - parseFloat(b.dish_type))
              .reduce((acc, cur) => {
                acc[cur.dish_type] ??= [];
                acc[cur.dish_type].push(cur);
                return acc;
              }, {});

            Object.assign(groupDish, data);
          }
        })
        .catch(() => {
          proxy.$store.commit("SHOW_LOADER", false);
        });
      proxy.$store.commit("SHOW_LOADER", false);
    }

    function openForm(table) {
      proxy.getDishes();
      dataForm.orderData.table_id = table.table_id;
      
      dataForm.showForm = true;
    }

    function closeForm() {
      dataForm.showForm = false;
    }

    /**
     * Order món ăn
     * @param {*} dish
     */
    function addDish(dish) {
      let item = dataForm.orderData.dishes.find(
        (x) => x.dish_id == dish.dish_id
      );
      if (item) {
        item.quantity++;
      } else {
        dish.quantity = 1;
        dataForm.orderData.dishes.push(dish);
      }
    }

    /**
     * Bỏ chọn món ăn
     * @param {} dish
     */
    function removeDishItem(dish) {
      dataForm.orderData.dishes = dataForm.orderData.dishes.filter(
        (x) => x.dish_id != dish.dish_id
      );
    }

    /**
     * Đặt món
     */
    function orderDish() {
      proxy.$refs.Invoice.openForm();

      // OrderAPI.insert(dataForm.orderData)
      //   .then((response) => {
      //     if (response.data.code == 200) {
      //       //Hiển thị toast message
      //       proxy.$store.commit("SHOW_TOAST", {
      //         toastType: Resource.Toast.Success,
      //         toastMessage: Resource.Message.AddSuccess,
      //       });
      //     }

      //     dataForm.tableData = {};
      //     proxy.$emit("refreshData");
      //   })
      //   .catch((e) => {
      //     proxy.$store.commit("SHOW_LOADER", false);

      //     //Nếu không lưu được thì thông báo lỗi
      //     dataForm.allInputValid = false;
      //     dataForm.errorMessage = e.response.data.data.status[0]
      //       ? e.response.data.data.status[0]
      //       : Resource.Message.ServerError;
      //     dataForm.showErrorPopup = true;
      //   });
    }

    return {
      Enumeration,
      Resource,
      CommonFn,
      dataForm,
      dishGrid,
      openForm,
      closeForm,
      getDishes,
      groupDish,
      currentDishType,
      addDish,
      removeDishItem,
      summaryMoney,
      orderDish,
    };
  },
};
</script>

<style lang="scss">
@import "../../../assets/css/common/form.scss";
@import "./RestaurantOrderForm.scss";
</style>