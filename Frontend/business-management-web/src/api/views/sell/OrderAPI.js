import BusinessAPI from "../BusinessAPI";
import axios from "axios";

class OrderAPI extends BusinessAPI {
    constructor() {
        super();

        this.controller = "api/v1/Orders";
    }

    /**
     * Lấy ra bàn trong nhà hàng cùng trạng thái đặt của nó
     */
    getTables() {
        this.getBaseURL();
        return axios.get(`${this.baseURL}/tables`);
    }

    /**
     * Thêm mới order
     * @param {*} data 
     * @returns 
     */
    insertOrder(data) {
        this.getBaseURL();
        return axios.post(`${this.baseURL}/order`, data);
    }

    /**
     * Lấy order theo id
     * @param {*} id 
     * @returns 
     */
    getOrder(id) {
        this.getBaseURL();
        return axios.get(`${this.baseURL}/order/${id}`);
    }

    /**
     * Sửa order
     * @param {*} data 
     * @returns 
     */
    updateOrder(data) {
        this.getBaseURL();
        return axios.put(`${this.baseURL}/order`, data);
    }

}

export default new OrderAPI();