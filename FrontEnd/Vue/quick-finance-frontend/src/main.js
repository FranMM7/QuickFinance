import { createApp } from 'vue';
import App from './App.vue';
import store from './store'; // Vuex store for state management
import router from './router'; // Import router

// Import Bootswatch theme (slate)
import 'bootswatch/dist/slate/bootstrap.min.css';

// Note: If you want to use the default Bootstrap instead of a Bootswatch theme, 
// uncomment the following line and comment out the Bootswatch import above.
// import 'bootstrap/dist/css/bootstrap.min.css';

// Import Bootstrap's JavaScript (required for modals, tooltips, etc.)
// This includes Bootstrap's JS components and Popper.js (for tooltips, dropdowns)
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

// Create and mount the Vue app instance
createApp(App)
    .use(store) // Register the Vuex store to manage app state
    .use(router) // Use the router
    .mount('#app'); // Mount the app to the element with ID 'app'
