<template>
  <div class="pie__chart">
    <canvas id="pie-chart"></canvas>
  </div>
</template>
    
  <script>
import { Chart } from "chart.js/auto";
import { getCurrentInstance, onMounted, reactive } from "@vue/runtime-core";

export default {
  name: "PieChart",
  props: {
    customData: {
      type: Object,
      require: true,
    },
  },
  data() {
    return {
      pieChart: null,
    };
  },
  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const config = reactive({
      type: "pie",
      data: {
        labels: props.customData?.labels,
        datasets: props.customData?.datasets,
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
      },
    });

    function refreshChart() {
      proxy.pieChart?.destroy();
      proxy.pieChart = new Chart(document.getElementById("pie-chart"), config);
      //   proxy.chart.update();
    }

    return {
      refreshChart,
    };
  },
};
</script>