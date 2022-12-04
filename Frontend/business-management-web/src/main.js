import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import router from "./router";
import { store } from './store/index'
import 'devextreme/dist/css/dx.light.css';
import FieldInputLabel from './components/FieldInputLabel.vue'
import ComboBox from './components/Combobox.vue'
import Dropdown from './components/Dropdown.vue'

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

//Component
app.component('FieldInputLabel', FieldInputLabel);
app.component('ComboBox', ComboBox);
app.component('Dropdown', Dropdown);

app.mount("#app");
