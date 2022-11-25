import BaseAPI from "../../base/BaseAPI";
import BaseAPIConfig from "../../base/BaseAPIConfig";

class TaxAPI extends BaseAPI {
    constructor() {
        super();

        this.controller = "api/v1/Tax";
    }

    /**
     * Hàm lấy mã kỳ thuế mới
     * @returns Mã kỳ thuế mới
     * NVTOAN 12/08/2021
     */
    getNewTaxCode() {
        return BaseAPIConfig.get(`${this.controller}/NewTaxCode`);
    }

    /**
     * Hàm lấy bản ghi theo property
     * @param {string} propName Tên property cần truy vấn
     * @param {string} propValue Giá trị của property
     * @returns Một bản ghi lấy được có propertyName và propValue truyền vào
     * NVTOAN 12/08/2021
     */
     getTaxByProperty(propName, propValue) {
        let queryString = `${this.controller}/Property?propName=${propName}&propValue=${propValue}`;

        return BaseAPIConfig.get(`${queryString}`);
    }

    /**
     * Hàm lấy thông tin thuế theo giá trị filter
     * @param {int} year Năm cần lấy dữ liệu
     * @param {int} pageSize Số bản ghi/trang
     * @param {int} pageNum Số trang
     * @param {string} filterValue Giá trị filter
     */
    filter(year, pageSize, pageNum, filterValue) {
        let queryString = `${this.controller}/Filter?year=${year}&pageSize=${pageSize}&pageNum=${pageNum}${filterValue ? ('&filterValue=' + filterValue) : ''}`;
        
        return BaseAPIConfig.get(`${queryString}`);
    }
}

export default new TaxAPI();