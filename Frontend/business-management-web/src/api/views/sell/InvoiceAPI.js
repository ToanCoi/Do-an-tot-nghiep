import BusinessAPI from "../BusinessAPI";
import axios from "axios";

class InvoiceAPI extends BusinessAPI {
    constructor() {
        super();

        this.controller = "api/v1/Invoices";
    }

    /**
     * Thêm mới invoice
     * @param {*} data 
     * @returns 
     */
    async insertInvoice(data) {
        this.getBaseURL();
        return await axios.post(`${this.baseURL}/invoice`, data);
    }
}

export default new InvoiceAPI();