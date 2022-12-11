import BaseAPI from "../../base/BaseAPI";

class TableAPI extends BaseAPI {
    constructor() {
        super();

        this.controller = "api/v1/Tables";
    }

}

export default new TableAPI();