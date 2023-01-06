import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import router from "./router";
import { store } from './store/index'
import 'devextreme/dist/css/dx.light.css';
import FieldInputLabel from './components/FieldInputLabel.vue'
import ComboBox from './components/Combobox.vue'
import Dropdown from './components/Dropdown.vue'

import { pluginInstall } from "./js/common/plugin";

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

/* import specific icons */
import { faUser, faLock, faChevronRight } from '@fortawesome/free-solid-svg-icons'

/* add icons to the library */
library.add(faUser, faLock, faChevronRight)

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(store);

//Component
app.component('FieldInputLabel', FieldInputLabel);
app.component('ComboBox', ComboBox);
app.component('Dropdown', Dropdown);
app.component('font-awesome-icon', FontAwesomeIcon)

app.mount("#app");
