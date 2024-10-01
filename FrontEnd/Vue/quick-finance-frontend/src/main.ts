import { createApp } from 'vue'; // Import the createApp function from Vue to create a new Vue application instance.
import App from './App.vue'; // Import the root App component which serves as the entry point of the application.\
import { createPinia } from 'pinia'; // Import createPinia function from the Pinia library, which is used for state management in Vue applications.

//For appareance and customizations. 
import 'bootstrap/dist/js/bootstrap.bundle.min.js'; // Import Bootstrap's JavaScript bundle for functionalities like modals, dropdowns, and other interactive components.
import 'bootswatch/dist/cyborg/bootstrap.min.css'; // Import Bootswatch's Cyborg theme for styling. Bootswatch provides a set of Bootstrap themes for easy customization.
import router from './router'; // Import the router instance from the router module, which manages navigation between different views in the application.

// Customize FontAwesome libraries for icons
import { library } from '@fortawesome/fontawesome-svg-core'; // Import the library function from FontAwesome to create a library of icons.
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'; // Import the FontAwesomeIcon component to use FontAwesome icons in Vue components.
import { fas } from '@fortawesome/free-solid-svg-icons'; // Import solid icons from FontAwesome. You can import other styles (like brands or regular) as needed.

// Add solid icons to the library, making them available throughout the application.
library.add(fas); 

// Create a new Vue application instance using the root App component.
const app = createApp(App);

// Register the formatDate function as a global property to be accessible in all components.
app.config.globalProperties.formatDate = function (dateString: string) {
    const date = new Date(dateString); // Create a new Date object from the input string.
    return date.toLocaleDateString(); // Returns the date formatted as 'MM/DD/YYYY'.
};

// Register the FontAwesomeIcon component globally, allowing its use throughout the project.
app.component('font-awesome-icon', FontAwesomeIcon);

// Use the router and Pinia state management in the application.
app.use(router); // Register the router with the Vue application to enable navigation between views.
app.use(createPinia()); // Register Pinia for state management, allowing components to share and manage state.


// Mount the Vue application to the DOM element with the id 'app'.
// This renders the application and starts the Vue instance.
app.mount('#app'); 
