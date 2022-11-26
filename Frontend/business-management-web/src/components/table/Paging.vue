<template>
  <div class="paging" id="EmployeePaging">
    <div class="paging__left">
      <span class="left__text">
        Tổng số:
        <b>{{ customData.totalRecord }}</b>
        bản ghi
      </span>
    </div>
    <div class="paging__right">
      <Dropdown
        :customData="pagingDropdown"
        :model="clonePageSize"
        @valueChanged="changePageSize"
      />
      <div class="paging__direction">
        <div
          class="paging__direction-item paging-previous"
          :class="{
            'paging-text-disable': currentPageNum == 1,
          }"
          @click="clickPreviousPage"
        >
          Trước
        </div>
        <div
          class="paging__direction-item paging-num"
          :class="{ 'paging-num-selected': currentPageNum == 1 }"
          @click="clickFirstPage"
        >
          <span>1</span>
        </div>
        <div class="paging__direction-extant" v-show="extantLeft">...</div>
        <div
          class="paging__direction-item paging-num"
          v-for="(item, index) in arrPageNumDisplay"
          :key="index"
          :class="{ 'paging-num-selected': currentPageNum == item }"
          @click="clickPageNum(item)"
        >
          <span>{{ item }}</span>
        </div>
        <div class="paging__direction-extant" v-show="extantRight">...</div>
        <div
          v-show="customData.totalPage >= 2"
          class="paging__direction-item paging-num"
          :class="{
            'paging-num-selected': currentPageNum == customData.totalPage,
          }"
          @click="clickLastPage"
        >
          <span>{{ customData.totalPage }}</span>
        </div>
        <div
          class="paging__direction-item paging-next"
          :class="{
            'paging-text-disable': currentPageNum == customData.totalPage,
          }"
          @click="clickNextPage"
        >
          Sau
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  component: {},
  props: {
    customData: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      currentPageNum: 1,
      currentHoverPageNum: null,
      extantLeft: false,
      extantRight: false,
      arrPageNumDisplay: [],
      clonePageSize: 0,
      timeOut: null,
      pagingDropdown: {
        defaultValue: "",
        displayValues: [
          "10 bản ghi trên 1 trang",
          "20 bản ghi trên 1 trang",
          "30 bản ghi trên 1 trang",
          "50 bản ghi trên 1 trang",
          "100 bản ghi trên 1 trang",
        ],
        keys: [10, 20, 30, 50, 100],
        width: "calc(var(--column-width) * 2)",
        height: "32px",
      },
    };
  },
  created() {
    this.clonePageSize = this.customData.pageSize;
    this.currentPageNum = this.customData.currentPageNum;
  },
  watch: {
    customData: {
      deep: true,
      handler(val) {
        this.clonePageSize = val.pageSize;
        this.currentPageNum = val.currentPageNum;

        this.getArrPageNumDisplay();
      },
    },
  },
  methods: {
    /**
     * Hàm khi hover và pageNum
     */
    hoverPageNum(pageIndex) {
      this.currentHoverPageNum = pageIndex;
    },

    /**
     * Hàm khi hover và pageNum
     */
    unhoverPageNum() {
      this.currentHoverPageNum = null;
    },

    /**
     * Hàm lấy ra mảng phần tử hiển thị ở giữa của page-num
     */
    getArrPageNumDisplay() {
      //Nếu tổng số trang > số trang hiển thị thì tạo mảng ở giữa
      if (this.customData.totalPage > this.customData.maxPageNumDisplay) {
        //Tạo mảng hiển thị page-num trong trường hợp tổng số trang > 5
        this.getArrPageNumDisplayMultiple();
      }
      //Không thì tạo mảng từ 1-max
      else {
        //Nếu tổng page >= 3 thì mới cần list ở giữa
        if (this.customData.totalPage >= 3) {
          //Tạo mảng ở giữa
          this.arrPageNumDisplay = Array.from(
            { length: this.customData.totalPage - 2 },
            (_, i) => i + 2
          );
        } else {
          this.arrPageNumDisplay = [];
        }
      }

      //Hiển thị dấu '...' tương ứng
      this.displayExtant();
    },

    /**
     * Hàm tạo mảng hiển thị page-num trong trường hợp tổng số trang > 5
     */
    getArrPageNumDisplayMultiple() {
      //Nếu hiện tại đang chọn page 1
      if (this.currentPageNum == 1) {
        this.arrPageNumDisplay = [2, 3];
      }
      //Nếu hiện tại đang chọn page cuối
      else if (this.currentPageNum == this.customData.totalPage) {
        this.arrPageNumDisplay = [
          this.customData.totalPage - 2,
          this.customData.totalPage - 1,
        ];
      }
      //Nếu đang chọn page ở giữa
      else {
        this.getArrPageNumMiddle();
      }
    },

    /**
     * Hàm lấy page num ở giữa
     */
    getArrPageNumMiddle() {
      //Nếu đang là trang 2 thì chỉ hiển thị 2,3
      if (this.currentPageNum == 2) {
        this.arrPageNumDisplay = [2, 3];
      }
      //Nếu đang là trang gần cuối thì chỉ hiển thị 3 trang cuối
      else if (this.currentPageNum == this.customData.totalPage - 1) {
        this.arrPageNumDisplay = [
          this.customData.totalPage - 2,
          this.customData.totalPage - 1,
        ];
      }
      //Nếu đang là trang ở giữa
      else {
        this.arrPageNumDisplay = [
          this.currentPageNum - 1,
          this.currentPageNum,
          this.currentPageNum + 1,
        ];
      }
    },

    /**
     * Hàm hiển thị dấu còn nữa '...'
     */
    displayExtant() {
      //Dấu còn nữa bên trái
      this.arrPageNumDisplay[0] - 1 > 1
        ? (this.extantLeft = true)
        : (this.extantLeft = false);

      //Dấu còn nữa bên phải
      this.arrPageNumDisplay[this.arrPageNumDisplay.length - 1] + 1 <
      this.customData.totalPage
        ? (this.extantRight = true)
        : (this.extantRight = false);
    },

    /**
     * Hàm khi click page num
     */
    clickPageNum(pageIndex) {
      this.currentPageNum = pageIndex;
      this.getArrPageNumDisplay();

      this.$emit("clickPageNum", pageIndex);
    },

    /**
     * Hàm xử lý khi ấn first page
     */
    clickFirstPage() {
      if (this.currentPageNum != 1) {
        this.currentPageNum = 1;
        this.getArrPageNumDisplay();

        this.$emit("clickPageNum", 1);
      }
    },

    /**
     * Hàm xử lý khi ấn previous page
     */
    clickPreviousPage() {
      if (this.currentPageNum > 1) {
        this.currentPageNum--;
        this.getArrPageNumDisplay();

        this.$emit("clickPageNum", this.currentPageNum);
      }
    },

    /**
     * Hàm xử lý khi ấn next page
     */
    clickNextPage() {
      if (this.currentPageNum < this.customData.totalPage) {
        this.currentPageNum++;
        this.getArrPageNumDisplay();

        this.$emit("clickPageNum", this.currentPageNum);
      }
    },

    /**
     * Hàm xử lý khi ấn last page
     */
    clickLastPage() {
      if (this.currentPageNum != this.customData.totalPage) {
        this.currentPageNum = this.customData.totalPage;
        this.getArrPageNumDisplay();

        this.$emit("clickPageNum", this.currentPageNum);
      }
    },

    /**
     * Hàm thay đổi page size
     */
    changePageSize(pageSize) {
      this.$emit("changePageSize", pageSize);
    },
  },
};
</script>

<style lang="scss" scoped>
@import url("../../assets/css/components/table/paging.css");

.paging-item {
  transition: 0.1s;
}

.disable {
  opacity: 0.4;
  cursor: default;
}

.disable:hover {
  background-color: unset;
}

.disable:active {
  background-color: unset;
}
</style>