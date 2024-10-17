// Importing required dependencies from Vue and other libraries.
import { createApp } from 'vue'; // Imports the createApp function from Vue, which is used to create and configure the Vue application instance.
import App from './App.vue'; // Imports the root component of the app (App.vue), which serves as the main entry point of the Vue application.
import { createPinia } from 'pinia'; // Imports the createPinia function from Pinia for state management, allowing for a modular approach to managing global state.
import store from './stores'; // (Will be removed) Imports the Vuex store for state management (currently not needed as you're moving to Pinia).
import Toast from 'vue-toastification';
import 'vue-toastification/dist/index.css';

// Import Bootstrap and Bootswatch for UI styling and interactive components.
import 'bootstrap/dist/js/bootstrap.bundle.min.js'; // Imports Bootstrap's JavaScript bundle, which includes interactive components like dropdowns, modals, etc.
import 'bootswatch/dist/cyborg/bootstrap.min.css'; // Imports the Bootswatch Cyborg theme for customizing Bootstrap's default styling.

// Import router to manage the app's routes (navigation between views).
import router from './router'; // Imports the Vue Router instance, which controls navigation and URL management in your app.

// Import and configure FontAwesome for using icons in the app.
import { library } from '@fortawesome/fontawesome-svg-core'; // Imports the library function from FontAwesome, which allows you to create a library of icons that can be used in the app.
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'; // Imports the FontAwesomeIcon component, which enables the use of FontAwesome icons in Vue components.
import { fas } from '@fortawesome/free-solid-svg-icons'; // Imports the "solid" style icon pack from FontAwesome, including various frequently used icons (like home, user, etc.).

// Add the imported solid icons to the FontAwesome library, making them available globally within the app.
library.add(fas); 

// Create the Vue application instance.
const app = createApp(App); // The app instance is created using the root App component, which is the starting point of the application.

// Register a global helper function to format date strings across all components.
app.config.globalProperties.formatDate = function (dateString: string) {
    const date = new Date(dateString); // Creates a new Date object from the passed string (dateString).
    return date.toLocaleDateString(); // Returns the formatted date in the 'MM/DD/YYYY' format.
};

// Register FontAwesomeIcon as a globally available component so you can use <font-awesome-icon> in your templates.
app.component('font-awesome-icon', FontAwesomeIcon);

// Configure the app to use the router for handling page navigation and Pinia for managing application state.
app.use(router); // Tells the app to use Vue Router, enabling the app to navigate between different pages or views.
app.use(createPinia()); // Tells the app to use Pinia for state management, replacing Vuex for managing shared state between components.
app.use(store); // (To be removed) Tells the app to use the Vuex store (this will be removed as we switch to Pinia).
app.use(Toast); //To display message and notifiacions

// Finally, mount the Vue application to the DOM element with the ID 'app'.
// This renders the entire application and starts the Vue instance, making it visible on the webpage.
app.mount('#app');
