import AuthAPI from "../AuthAPI"
import axios from "axios";

class AccountAPI extends AuthAPI {
    constructor() {
        super();

        this.controller = "api/v1/Accounts";
    }

    login(username, password) {
        this.getBaseURL();
        return axios.post(`${this.baseURL}/login?username=${username}&password=${password}`);
    }

}

export default new AccountAPI();