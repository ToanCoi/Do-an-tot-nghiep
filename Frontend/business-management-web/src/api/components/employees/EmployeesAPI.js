import BaseAPI from "../../base/BaseAPI";
import BaseAPIConfig from "../../base/BaseAPIConfig";

class EmployeesAPI extends BaseAPI {
    constructor() {
        super();

        this.controller = "api/v1/Employees";
    }

    /**
     * 
     * @param {int} pageSize Số bản ghi / trang
     * @param {int} pageNum 
     * @param {string} filterValue Giá trị cần tìm kiếm
     * @returns 
     */
    filter(pageSize, pageNum, filterValue) {
        let queryString = `${this.controller}/Filter?pageSize=${pageSize}&pageNum=${pageNum}${filterValue ? ('&filterValue=' + filterValue) : ''}`;

        return BaseAPIConfig.get(`${queryString}`);
    }

    /**
     * Hàm lấy mã nhân viên mới
     * NVTOAN 05/07/2021
     * @returns 
     */
    getNewEmployeeCode() {
        return BaseAPIConfig.get(`${this.controller}/NewEmployeeCode`);
    }

    /**
     * Hàm lấy bản ghi theo property
     * @param {string} propName Tên property cần truy vấn
     * @param {string} propValue Giá trị của property
     * @returns Một bản ghi lấy được có propertyName và propValue truyền vào
     * NVTOAN 09/07/2021
     */
    getEmployeeByProperty(propName, propValue) {
        let queryString = `${this.controller}/Property?propName=${propName}&propValue=${propValue}`;

        return BaseAPIConfig.get(`${queryString}`);
    }

    /**
     * Hàm export dữ liệu
     * @param {string} filterValue Giá trị filter
     * NVTOAN 12/07/2021
     */
    exportData(filterValue) {
        let queryString = `${this.controller}/Export${filterValue ? ('?&filterValue=' + filterValue) : ''}`;

        return BaseAPIConfig.get(`${queryString}`, { responseType: 'blob' });
    }
    
    /**
     * Hàm lấy danh sách nhân viên ngoại trừ 1 số nhân viên
     * @param {array} listId Danh sách Id nhân viên không lấy
     * @returns Danh sách nhân viên
     * NVTOAN 19/08/2021
     */
    getEmployeesExceptSome(listId, pageSize, pageNum, filterValue) {
        let queryString = `${this.controller}/Filter/Except?pageSize=${pageSize}&pageNum=${pageNum}${filterValue ? ('&filterValue=' + filterValue) : ''}`;

        return BaseAPIConfig.post(`${queryString}`, listId);
    }
}

export default new EmployeesAPI();