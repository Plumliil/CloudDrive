import { createApp } from 'vue'
import Idux from "./idux";
import App from './App.vue'
import './tailwind.css'
import router from './router'

createApp(App).use(Idux).use(router).mount('#app')
