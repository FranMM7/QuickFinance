import { createApp } from 'vue';
import App from './App.vue';
import store from './store'; // Import the store
import 'bootstrap/dist/css/bootstrap.min.css';

createApp(App)
    .use(store) // Use the store
    .mount('#app');
