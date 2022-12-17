import axios from "axios";
import BaseAPI from "../../base/BaseAPI";
import APIConfig from "../../config/APIConfig"

class DishAPI extends BaseAPI {
    constructor() {
        super();

        this.controller = "api/v1/Dish";
    }

    /**
     *  override Hàm thêm mới dữ liệu
     * @param {*} data 
     * @returns 
     */
    insert(data) {
        return axios.post(APIConfig + `${this.controller}`, data);
    }

}

export default new DishAPI();