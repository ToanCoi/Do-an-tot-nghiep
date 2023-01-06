import BusinessAPI from "../BusinessAPI";

class TableAPI extends BusinessAPI {
    constructor() {
        super();

        this.controller = "api/v1/Tables";
    }

}

export default new TableAPI();