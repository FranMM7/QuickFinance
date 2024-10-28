// Importing required dependencies from Vue and other libraries.
import { createApp } from 'vue'; // Function to create and configure the Vue application instance.
import App from './App.vue'; // The root component of the app, serves as the main entry point.
import { createPinia } from 'pinia'; // Pinia for state management, a modern alternative to Vuex.
import store from './stores'; // Imports the Vuex store (to be removed as you're moving to Pinia).
import Toast from 'vue-toastification'; // Library for displaying notifications and messages.
import 'vue-toastification/dist/index.css'; // CSS styles for Toast notifications.

// Import Bootstrap's JavaScript bundle for interactive UI components like modals, dropdowns, etc.
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

// Commented out the import for default Bootstrap CSS.
// import 'bootstrap/dist/css/bootstrap.min.css'; 

// Import Bootswatch theme. (This should be replaced dynamically based on user selection)
// import 'bootswatch/dist/Cyborg/bootstrap.min.css'; // Cyborg Bootswatch theme for customizing Bootstrap styles.

// Import router to manage the app's routes and navigation between views.
import router from './router'; // Vue Router instance for handling routing in the app.

// Import and configure FontAwesome for using icons in the app.
import { library } from '@fortawesome/fontawesome-svg-core'; // Function to create a library of icons for use in the app.
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'; // Enables the use of FontAwesome icons in Vue components.
import { fas } from '@fortawesome/free-solid-svg-icons'; // Imports the solid style icon pack from FontAwesome, includes frequently used icons.
import { useThemeStore } from './stores/themesStore'; // Importing the theme store for managing user-selected themes.

library.add(fas); // Add the imported solid icons to the FontAwesome library for global availability.

// Create the Vue application instance.
const app = createApp(App);

// Register a global helper function to format date strings across all components.
app.config.globalProperties.formatDate = function (dateString: string) {
  const date = new Date(dateString); // Creates a new Date object from the passed string.
  return date.toLocaleDateString(); // Returns the formatted date in the 'MM/DD/YYYY' format.
};

// Register FontAwesomeIcon as a globally available component.
app.component('font-awesome-icon', FontAwesomeIcon);

// Create a Pinia instance and configure the app to use it, enabling state management.
const pinia = createPinia(); // Create a Pinia instance for state management.
app.use(router); // Enable Vue Router for navigation.
app.use(pinia); // Enable Pinia for managing shared state.
app.use(store); // Enable the Vuex store (to be removed as you switch to Pinia).
app.use(Toast); // Enable Toast for displaying notifications and messages.

// Create a link element to load the theme CSS dynamically.
const link = document.createElement('link');
link.rel = 'stylesheet';
link.id = 'theme-link';
document.head.appendChild(link); // Append the link element to the document head.

// Load the stored theme on initial load.
const themeStore = useThemeStore(pinia); // Initialize the theme store with the Pinia instance.
themeStore.loadTheme(themeStore.currentTheme); // Load the current theme from the store.

// Finally, mount the Vue application to the DOM element with the ID 'app'.
app.mount('#app'); // Renders the entire application and starts the Vue instance.
