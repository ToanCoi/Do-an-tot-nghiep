<template>
  <form v-if="showForm">
    <div class="form invoice__form">
      <div class="form__title">
        <div class="form__title-left">
          <div class="form__title-text">
            <span class="text-semibold">Thông tin hoá đơn</span>
          </div>
        </div>
        <div class="form__title-right d-flex">
          <Tooltip :customData="'Trợ giúp'">
            <div class="page-icon">
              <div class="form__icon-help"></div>
            </div>
          </Tooltip>
          <Tooltip :customData="'Đóng'">
            <div class="page-icon" @click="backToOrder()">
              <div class="form__icon-cancel"></div>
            </div>
          </Tooltip>
        </div>
      </div>
      <div class="form__content" id="print__content" ref="FormData">
        <div class="restaurant-name text-bold mb-1">Nhà hàng NVTOAN</div>
        <div class="text-bold invoice-title mb-3">HOÁ ĐƠN THANH TOÁN</div>
        <div class="invoice__detail">
          <div class="mb-1">
            Khách hàng:
            <span class="text-bold">{{ orderData?.customer_name }}</span>
          </div>
          <div class="mb-2">
            Thu Ngân:
            <span class="text-bold">{{ orderData.cashier?.user_name }}</span>
          </div>
          <div class="form__dish-order mb-2">
            <table>
              <tr>
                <th class="text-left">Tên món</th>
                <th class="text-center pl-2">Số lượng</th>
                <th class="text-center pl-2">Đơn giá</th>
                <th class="text-right pl-2">Thành tiền</th>
              </tr>
              <tr
                class="body"
                v-for="(dishOrder, index) in orderData.dishes"
                :key="index"
              >
                <td class="dish__name">{{ dishOrder.dish_name }}</td>
                <td class="pl-2">
                  <div class="dish__quantity">
                    <span>{{ dishOrder.quantity }}</span>
                  </div>
                </td>
                <td class="text-right pl-2">
                  {{
                    CommonFn.convertOriginData(
                      dishOrder.price,
                      Resource.DataTypeColumn.Number
                    )
                  }}
                </td>
                <td class="text-right pl-2">
                  {{
                    CommonFn.convertOriginData(
                      dishOrder.quantity * dishOrder.price,
                      Resource.DataTypeColumn.Number
                    )
                  }}
                </td>
              </tr>
              <tr class="row-summary" v-if="orderData.dishes?.length > 0">
                <td class="text-bold pl-1">Tổng:</td>
                <td></td>
                <td></td>
                <td class="text-right text-bold">
                  {{
                    CommonFn.convertOriginData(
                      summaryMoney,
                      Resource.DataTypeColumn.Number
                    )
                  }}
                </td>
              </tr>
            </table>
          </div>
        </div>
      </div>
      <div class="form__footer">
        <div></div>
        <div class="btn-group d-flex">
          <div
            class="btn btn-primary text-semibold"
            tabindex="0"
            @click="print"
            @keyup.enter="print"
          >
            In
          </div>
          <div
            class="btn btn-primary text-semibold"
            tabindex="0"
            @click="createInvoice"
            @keyup.enter="createInvoice"
          >
            Hoàn thành
          </div>
        </div>
      </div>
    </div>
  </form>
</template>

<script>
import { reactive, ref, getCurrentInstance, computed } from "vue";
import Enumeration from "../../../js/common/Enumeration.js";
import Resource from "../../../js/common/Resource.js";
import CommonFn from "../../../js/common/CommonFn.js";
import InvoiceAPI from "../../../api/views/sell/InvoiceAPI";

export default {
  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const showForm = ref(false);
    const orderData = reactive({});

    const summaryMoney = computed(() => {
      let res = 0;
      if (orderData.dishes?.length > 0) {
        res = orderData.dishes?.reduce(
          (sum, x) => (sum += x.price * x.quantity),
          res
        );
      }
      return res;
    });

    function openForm(order) {
      Object.assign(orderData, order);

      orderData.cashier = proxy.$store.getters.user;
      showForm.value = true;
    }

    function print() {
      window.print();
    }

    async function createInvoice() {
      let invoiceData = {
        employee_id: orderData.cashier?.user_id,
        customer_name: orderData.customer_name,
        customer_phone: orderData.customer_phone,
        order_id: orderData.order_id,
        total: summaryMoney.value,
      };

      await InvoiceAPI.insertInvoice(invoiceData)
        .then((response) => {
          proxy.$store.commit("SHOW_LOADER", false);

          if (response.data.code == 200) {
            //Hiển thị toast message
            proxy.$store.commit("SHOW_TOAST", {
              toastType: Resource.Toast.Success,
              toastMessage: Resource.Message.AddSuccess,
            });
          }

          proxy.closeForm();
        })
        .catch((e) => {
          proxy.$store.commit("SHOW_LOADER", false);

          //Nếu không lưu được thì thông báo lỗi
          // dataForm.allInputValid = false;
          // dataForm.errorMessage = e.response.data.data.status[0]
          //   ? e.response.data.data.status[0]
          //   : Resource.Message.ServerError;
          // dataForm.showErrorPopup = true;
        });
    }

    function closeForm() {
      proxy.$emit("refreshTables");
      proxy.$emit("refreshData");
      showForm.value = false;
    }

    function backToOrder() {
      proxy.$emit("openOrderForm");

      showForm.value = false;
    }

    return {
      Enumeration,
      Resource,
      CommonFn,
      orderData,
      showForm,
      openForm,
      print,
      closeForm,
      summaryMoney,
      backToOrder,
      createInvoice,
    };
  },
};
</script>


<style lang="scss">
@import "../../../assets/css/common/form.scss";
@import "./RestaurantSellInvoice.scss";
</style>