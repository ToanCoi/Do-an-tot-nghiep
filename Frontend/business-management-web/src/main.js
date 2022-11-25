import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import router from "./router";
import { store } from './store/index'
import 'devextreme/dist/css/dx.light.css';

import Resource from './js/common/Resource'
import Enumeration from './js/common/Enumeration'
import CommonFn from './js/common/CommonFn'

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(store);
app.use(Resource);
app.use(Enumeration)
app.use(CommonFn);

app.mount("#app");
