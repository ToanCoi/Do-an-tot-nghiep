import BusinessAPI from "../BusinessAPI";

class DishAPI extends BusinessAPI {
    constructor() {
        super();

        this.controller = "api/v1/Dish";
    }

}

export default new DishAPI();