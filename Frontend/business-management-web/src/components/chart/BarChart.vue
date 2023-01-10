<template>
  <div class="bar__chart">
    <canvas id="bar-chart"></canvas>
  </div>
</template>
  
<script>
import { Chart } from "chart.js/auto";
import { getCurrentInstance, onMounted, reactive } from "@vue/runtime-core";

export default {
  name: "BarChart",
  props: {
    customData: {
      type: Object,
      require: true,
    },
  },
  data() {
    return {
      chart: null,
    };
  },
  //   mounted() {
  //     this.chart = new Chart(document.getElementById("bar-chart"), config);
  //   },
  setup: (props) => {
    const { proxy } = getCurrentInstance();
    const config = reactive({
      type: "bar",
      data: {
        labels: props.customData?.labels,
        datasets: props.customData?.datasets,
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      },
    });

    // onMounted(() => {
    //   proxy.chart = new Chart(document.getElementById("bar-chart"), config);
    // });

    function refreshChart() {
      proxy.chart?.destroy();
      proxy.chart = new Chart(document.getElementById("bar-chart"), config);
      //   proxy.chart.update();
    }

    return {
      refreshChart,
    };
  },
};
</script>