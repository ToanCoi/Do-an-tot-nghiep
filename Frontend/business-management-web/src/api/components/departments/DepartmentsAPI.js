import BaseAPI from "../../base/BaseAPI";

class DepartmentsAPI extends BaseAPI {
    constructor() {
        super();

        this.controller = "api/v1/Departments";
    }

}

export default new DepartmentsAPI();