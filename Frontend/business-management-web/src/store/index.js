import { createStore } from 'vuex'

export const store = createStore({
  state: {
    //Dữ liệu custom toast message
    toastData: {},

    //Hiển thị loader
    showLoader: false,

    //Hiển thị popup lỗi phía server
    errorPopupData: {},

    //Năm cần xem dữ liệu
    dataYear: null,

    //token
    token: null,

    //user
    user: null
  },
  mutations: {
    /**
     * Hiển thị toast message
     * @param {*} state 
     * @param {*} toastData 
     */
    SHOW_TOAST(state, toastData) {

      state.toastData = JSON.parse(JSON.stringify(toastData));
    },
    
    /**
     * Hiển thị loader
     * @param {*} state 
     * @param {boolean} showLoader
     */
    SHOW_LOADER(state, showLoader) {
      state.showLoader = showLoader;
    },

    /**
     * Hiển thị popup lỗi phía server
     * @param {*} state 
     * @param {*} serverErrorMessage Tin nhắn lỗi
     */
    SHOW_ERROR_POPUP(state, errorPopupData) {
      state.errorPopupData = JSON.parse(JSON.stringify(errorPopupData));
    },

    /**
     * Thay đổi năm xem dữ liệu
     * @param {*} state 
     * @param {*} year 
     */
    CHANGE_YEAR(state, year) {
      state.dataYear = year;
    },
    
    changeToken(state, token) {
      localStorage.setItem('token', JSON.stringify(token));
      state.token = token;
    },

    changeUser(state, user) {
      localStorage.setItem('user', JSON.stringify(user));
      state.user = user
    },
  },
  getters: {
    //Dữ liệu custom toast message
    toastData: state => {
      return state.toastData;
    },

    //loader
    showLoader: state => {
      return state.showLoader;
    },

    //Popup lỗi server
    errorPopupData: state => {
      return state.errorPopupData;
    },
    
    //Năm cần xem dữ liệu
    dataYear: state => {
      return state.dataYear;
    },

    token: state => {
      return state.token ?? localStorage.getItem('token');
    },

    user: state => {
      return state.user ?? JSON.parse(localStorage.getItem('user'));
    }
  },
});