<script lang="ts">
import { defineComponent, ref } from "vue";
import apiClient from "@/api/services/apiClient"; // Assuming you have an API client set up
import { useToast } from "vue-toastification";

export default defineComponent({
  name: "Register",
  setup() {
    const username = ref("");
    const email = ref("");
    const password = ref("");
    const confirmPassword = ref("");
    const toast = useToast();
    const passwordVisible = ref(false);

    const handleSubmit = async () => {
      // Check if passwords match
      if (password.value !== confirmPassword.value) {
        toast.error("Passwords do not match.");
        return;
      }

      try {
        // Call API to register user
        const response = await apiClient.post("/auth/register", {
          username: username.value,
          email: email.value,
          password: password.value,
          confirmPassword:confirmPassword.value
        });

        toast.success("Registration successful! You can now log in.");
        // Redirect to login page after successful registration
        window.location.href = "/login";
      } catch (error) {
        console.error("Registration failed:", error);
        toast.error("Registration failed. Please try again.");
      }
    };

    // Toggle password visibility
    const togglePasswordVisibility = () => {
      passwordVisible.value = !passwordVisible.value;
    };

    return {
      username,
      email,
      password,
      confirmPassword,
      handleSubmit,
      passwordVisible,
      togglePasswordVisibility
    };
  },
});
</script>


<template>
  <div class="center-container">
    <div class="form-container">
      <p class="title">Create an Account</p>
      <hr />
      <form class="form" @submit.prevent="handleSubmit">
        <!-- Username Field -->
        <div>
          <label class="form-label">Username</label>
          <div class="form-floating">
            <input type="text" class="form-control" id="floatingUsername" v-model="username" placeholder="Username" />
            <label for="floatingUsername">Enter your username</label>
          </div>
        </div>

        <!-- Email Field -->
        <div>
          <label class="form-label">Email</label>
          <div class="form-floating">
            <input type="email" class="form-control" id="floatingEmail" v-model="email" placeholder="Email" />
            <label for="floatingEmail">Enter your email</label>
          </div>
        </div>

        <!-- Password Field -->
        <div>
          <label class="form-label">Password</label>
          <div class="form-floating">
            <div class="input-group">
              <div class="form-floating flex-grow-1">
                <input :type="passwordVisible ? 'text' : 'password'" class="form-control" id="floatingPassword"
                  v-model="password" placeholder="Password" autocomplete="off" />
                <label for="floatingPassword">Enter your password</label>
              </div>
              <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
              </button>
            </div>
          </div>
        </div>

        <!-- Confirm Password Field -->
        <div>
          <label class="form-label">Confirm Password</label>
          <div class="form-floating">
            <div class="input-group">
              <div class="form-floating flex-grow-1">
                <input :type="passwordVisible ? 'text' : 'password'" class="form-control" id="floatingConfirmPassword"
                  v-model="confirmPassword" placeholder="Confirm Password" autocomplete="off" />
                <label for="floatingConfirmPassword">Confirm your password</label>
              </div>
              <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
              </button>
            </div>
          </div>
        </div>

        <button class="btn btn-lg btn-primary">Register</button>
      </form>

      <p class="sign-up-label">
        Already have an account?
        <router-link to="/login">Log in</router-link>
      </p>
    </div>
  </div>
</template>
