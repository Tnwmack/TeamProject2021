import "./styles/main.scss"
import { createApp } from 'vue'
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
import App from './App.vue'
import router from './router'
import store from './store'


const toastOptions = {

};

createApp(App).use(store).use(router).use(Toast, toastOptions).mount('#app')
