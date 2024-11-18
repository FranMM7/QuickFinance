<script lang="ts">
import { ref, onMounted, defineComponent } from 'vue'; // Importing required Vue functions
import { useThemeStore } from '@/stores/themesStore'; // Importing theme store
import { useAuthStore } from '@/stores/auth';

export default defineComponent({
  name: "Navbar",
  setup() {
    const themeStore = useThemeStore();
    const currentTheme = ref<string>('default'); // Declare currentTheme with default value
    const authStore = useAuthStore();

    const handleLogout = () => {
      authStore.logout();
      window.location.href = "/login"; // Redirect to login page
    };

    // Load the theme when the component mounts
    const loadTheme = () => {
      const savedTheme = localStorage.getItem('theme') || 'default'; // Default theme if none saved
      setTheme(savedTheme);
    }

    // Set the theme and update the stylesheet link
    const setTheme = (theme: string) => {
      currentTheme.value = theme;
      localStorage.setItem('theme', theme); // Save to local storage

      // Get the link element and ensure it's of type HTMLLinkElement
      const link = document.getElementById('theme-link') as HTMLLinkElement | null;

      // Check if the link element is not null before accessing its properties
      if (link) {
        // Set the URL to the Bootswatch theme
        link.href = `https://bootswatch.com/${theme}/bootstrap.min.css`;
      } else {
        console.error("Theme link element not found!");
      }

      themeStore.setTheme(theme); // If you're using a store to manage themes
    }



    // Change theme when a dropdown item is clicked
    const changeTheme = (theme: string) => {
      setTheme(theme); // Call setTheme method
    }

    // Load the theme when the component is mounted
    onMounted(() => {
      loadTheme(); // Call loadTheme to apply the saved theme
    });

    return {
      changeTheme,
      handleLogout,
      authStore
    }
  }
});
</script>


<template>
  <nav class="navbar navbar-expand-lg bg-primary" data-bs-theme="dark" style="color:whitesmoke">
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/">Quick Finance App</router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarColor02"
        aria-controls="navbarColor02" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" id="navbarColor02">
        <ul class="navbar-nav me-auto">
          <li class="nav-item">
            <router-link to="/" class="nav-link" exact-active-class="active">
              <font-awesome-icon :icon="['fas', 'house']" />
              Home</router-link>
          </li>
          <template v-if="authStore.isAuthenticated">
            <li class="nav-item">
              <router-link to="/budgets" class="nav-link">
                <font-awesome-icon :icon="['fas', 'wallet']" />
                Budgets</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/Categories" class="nav-link">
                <font-awesome-icon :icon="['fas', 'list-ul']" />
                Categories</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/finance" class="nav-link">
                <font-awesome-icon :icon="['fas', 'coins']" />
                Finance Analysis</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/Shopping" class="nav-link">
                <font-awesome-icon :icon="['fas', 'cart-shopping']" />
                Shopping List</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/Settings" class="nav-link">
                <font-awesome-icon :icon="['fas', 'gear']" />
                Settings</router-link>
            </li>
          </template>

          <!-- Theme Selection Dropdown -->
          <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-haspopup="true"
              aria-expanded="false">
              <font-awesome-icon :icon="['fas', 'list']" />
              Themes
            </a>
            <div class="dropdown-menu">
              <a class="dropdown-item" href="#" @click="changeTheme('default')">Bootstrap Default</a>
              <a class="dropdown-item" href="#" @click="changeTheme('cyborg')">Cyborg</a>
              <a class="dropdown-item" href="#" @click="changeTheme('cerulean')">Cerulean</a>
              <!-- <a class="dropdown-item" href="#" @click="changeTheme('cosmo')">Cosmo</a> -->
              <a class="dropdown-item" href="#" @click="changeTheme('darkly')">Darkly</a>
              <a class="dropdown-item" href="#" @click="changeTheme('flatly')">Flatly</a>
              <!-- <a class="dropdown-item" href="#" @click="changeTheme('lux')">Lux</a> -->
              <!-- <a class="dropdown-item" href="#" @click="changeTheme('pulse')">Pulse</a> -->
              <a class="dropdown-item" href="#" @click="changeTheme('quartz')">Quartz</a>
              <a class="dropdown-item" href="#" @click="changeTheme('vapor')">Vapor</a>
              <a class="dropdown-item" href="#" @click="changeTheme('slate')">Slate</a>
              <a class="dropdown-item" href="#" @click="changeTheme('united')">United</a>
              <!-- Add other Bootswatch themes as needed -->
            </div>
          </li>

          <li v-if="!authStore.isAuthenticated" class="nav-item"><a href="/login" class="nav-link">Login</a></li>
          <li v-if="authStore.isAuthenticated" class="nav-item">
            <button @click="handleLogout" class="nav-link">Logout</button>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>
