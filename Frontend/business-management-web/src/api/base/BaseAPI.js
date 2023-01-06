import axios from "axios";

export default class BaseAPI {

    constructor() {
        this.controller = null;
        this.apiName = null;
        this.baseURL = null;
    }

    getBaseURL() {
        if(!this.baseURL) {
            this.baseURL = `${window.apis[this.apiName]}${this.controller}`;
        }
    }

    /**
     * Hàm lấy tất cả dữ liệu
     */
    getAll() {
        this.getBaseURL();
        return axios.get(`${this.baseURL}`);
    }

    /**
     * Lấy dữ liệu paging
     * @param {*} payload 
     * @returns 
     */
    getPagingData(payload) {
        this.getBaseURL();
        return axios.post(`${this.baseURL}/list`, payload)
    }

    /**
     * Hàm lấy bản ghi theo Id
     * @param {*} id 
     */
    getById(id) {
        this.getBaseURL();
        return axios.get(`${this.baseURL}/${id}`)
    }

    /**
     * Hàm thêm mới dữ liệu
     * @param {*} data 
     * @returns 
     */
    insert(data) {
        this.getBaseURL();
        return axios.post(`${this.baseURL}`, data);
    }

    /**
     * Hàm sửa dữ liệu
     * @param {*} id 
     * @param {*} data 
     * @returns 
     */
    update(data) {
        this.getBaseURL();
        return axios.put(`${this.baseURL}`, data);
    }

    /**
     * Hàm xóa dữ liệu
     * @param {*} id 
     * @returns 
     */
    delete(id) {
        this.getBaseURL();
        return axios.delete(`${this.baseURL}/${id}`);
    }

    /**
     * Hàm xóa nhiều bản ghi
     * @param {array} listId Danh sách Id bản ghi cần xóa
     * @returns Trạng thái xóa các bản ghi
     */
    multipleDelete(listId) {
        this.getBaseURL();
        return axios.delete(`${this.baseURL}`, { data: listId });
    }

    /**
     * Hàm kiểm tra xem dữ liệu có bị xung đột không để xoá
     * @param {string} id Id bản ghi cần kiểm tra
     * @returns Dữ liệu có bị xung đột không
     */
    checkConflict(id) {
        this.getBaseURL();
        return axios.put(`${this.baseURL}/Conflict/${id}`);
    }
}