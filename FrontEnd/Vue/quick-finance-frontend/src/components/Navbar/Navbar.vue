<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useThemeStore } from '@/stores/themesStore';
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import apiClient, { changePassword, userInfo } from '@/api/services/apiClient';
import { AxiosError } from 'axios';

// Stores and Router
const themeStore = useThemeStore();
const authStore = useAuthStore();
const router = useRouter();
const toast = useToast();

// Refs
const currentTheme = ref<string>('default');
const displayUserInfo = ref('');
const adminAccess = ref<boolean>(false);

const modalPsw = ref<boolean>(false);
const currentPassword = ref('');
const newPassword = ref('');
const confirmPassword = ref('');
const passwordVisible = ref<boolean>(false);
const passwordInputType = ref<'password' | 'text'>('password');
const formControlClass = ref('form-control');
const feedbackMessage = ref('');
const feedbackClass = ref('');


// Theme handling
const loadTheme = () => {
  const savedTheme = localStorage.getItem('theme') || 'default';
  setTheme(savedTheme);
};

const setTheme = (theme: string) => {
  currentTheme.value = theme;
  localStorage.setItem('theme', theme);

  const link = document.getElementById('theme-link') as HTMLLinkElement | null;
  if (link) {
    link.href = `https://bootswatch.com/${theme}/bootstrap.min.css`;
  } else {
    console.error('Theme link element not found!');
  }

  themeStore.setTheme(theme);
};

const changeTheme = (theme: string) => setTheme(theme);

// Password modal handling
const openModalRstPsw = () => (modalPsw.value = true);
const closeModalRstPsw = () => {
  modalPsw.value = false;
  currentPassword.value = '';
  newPassword.value = '';
  confirmPassword.value = '';
  resetValidation();
};

const togglePasswordVisibility = () => {
  passwordVisible.value = !passwordVisible.value;
  passwordInputType.value = passwordVisible.value ? 'text' : 'password';
};

const validatePasswords = () => {
  if (newPassword.value && confirmPassword.value) {
    if (newPassword.value === confirmPassword.value) {
      formControlClass.value = 'form-control is-valid';
      feedbackMessage.value = 'Password Matched!';
      feedbackClass.value = 'valid-feedback';
    } else {
      formControlClass.value = 'form-control is-invalid';
      feedbackMessage.value = 'Password Unmatched!';
      feedbackClass.value = 'invalid-feedback';
    }
  } else {
    resetValidation();
  }
};

const resetValidation = () => {
  formControlClass.value = 'form-control';
  feedbackMessage.value = '';
  feedbackClass.value = '';
};

const resetPassword = async () => {
  // Validate all fields
  if (!currentPassword.value || !newPassword.value || !confirmPassword.value) {
    toast.error('All fields are required!');
    return;
  }

  // Validate password strength
  const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
  if (!passwordRegex.test(newPassword.value)) {
    toast.error(
      'Password must be at least 8 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.'
    );
    return;
  }

  // Ensure passwords match
  if (newPassword.value !== confirmPassword.value) {
    toast.error("Passwords don't match!");
    return;
  }

  try {
    // Call the `changePassword` function
    const responseStatus = await changePassword(currentPassword.value, newPassword.value);

    if (responseStatus === 200) {
      toast.success('Password has been changed!');
      closeModalRstPsw(); // Close modal on success
      handleLogout(); // Log the user out
    } else {
      toast.error('Unexpected response from the server.');
    }
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : 'An unknown error occurred';
    toast.error(errorMessage);
    console.error('Password reset error:', errorMessage);
  }
};

const getUserInfo = async () => {
  const response = userInfo()
  console.log('getUserInfo res: ', response)
}

// Authentication
const handleLogout = () => {
  authStore.logout();
  displayUserInfo.value = '';
  router.push({ name: 'Login' });
};

// Lifecycle
onMounted(() => {
  const userName = authStore.user?.username;
  const fullName = authStore.user?.fullName;

  adminAccess.value = authStore.user?.roles?.includes('Admin') || false;
  displayUserInfo.value = fullName || userName || '';
  loadTheme();
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
              <router-link to="/Categories" class="nav-link">
                <font-awesome-icon :icon="['fas', 'list-ul']" />
                Categories</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/budgets" class="nav-link">
                <font-awesome-icon :icon="['fas', 'wallet']" />
                Budgets</router-link>
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
            <!-- --register new admin users  -->
            <li class="nav-item" v-if="adminAccess">
              <a href="/registerAdmin" class="nav-link">Users</a>
            </li>
          </template>
        </ul>

        <ul class="navbar-nav d-flex">
          <!-- Theme Selection Dropdown -->
          <li class="nav-item dropdown ">
            <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-haspopup="true"
              aria-expanded="false">
              <font-awesome-icon :icon="['fas', 'palette']" />
              Themes
            </a>
            <div class="dropdown-menu">
              <a class="dropdown-item" href="#" @click="changeTheme('default')">Bootstrap Default</a>
              <a class="dropdown-item" href="#" @click="changeTheme('cyborg')">Cyborg</a>
              <a class="dropdown-item" href="#" @click="changeTheme('cerulean')">Cerulean</a>
              <!-- <a class="dropdown-item" href="#" @click="changeTheme('cosmo')">Cosmo</a> -->
              <a class="dropdown-item" href="#" @click="changeTheme('darkly')">Darkly</a>
              <a class="dropdown-item" href="#" @click="changeTheme('flatly')">Flatly</a>
              <a class="dropdown-item" href="#" @click="changeTheme('lux')">Lux</a>
              <a class="dropdown-item" href="#" @click="changeTheme('pulse')">Pulse</a>
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
              Welcome {{ displayUserInfo || 'User' }}
            </a>
            <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">

              <li class="nav-item">
                <router-link to="/profile" class="dropdown-item">
                  <font-awesome-icon :icon="['fas', 'user']" /> Profile
                </router-link>
              </li>

              <li class="nav-item">
                <router-link to="/Settings" class="dropdown-item">
                  <font-awesome-icon :icon="['fas', 'gear']" />
                  Settings</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/Dashboard" class="dropdown-item">
                  <font-awesome-icon :icon="['fas', 'gauge']" />
                  Dashboard</router-link>
              </li>

              <li>
                <hr>
              </li>

              <li class="nav-item">
                <a href="#" class="dropdown-item" @click="openModalRstPsw()">
                  <font-awesome-icon :icon="['fas', 'key']" />
                  Change Password
                </a>
              </li>

              <li class="nav-item">
                <a href="#" class="dropdown-item" @click="handleLogout">
                  <font-awesome-icon :icon="['fas', 'sign-out-alt']" /> Logout
                </a>
              </li>
            </ul>

          </li>
        </ul>

      </div>
    </div>
  </nav>

  <!-- modal //reset password-->
  <div v-if="modalPsw" class="modal show" tabindex="-1" style="display: block;" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title text-center">RESET PASSWORD</h5>
          <button type="button" class="btn-close" @click="closeModalRstPsw"></button>
        </div>

        <div class="modal-body">

          <div class="mb-3">
            <label class="form-label">Current Password</label>
            <div class="input-group">
              <input :type="passwordInputType" class="form-control" v-model="currentPassword" id="floatingPassword" />
              <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
              </button>
            </div>
          </div>

          <div class="mb-3">
            <label class="form-label">New Password</label>
            <div class="input-group">
              <input :type="passwordInputType" :class="formControlClass" v-model="newPassword"
                @input="validatePasswords" id="floatingPassword" />
              <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
              </button>
              <div :class="feedbackClass">{{ feedbackMessage }}</div>
            </div>
          </div>

          <div class="mb-3">
            <label class="form-label">Confirm Password</label>
            <div class="input-group">
              <input :type="passwordInputType" :class="formControlClass" v-model="confirmPassword"
                @input="validatePasswords" id="floatingPassword" />
              <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
              </button>
            </div>
          </div>

          <button class="btn btn-primary" @click="resetPassword">Change Password</button>
        </div>
      </div>
    </div>
    <!-- end modal -->

  </div>

</template>
