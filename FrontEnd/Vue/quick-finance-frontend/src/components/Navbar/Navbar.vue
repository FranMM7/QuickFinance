<script lang="ts">
import { ref, onMounted, defineComponent } from 'vue'; // Importing required Vue functions
import { useThemeStore } from '@/stores/themesStore'; // Importing theme store
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: "Navbar",
  setup() {
    const themeStore = useThemeStore();
    const currentTheme = ref<string>('default'); // Declare currentTheme with default value
    const authStore = useAuthStore();
    const username = ref('');
    const router = useRouter()

    const handleLogout = () => {
      authStore.logout();
      username.value = ''
      router.push({ name: 'Login' })
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
      username.value = useAuthStore().user?.username || ''
      loadTheme(); // Call loadTheme to apply the saved theme
    });

    return {
      changeTheme,
      handleLogout,
      authStore,
      username
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
          </template>
        </ul>

        <ul class="navbar-nav d-flex">
          <!-- Theme Selection Dropdown -->
          <li class="nav-item dropdown ">
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

          <!-- Login link if user is not authenticated -->
          <li v-if="!authStore.isAuthenticated" class="nav-item d-flex">
            <router-link to="/login" class="nav-link">Login</router-link>
          </li>

          <!-- Welcome user dropdown if user is authenticated -->
          <li v-if="authStore.isAuthenticated" class="nav-item dropdown topnav-right">
            <a href="#" class="nav-link dropdown-toggle" id="userDropdown" role="button" data-bs-toggle="dropdown"
              aria-expanded="false">
              <font-awesome-icon :icon="['fas', 'user']" />
              Welcome {{ authStore.user?.username || 'User' }}
            </a>
            <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
              <li class="nav-item">
                <router-link to="/profile" class="dropdown-item">
                  <font-awesome-icon :icon="['fas', 'user']" /> Profile
                </router-link>
              </li>
              <li class="nav-item">
                <a href="#" class="dropdown-item" @click="handleLogout">
                  <font-awesome-icon :icon="['fas', 'sign-out-alt']" /> Logout
                </a>
              </li>
              <li class="nav-item">
                <router-link to="/Settings" class="nav-link">
                  <font-awesome-icon :icon="['fas', 'gear']" />
                  Settings</router-link>
              </li>
              <li class="nav-item">
                <router-link to="/Dashboard" class="nav-link">
                  <font-awesome-icon :icon="['fas', 'gauge']" />
                  Dashboard</router-link>
              </li>
            </ul>
          </li>

        </ul>




      </div>
    </div>
  </nav>

</template>
