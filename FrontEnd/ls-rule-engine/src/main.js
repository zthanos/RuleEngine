import { createApp } from 'vue'
import './assets/style.css'
import App from './App.vue'
import 'flowbite';
import router from './router';


createApp(App).use(router).mount('#app')
