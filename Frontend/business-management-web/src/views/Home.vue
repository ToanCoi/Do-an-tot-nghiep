<template>
  <div class="home pl-2">
    <div class="home-label text-bold pt-2">Thống kê tình hình kinh doanh</div>
    <div class="home__statistic-filter pt-3">
      <div class="pr-2">Thống kê theo</div>
      <ComboBox
        tabindex="0"
        style="margin-top: 4px"
        :customData="filterStatistic"
        :model="dataStatistic"
        @valueChanged="updateValueInput"
      />
    </div>
    <div class="home__chart pt-4 pl-2">
      <div class="chart__revenue">
        <BarChart :customData="revenueChartData" ref="BarChart" />
      </div>
      <div class="chart__dish ml-2">
        <PieChart :customData="dishOutStandChartData" ref="PieChart" />
      </div>
    </div>
  </div>
</template>

<script>
import Enumeration from "../js/common/Enumeration.js";
import Resource from "../js/common/Resource.js";
import CommonFn from "../js/common/CommonFn.js";
import { reactive, ref } from "@vue/reactivity";
import BarChart from "../components/chart/BarChart.vue";
import PieChart from "../components/chart/PieChart.vue";
import { getCurrentInstance, onMounted } from "@vue/runtime-core";
import StatisticAPI from "../api/views/statistic/StatisticAPI";

export default {
  components: {
    BarChart,
    PieChart,
  },

  setup: (props) => {
    const { proxy } = getCurrentInstance();

    const firtLoad = ref(true);
    const dataStatistic = ref(1);
    const filterStatistic = reactive({
      inputFieldName: "dish_type",
      labelText: "Đơn vị",
      defaultValue: "",
      displayValues: [
        Resource.TimeStatistic.Month,
        Resource.TimeStatistic.Quarter,
      ],
      keys: [
        Enumeration.TimeStatistic.Month,
        Enumeration.TimeStatistic.Quarter,
      ],
      width: "calc(var(--column-width) * 1)",
      height: "30px",
    });

    onMounted(() => {
      getStatisticData();
    });

    const revenueChartData = reactive({
      labels: ["a", "b", "c"],
      datasets: [
        {
          label: "Doanh thu",
          data: [5, 5, 7],
          borderWidth: 1,
        },
      ],
    });

    const dishOutStandChartData = reactive({
      labels: [],
      datasets: [
        {
          label: "Doanh thu",
          data: [],
          backgroundColor: [],
          hoverOffset: 4,
        },
        {
          label: "Số lượng",
          data: [],
          backgroundColor: [],
          hoverOffset: 4,
        },
      ],
    });

    /**
     * Hàm update dữ liệu khi người dùng thay đổi
     */
    function updateValueInput(key, value) {
      dataStatistic.value = value;

      getStatisticData();
    }

    function getStatisticData() {
      proxy.$store.commit("SHOW_LOADER", true);
      StatisticAPI.getStatistic(dataStatistic.value)
        .then((res) => {
          changeChartData(res.data.data);
          proxy.$store.commit("SHOW_LOADER", false);
        })
        .catch((ex) => {
          proxy.$store.commit("SHOW_LOADER", false);
        });
    }

    function changeChartData(statData) {
      switch (dataStatistic.value) {
        case Enumeration.TimeStatistic.Month:
          Object.assign(revenueChartData.labels, [
            "Tháng 1",
            "Tháng 2",
            "Tháng 3",
            "Tháng 4",
            "Tháng 5",
            "Tháng 6",
            "Tháng 7",
            "Tháng 8",
            "Tháng 9",
            "Tháng 10",
            "Tháng 11",
            "Tháng 12",
          ]);

          revenueChartData.datasets[0].data = [
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          ];

          statData.revenueStat?.forEach((e) => {
            revenueChartData.datasets[0].data[e.month - 1] = e.revenue;
          });

          break;
        case Enumeration.TimeStatistic.Quarter:
          while (revenueChartData.labels.length > 0) {
            revenueChartData.labels.pop();
          }
          Object.assign(revenueChartData.labels, [
            "Quý I",
            "Quý II",
            "Quý III",
            "Quý IV",
          ]);

          revenueChartData.datasets[0].data = [0, 0, 0, 0];

          statData.revenueStat?.forEach((e) => {
            revenueChartData.datasets[0].data[e.quater] = e.revenue;
          });
          break;
      }

      proxy.$refs.BarChart.refreshChart();

      if (firtLoad) {
        statData.dishStat?.forEach((x) => {
          dishOutStandChartData.labels.push(x.dish_name);
          dishOutStandChartData.datasets[0].data.push(x.sum_price);
          dishOutStandChartData.datasets[1].data.push(x.sum_quantity);

          let color = random_rgba();
          dishOutStandChartData.datasets[0].backgroundColor.push(color);
          dishOutStandChartData.datasets[1].backgroundColor.push(color);
        });
        proxy.$refs.PieChart.refreshChart();

        firtLoad.value = false;
      }
    }

    function random_rgba() {
      var o = Math.round,
        r = Math.random,
        s = 255;
      return (
        "rgba(" +
        o(r() * s) +
        "," +
        o(r() * s) +
        "," +
        o(r() * s) +
        "," +
        r().toFixed(1) +
        ")"
      );
    }

    return {
      Enumeration,
      Resource,
      CommonFn,
      filterStatistic,
      dataStatistic,
      revenueChartData,
      updateValueInput,
      getStatisticData,
      changeChartData,
      dishOutStandChartData,
      random_rgba,
      firtLoad,
    };
  },
};
</script>

<style lang="scss">
@import "./Home.scss";
</style>

