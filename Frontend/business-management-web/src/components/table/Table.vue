<template>
  <div
    class="grid"
    ref="Grid"
    :style="{ height: customData.gridHeight, width: customData.width }"
  >
    <div class="grid__ulti">
      <div
        class="search-box"
        :class="{ 'search-box-active': this.searchFocused }"
      >
        <input
          type="text"
          class="field-input__input"
          :placeholder="customData.searchPlaceholder"
          v-model="searchGrid"
          @focus="this.searchFocused = true"
          @blur="this.searchFocused = false"
          @keypress.enter="filterDataTable"
        />
        <div class="page-icon">
          <div class="util__icon-search"></div>
        </div>
      </div>
      <div class="util__right">
        <div class="btn-refresh page-icon" @click="refreshData">
          <div title="Làm mới" class="util__icon-refresh"></div>
        </div>
        <div
          title="Xoá bản ghi"
          class="page-icon ml-2"
          @click="deleteSelectedRows"
        >
          <svg
            width="20px"
            height="20px"
            opacity="0.7"
            aria-hidden="true"
            focusable="false"
            data-prefix="fas"
            data-icon="trash-alt"
            class="svg-inline--fa fa-trash-alt fa-w-14"
            role="img"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 448 512"
          >
            <path
              fill="grey"
              d="M32 464a48 48 0 0 0 48 48h288a48 48 0 0 0 48-48V128H32zm272-256a16 16 0 0 1 32 0v224a16 16 0 0 1-32 0zm-96 0a16 16 0 0 1 32 0v224a16 16 0 0 1-32 0zm-96 0a16 16 0 0 1 32 0v224a16 16 0 0 1-32 0zM432 32H312l-9.4-18.7A24 24 0 0 0 281.1 0H166.8a23.72 23.72 0 0 0-21.4 13.3L136 32H16A16 16 0 0 0 0 48v32a16 16 0 0 0 16 16h416a16 16 0 0 0 16-16V48a16 16 0 0 0-16-16z"
            ></path>
          </svg>
        </div>
        <div class="btn-export page-icon ml-2" @click="exportData">
          <div title="Xuất khẩu" class="util__icon-export"></div>
        </div>
      </div>
    </div>
    <div class="table" :style="{ height: customData.tableHeight }">
      <table>
        <thead>
          <tr>
            <td
              v-for="(item, index) in customData.column"
              class="noselect"
              :class="{
                'text-right': item.dataType == Resource.DataTypeColumn.Number,
                'text-center':
                  item.dataType == Resource.DataTypeColumn.Date ||
                  item.dataType == Resource.DataTypeColumn.OrderNumber,
                'col-function text-center': item.functionColumn,
                'col-selectBox': item.selectBoxColumn,
              }"
              :key="index"
            >
              <div
                class="select-box"
                v-show="item.selectBoxColumn"
                @click="toggleSelectAllRow"
                :class="{
                  'rotate-90 box-selected':
                    customData.gridData &&
                    currentSelectedRows.length == customData.gridData.length,
                }"
              >
                <div
                  class="table__icon-row-select"
                  v-show="
                    customData.gridData &&
                    currentSelectedRows.length == customData.gridData.length
                  "
                ></div>
              </div>
              {{ item.columnName }}
            </td>
          </tr>
        </thead>
        <tbody ref="tbody">
          <tr
            tabindex="0"
            :ref="index"
            v-for="(item, index) in customData.gridData"
            :ItemId="item[customData.idFieldName]"
            :key="index"
            @mouseenter.exact="rowHover"
            @mouseleave="rowUnhover"
            @click.exact="clickRow(index)"
            @click.ctrl="ctrlClickRow(index)"
            @click.shift="shiftClickRow(index)"
            @dblclick="dbClickRow(item)"
            @keydown.up="pressUpArrowKey(index)"
            @keydown.down="pressDownArrowKey(index)"
            @contextmenu.prevent="rightClickRow($event, index)"
          >
            <td
              v-for="(col, index) in customData.column"
              class="noselect"
              :class="{
                'text-right': col.dataType == Resource.DataTypeColumn.Number,
                'text-center':
                  col.dataType == Resource.DataTypeColumn.Date ||
                  col.dataType == Resource.DataTypeColumn.OrderNumber,
                'col-function': col.functionColumn,
                'col-selectBox': col.selectBoxColumn,
                'tr-selected': checkRowSelected(
                  customData.gridData.indexOf(item)
                ),
              }"
              :style="[
                col.functionColumn
                  ? {
                      'z-index': dropup
                        ? customData.gridData.indexOf(item)
                        : customData.gridData.length -
                          customData.gridData.indexOf(item),
                    }
                  : {},
              ]"
              :key="index"
            >
              <!-- Nếu là ô chọn nhiều -->
              <div
                class="select-box"
                :class="{
                  'rotate-90 box-selected': checkRowSelected(
                    customData.gridData.indexOf(item)
                  ),
                }"
                v-show="col.selectBoxColumn"
                @click.stop="selectBoxClick(customData.gridData.indexOf(item))"
                @dblclick.prevent.stop
              >
                <div
                  class="table__icon-row-select"
                  v-show="
                    currentSelectedRows.includes(
                      customData.gridData.indexOf(item)
                    )
                  "
                ></div>
              </div>
              <!-- Nếu là ô function -->
              <div class="function-cell" v-show="col.functionColumn">
                <span
                  class="text-semibold"
                  @click.stop
                  @click="clickDefaultFunction(item)"
                  >{{ customData.defaultFunction }}</span
                >
                <div
                  class="function-cel__dropdown"
                  tabindex="0"
                  @blur="closeDropdown"
                >
                  <div
                    class="page-icon"
                    :class="{
                      'dropdown-icon-select':
                        currentDropdown == customData.gridData.indexOf(item),
                    }"
                    @click.stop="toggleDropdownFunction($event, item)"
                    @dblclick.prevent.stop
                  >
                    <div class="table__icon-dropdown"></div>
                  </div>
                  <transition name="slide-fade">
                    <div
                      ref="Dropdown"
                      class="dropdown-function"
                      :style="[
                        dropup
                          ? { top: '-' + (dropdownHeight + 6) + 'px' }
                          : {},
                        dropdownRightPosition
                          ? { right: dropdownRightPosition + 'px' }
                          : {},
                      ]"
                      v-if="
                        currentDropdown == customData.gridData.indexOf(item)
                      "
                    >
                      <div
                        class="dropdown-function__item"
                        v-for="(fn, index) in customData.functions"
                        :key="index"
                        @click="clickFunctionItem(fn, item)"
                      >
                        {{ fn }}
                      </div>
                    </div>
                  </transition>
                </div>
              </div>
              <input
                v-if="col.editable"
                :ref="col.fieldName"
                class="cell-editable"
                :class="{
                  'cell-editable-invalid':
                    invalidEditable &&
                    (!item[col.fieldName] || item[col.fieldName].length == 0),
                }"
                @click.stop
                @focus="focusEditableCell($event)"
                @keydown="keyDownOnEditableCell($event, col.dataType)"
                @keyup="
                  onInputEditableCell(
                    col.dataType,
                    col.fieldName,
                    item[col.fieldName],
                    customData.gridData.indexOf(item)
                  )
                "
                v-model="item[col.fieldName]"
                v-mask="money"
              />
              <div v-else-if="col.imgColumn" class="img-cell">
                <img
                  :src="'data:image/jpeg;base64,' + item[col.fieldName]"
                  alt=""
                  width="40"
                  height="40"
                  min-width="40"
                  min-height="40"
                />
              </div>
              <span v-else>
                {{
                  col.dataType == Resource.DataTypeColumn.OrderNumber
                    ? customData.gridData.indexOf(item) + 1
                    : getDisplayValue(
                        item[col.fieldName],
                        col.dataType,
                        col.enumName
                      )
                }}
              </span>
            </td>
          </tr>
          <tr
            v-show="
              customData.showSummaryRow &&
              customData.gridData &&
              customData.gridData.length > 0
            "
            class="row-summary"
          >
            <td
              v-for="(item, index) in customData.column"
              :key="index"
              class="no-select text-right"
            >
              <span v-show="item.dataType == Resource.DataTypeColumn.Number">{{
                getSumaryValue(item.fieldName)
              }}</span>
            </td>
          </tr>
        </tbody>
      </table>
      <div
        class="data-empty"
        v-if="!customData.gridData || customData.gridData.length == 0"
      >
        <div class="data-empty__content">
          <img src="../../assets/images/eport_nodata.76e50bd8.svg" alt="" />
          <span>Không có dữ liệu</span>
        </div>
      </div>
    </div>
    <Paging
      v-show="customData.showPaging"
      :customData="paging"
      @clickPageNum="clickPageNum"
      @changePageSize="changePageSize"
      ref="Paging"
    />
  </div>
</template>

<script>
import CommonFn from "../../js/common/CommonFn";
import Paging from "./Paging.vue";
import createNumberMask from "text-mask-addons/dist/createNumberMask";
import Resource from "../../js/common/Resource";
import Tooltip from "../Tooltip.vue";

const currencyMask = createNumberMask({
  prefix: "",
  allowDecimal: true,
  includeThousandsSeparator: true,
  allowNegative: false,
});

export default {
  components: {
    Paging,
    Tooltip,
  },
  props: {
    customData: {
      type: Object,
      required: true,
    },
    saveValidate: {
      type: Boolean,
      default: false,
    },
  },
  mounted() {
    this.displayDropdown = true;
  },
  data() {
    return {
      Resource: Resource,
      searchGrid: null,
      searchFocused: false,
      refreshFocused: false,
      exportFocused: false,
      editableDataDisplay: null,
      invalidEditable: false,
      currentSelectedRows: [],
      selectedRow: null,
      countClick: 0,
      timer: null,
      currentDropdown: null,
      dropup: false,
      dropdownHeight: null,
      dropdownRightPosition: 0,
      showEmptyData: false,
      cloneEditCellModel: null,
      paging: {
        pageSize: 1,
        totalRecord: 0,
        totalPage: 1,
        maxPageNumDispaly: 0,
        currentPageNum: 1,
      },
      money: currencyMask,
    };
  },
  created() {
    this.paging.pageSize = this.customData.pageSize;
    this.paging.totalRecord = this.customData.totalRecord;
    this.paging.totalPage = this.customData.totalPage;
    this.paging.maxPageNumDisplay = this.customData.maxPageNumDisplay;
    this.paging.currentPageNum = this.customData.currentPageNum;
  },
  watch: {
    customData: {
      deep: true,
      handler(val) {
        //Dữ liệu paging
        this.paging.pageSize = val.pageSize;
        this.paging.totalRecord = val.totalRecord;
        this.paging.totalPage = val.totalPage;
        this.paging.maxPageNumDisplay = val.maxPageNumDisplay;
        this.paging.currentPageNum = val.currentPageNum;
      },
    },
    currentSelectedRows: {
      deep: true,
      immediate: true,
      handler(val) {
        this.$emit("changeListSelectedRow", val);
      },
    },
    saveValidate(val) {
      if (val) {
        this.invalidEditable = true;
      }
    },
  },
  methods: {
    /**
     * Hàm refresh dữ liệu
     */
    refreshData() {
      this.$emit("refreshData");
    },

    /**
     * Hàm filter dữ liệu trên bảng
     */
    filterDataTable() {
      this.$emit("filterDataTable", searchGrid);
    },

    /**
     * Hàm export dữ liệu sang excel
     */
    exportData() {
      this.$emit("exportData");
    },

    /**
     * Hàm xóa bản ghi đã chọn
     */
    deleteSelectedRows() {
      this.$emit("deleteSelectedRows");
    },

    /**
     * Hàm chọn/bỏ chọn tất cả các row
     */
    toggleSelectAllRow() {
      //Auto chọn bản ghi đầu
      if (this.currentSelectedRows.length != this.customData.gridData.length) {
        this.currentSelectedRows = [
          ...Array(this.customData.gridData.length).keys(),
        ];
      } else {
        this.currentSelectedRows = [];
      }
    },
    /**
     * Hàm xử lý hover chuột vào row
     */
    rowHover(e) {
      e.target.classList.add("tr-hover");
    },

    /**
     * Hàm xử lý bỏ hover chuột vào row
     */
    rowUnhover(e) {
      e.target.classList.remove("tr-hover");
    },

    /**
     * Hàm xử lý click chọn một row
     */
    clickRow(index) {
      //Reset list
      this.currentSelectedRows = [];

      //Thêm row được chọn vào list
      this.currentSelectedRows.push(index);

      //focus vào ô được chọn
      // this.$refs[index][0]?.focus();
    },

    /**
     * Hàm xử lý khi ctrl click vào row
     */
    ctrlClickRow(index) {
      //Thêm row được chọn vào list
      if (!this.currentSelectedRows.includes(index)) {
        this.currentSelectedRows.push(index);
      } else {
        let idex = this.currentSelectedRows.indexOf(index);
        this.currentSelectedRows.splice(idex, 1);
      }

      //focus vào ô được chọn
      // this.$refs[index][0].focus();
    },

    /**
     * Hàm xử lý shift click row
     */
    shiftClickRow(index) {
      let len = this.currentSelectedRows.length,
        lastItem = this.currentSelectedRows[len - 1];

      //Nếu chưa click dòng nào thì click
      if (!len) {
        this.clickRow(index);
      } else {
        if (index > lastItem) {
          this.currentSelectedRows = Array.from(
            { length: index - lastItem + 1 },
            (_, i) => i + lastItem
          ).reverse();
        } else if (index < lastItem) {
          this.currentSelectedRows = Array.from(
            { length: lastItem - index + 1 },
            (_, i) => i + index
          );
        }
      }
    },

    /**
     * Hàm xử lý click chuột vào row
     */
    dbClickRow(item) {
      this.$emit("dbClickRow", item);
    },

    /**
     * Hàm xử lý chọn row khi ấn mũi tên lên
     */
    pressUpArrowKey(index) {
      if (index > 0) {
        this.clickRow(index - 1);
      }
    },

    /**
     * Hàm xử lý chọn row khi ấn mũi tên xuống
     */
    pressDownArrowKey(index) {
      if (index < this.customData.gridData.length - 1) {
        this.clickRow(index + 1);
      }
    },

    /**
     * Hảm chọn nhiều khi chạy chuột và nhấn shift
     */
    mouseEnterWhenShiftPressed(index) {
      if (!this.currentSelectedRows.includes(index)) {
        this.ctrlClickRow(index);
      }
    },

    /**
     * Hàm hiện context menu
     */
    rightClickRow(event, index) {
      this.clickRow(index);

      //Hiển thị context menu
      this.currentDropdown = index;

      //vị trí ấn hiện tại
      var currentClickPosition = event.target.getBoundingClientRect();

      //Lấy ra function cell
      var listCell = event.target.closest("tr").querySelectorAll("td");
      var functonCell = listCell[listCell.length - 1];

      functonCell.querySelector(".function-cel__dropdown").focus();

      //Xét vị trí context menu
      setTimeout(() => {
        //Thông tin của dropdown
        let dropdownBound = functonCell
          .querySelector(".dropdown-function")
          .getBoundingClientRect();

        //Chiều cao của dropdown
        this.dropdownHeight = dropdownBound.height;
        this.dropdownRightPosition =
          window.innerWidth - event.x - dropdownBound.width;

        //Đểm cuối của table
        var tableHeight =
          this.$refs.Grid.querySelector(".table").getBoundingClientRect()
            .bottom;

        //Nếu vị trí ấn hiện tại + height dropdown vượt ra khỏi table => dropdown ngược lên
        if (currentClickPosition.y + this.dropdownHeight > tableHeight) {
          this.dropup = true;
        }
      }, 0.2);
    },

    /**
     * Hàm xử lý click chuột vào ô select
     */
    selectBoxClick(index) {
      if (this.currentSelectedRows.includes(index)) {
        this.currentSelectedRows.splice(
          this.currentSelectedRows.indexOf(index),
          1
        );
      } else {
        //Không thì thêm vào list đang được chọn
        this.currentSelectedRows.push(index);
      }
    },

    /**
     * Hàm kiểm tra xem row đã được chọn hay chưa
     */
    checkRowSelected(index) {
      return this.currentSelectedRows.includes(index);
    },

    /**
     * Hàm chuyển đổi dữ liệu để hiển thị lên bảng
     */
    getDisplayValue(data, dataType, enumName) {
      return CommonFn.convertOriginData(data, dataType, enumName);
    },

    /**
     * Hàm lấy ra số lượng item đang được chọn để hiện lên thông báo
     */
    getNumberSelectedItem() {
      return this.currentSelectedRows.length;
    },

    /**
     * Mở dropdown function
     */
    toggleDropdownFunction(event, item) {
      if (this.currentDropdown == null) {
        this.currentDropdown = this.customData.gridData.indexOf(item);
      } else {
        this.currentDropdown = null;
      }

      //vị trí ấn hiện tại
      var currentClickPosition = event.target.getBoundingClientRect();

      var cellFunction = event.target.closest(".function-cel__dropdown");

      //Hiển thị dropdown function
      setTimeout(() => {
        cellFunction.focus();

        //Chiều cao của dropdown
        this.dropdownHeight = cellFunction
          .querySelector(".dropdown-function")
          .getBoundingClientRect().height;

        //Đểm cuối của table
        var tableHeight =
          this.$refs.Grid.querySelector(".table").getBoundingClientRect()
            .bottom;

        //Nếu vị trí ấn hiện tại + height dropdown vượt ra khỏi table => dropdown ngược lên
        if (currentClickPosition.y + this.dropdownHeight > tableHeight) {
          this.dropup = true;
        }
      }, 0.2);
    },

    /**
     * Hàm đóng dropdown function
     */
    closeDropdown() {
      this.currentDropdown = null;
      this.dropup = false;
      this.dropdownRightPosition = 0;
    },

    /**
     * Gọi cha để thông báo người dùng click vào default function
     */
    async clickDefaultFunction(item) {
      await this.clickRow(this.customData.gridData.indexOf(item));

      this.$emit("clickDefaultFunction", item);
    },

    /**
     * Gọi cha để thông báo người dùng click vào một item function
     */
    clickFunctionItem(fn, item) {
      this.$emit("clickFunctionItem", fn, item);
    },

    /**
     * Hàm gọi cha để chuyển trang được click
     */
    clickPageNum(pageNum) {
      this.$emit("clickPageNum", pageNum);
    },

    /**
     * Hàm lấy id của những row đang được chọn
     */
    getListIdSelectedItem() {
      let listId = [],
        rows = this.$refs.tbody.querySelectorAll(".tr-selected");

      //Lấy tất cả id của row đang được chọn
      for (let i = 0; i < rows.length; i++) {
        listId.push(rows[i].getAttribute("ItemId"));
      }

      return listId;
    },

    /**
     * Hàm thực hiện reset list item đang được chọn về focus vào dòng đầu tiên
     */
    resetCurrentSelectedRows() {
      //Nếu có dữ liệu thì mới auto click
      if (this.customData.gridData && this.customData.gridData.length > 0) {
        if (this.currentSelectedRows.length == 1) {
          this.clickRow(this.currentSelectedRows[0]);
        } else {
          this.clickRow(0);
        }
      }
    },

    /**
     * Hàm gọi cha thay đổi page size
     */
    changePageSize(pageSize) {
      this.$emit("changePageSize", pageSize);
    },

    /**
     * Hàm xử lý sự kiện focus vào ô cho phép nhập
     */
    focusEditableCell(event) {
      event.target.select();
    },

    /**
     * Hàm xử lý có cho nhập hay không tuỳ vào kiểu dữ liệu
     */
    keyDownOnEditableCell(event, dataType) {
      switch (dataType) {
        //Nếu là dạng số
        case Resource.DataTypeColumn.Number:
          //Nếu phím nhập vào không phải số thì không cho nhập
          if (
            !isFinite(event.key) &&
            event.keyCode != 8 &&
            event.keyCode != 190 &&
            event.keyCode != 9 &&
            event.keyCode != 37 &&
            event.keyCode != 39
          ) {
            event.preventDefault();
          }

          break;
      }
    },

    /**
     * Hàm xử lý khi người dùng sửa dữ liệu ở ô cho phép sửa
     */
    onInputEditableCell(dataType, key, value, index) {
      //Thông báo ra ngoài
      this.$emit("changeValueEditableCell", index, dataType);
    },

    /**
     * Hàm lấy giá trị tổng của cột
     */
    getSumaryValue(fieldName) {
      if (this.customData.gridData && this.customData.gridData.length > 0) {
        let sum = this.customData.gridData.reduce(
          (a, b) => a + (b[fieldName] || 0),
          0
        );

        sum = Math.round(sum * 100) / 100;

        return CommonFn.convertOriginData(sum, Resource.DataTypeColumn.Number);
      }
    },
  },
};
</script>

<style lang="scss" scoped>
@import url("../../assets/css/components/table/table.scss");
.slide-fade-enter-active {
  transition: transform 0.2s ease;
}
.slide-fade-leave-active {
  transition: transform 0s cubic-bezier(1, 0.5, 0.8, 1);
}
.slide-fade-enter,
.slide-fade-leave-to {
  transform: translateY(10px);
  opacity: 0;
}
</style>