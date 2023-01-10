import axios from "axios";
import BusinessAPI from "../BusinessAPI";

class StatisticAPI extends BusinessAPI {
    constructor() {
        super();

        this.controller = "api/v1/Statistics";
    }

    //Lấy thống kê
    getStatistic(type) {
        this.getBaseURL();
        return axios.get(`${this.baseURL}/statistic/${type}`);
        // axios.post(`${this.baseURL}`, startDate, endDate);
    }
}

export default new StatisticAPI();