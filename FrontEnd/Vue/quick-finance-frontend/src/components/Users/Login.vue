  <script lang="ts">
  import apiClient from "@/api/services/apiClient";
  import { defineComponent, ref } from "vue";
  import { useToast } from "vue-toastification";
  import { useAuthStore } from "@/stores/auth";
  import { AxiosError } from "axios";
  import { useRoute, useRouter } from "vue-router";

  export default defineComponent({
    name: "Login",
    setup() {
      const username = ref("");
      const password = ref("");
      const toast = useToast();
      const authStore = useAuthStore();
      const passwordVisible = ref(false);
      const router = useRouter()

      const handleSubmit = async () => {
        try {
          const response = await apiClient.post("/auth/login", {
            username: username.value,
            password: password.value,
          });

          // Store token and user data in auth store
          authStore.login(response.data.token, {
            id: response.data.userId,
            username: response.data.userName,
            fullName: response.data.fullName,
            roles: response.data.roles?.$values || [],
          });

          const userName = authStore.user?.username
          const fullName = authStore.user?.fullName

          toast.success(`Login successful! Welcome back ${fullName ? fullName : userName}`);

          // Redirect to a protected page
          // router.push({ name: 'Dashboard' })
          // router.push({ path: "/dashboard" });
          window.location.href = "/dashboard"; // Change `/dashboard` to your protected route
        } catch (error) {
          const err = error as AxiosError;
          console.error("Login Failed:", err.response?.data || err.message);
          toast.error("Failed to login. Please check your credentials.");
        }
      };

      const togglePasswordVisibility = () => {
        passwordVisible.value = !passwordVisible.value;
        const passwordInput = document.getElementById(
          "floatingPassword"
        ) as HTMLInputElement;
        if (passwordInput) {
          passwordInput.type = passwordVisible.value ? "text" : "password";
        }
      };

      return {
        username,
        password,
        handleSubmit,
        togglePasswordVisibility,
        passwordVisible
      };
    },
  });
</script>

<style>
/* Centering Container */
.center-container {
  display: flex;
  justify-content: center;
  align-items: center;
  /* min-height: 100vh; */
  /* Full viewport height */
  /* background-color: #f8f9fa;  */
  /* Optional background color */
  padding: 20px;
  /* Padding for smaller viewports */
  box-sizing: border-box;
}

/* Form Styles */
.form-container {
  width: 350px;
  height: auto;
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  border-radius: 10px;
  box-sizing: border-box;
  padding: 20px 30px;
  /* background-color: #ffffff;  */
  /* White card background */
}

.title {
  text-align: center;
  font-family: "Lucida Sans", "Lucida Sans Regular", "Lucida Grande",
    "Lucida Sans Unicode", Geneva, Verdana, sans-serif;
  margin: 10px 0 30px 0;
  font-size: 28px;
  font-weight: 800;
}

.form {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 18px;
  margin-bottom: 15px;
}

.input {
  border-radius: 20px;
  border: 1px solid #c0c0c0;
  outline: 0 !important;
  box-sizing: border-box;
  padding: 12px 15px;
}

.form-btn {
  padding: 10px 15px;
  font-family: "Lucida Sans", "Lucida Sans Regular", "Lucida Grande",
    "Lucida Sans Unicode", Geneva, Verdana, sans-serif;
  border-radius: 20px;
  border: 0 !important;
  outline: 0 !important;
  background: teal;
  color: white;
  cursor: pointer;
  box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;
}

.form-btn:active {
  box-shadow: none;
}

.buttons-container {
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  margin-top: 20px;
  gap: 15px;
}

.sign-up-label {
  text-align: center;
  margin-top: 10px;
}
</style>


<template>
  <div class="center-container">
    <div class="form-container">
      <p class="title">Welcome back</p>
      <hr />
      <form class="form" @submit.prevent="handleSubmit">
        <div>
          <label class="form-label">Username</label>
          <div class="form-floating">
            <input type="text" class="form-control" id="floatingInput" v-model="username" placeholder="Username" />
            <label for="floatingInput">Enter your username</label>
          </div>
        </div>

        <div class="mb-3">
          <label class="form-label">Password</label>
          <div class="input-group">
            <div class="form-floating flex-grow-1">
              <input type="password" class="form-control" id="floatingPassword" v-model="password"
                placeholder="Password" autocomplete="off" />
              <label for="floatingPassword">Enter your password</label>
            </div>
            <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
              <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
            </button>
          </div>
        </div>
        <button class="btn btn-lg btn-primary">Log in</button>
      </form>
      <p class="sign-up-label">Don't have an account?</p>
      <div class="d-grid gap-2">
        <a class="btn btn-lg bg-secondary" href="/register">Register</a>
      </div>
    </div>
  </div>
</template>
