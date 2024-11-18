<script lang="ts">
import { defineComponent, ref, watch } from "vue";
import apiClient from "@/api/services/apiClient";
import { useToast } from "vue-toastification";
import { useRouter } from "vue-router";
import { saveSettings, Settings } from "@/api/services/generalService";

export default defineComponent({
  name: "Register",
  setup() {
    const username = ref("");
    const email = ref("");
    const password = ref("");
    const confirmPassword = ref("");
    const language = ref("ENG"); // Default language
    const currency = ref<keyof typeof currencyMap>("USD-DOLLAR"); // Strongly typed
    const currencySymbol = ref("$"); // Default currency symbol
    const passwordVisible = ref(false);
    const toast = useToast();
    const router = useRouter();

    // Map of currency to symbol
    const currencyMap: Record<string, string> = {
      "USD-DOLLAR": "$",
      EURO: "€",
      "HND-LEMPIRA": "L.",
    };

    // Update symbol automatically when currency changes
    watch(currency, (newValue) => {
      currencySymbol.value = currencyMap[newValue] || ""; // Safe access with TypeScript.
    });

    const handleSubmit = async () => {
      if (password.value !== confirmPassword.value) {
        toast.error("Passwords do not match.");
        return;
      }

      try {
        const response = await apiClient.post("/auth/register", {
          username: username.value,
          email: email.value,
          password: password.value,
          confirmPassword: password.value,
        });

        const userId = response.data.userId;

        const newSettings: Settings = {
          userId: userId,
          settingsName: "Cultural information",
          jsonValue: JSON.stringify({
            Language: language.value,
            Currency: currency.value,
            CurrencySymbol: currencySymbol.value,
          }),
        };

        await saveSettings(newSettings)
          .then((result) => {
            console.log("Settings saved successfully, response:", result);
          })
          .catch((error) => {
            console.error("Failed to save settings:", error);
          });

        toast.success("Registration successful! You can now log in.");
        router.push({ name: "Login" });
      } catch (error) {
        console.error("Registration failed:", error);
        toast.error("Registration failed. Please try again.");
      }
    };

    const togglePasswordVisibility = () => {
      passwordVisible.value = !passwordVisible.value;
    };

    return {
      username,
      email,
      password,
      confirmPassword,
      language,
      currency,
      currencySymbol,
      passwordVisible,
      togglePasswordVisibility,
      handleSubmit,
    };
  },
});
</script>


<template>
  <div class="center-container">
    <div class="form-container">
      <p class="title">Create an Account</p>
      <hr />
      <ul class="nav nav-tabs">
        <li class="nav-item">
          <a class="nav-link active" data-bs-toggle="tab" href="#user-info-tab">User Info</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" data-bs-toggle="tab" href="#settings-tab">Settings</a>
        </li>
      </ul>

      <form class="form tab-content" @submit.prevent="handleSubmit">
        <!-- User Info Tab -->
        <div class="tab-pane fade show active" id="user-info-tab">
          <div>
            <label class="form-label">Username</label>
            <div class="form-floating">
              <input type="text" class="form-control" v-model="username" placeholder="Username" />
              <label>Enter your username</label>
            </div>
          </div>

          <div>
            <label class="form-label">Email</label>
            <div class="form-floating">
              <input type="email" class="form-control" v-model="email" placeholder="Email" />
              <label>Enter your email</label>
            </div>
          </div>

          <div>
            <label class="form-label">Password</label>
            <div class="input-group">
              <div class="form-floating flex-grow-1">
                <input :type="passwordVisible ? 'text' : 'password'" class="form-control" v-model="password"
                  placeholder="Password" />
                <label>Enter your password</label>
              </div>
              <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
              </button>
            </div>
          </div>

          <div>
            <label class="form-label">Confirm Password</label>
            <div class="input-group">
              <div class="form-floating flex-grow-1">
                <input :type="passwordVisible ? 'text' : 'password'" class="form-control" v-model="confirmPassword"
                  placeholder="Confirm Password" />
                <label>Confirm your password</label>
              </div>
              <button type="button" class="btn btn-outline-secondary" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="['faR', passwordVisible ? 'eye-slash' : 'eye']" />
              </button>
            </div>
          </div>
        </div>

        <!-- Settings Tab -->
        <div class="tab-pane fade" id="settings-tab">
          <div class="row">
            <label class="form-label">Default Language</label>
            <select class="form-select" v-model="language">
              <option value="ENG">English</option>
              <option value="SPA">Español</option>
            </select>
          </div>

          <div class="row">
            <label class="form-label">Default Currency</label>
            <select class="form-select" v-model="currency">
              <option value="USD-DOLLAR">USD - Dollar ($)</option>
              <option value="EURO">EU - Euro (€)</option>
              <option value="HND-LEMPIRA">HND - Lempira (L.)</option>
            </select>
          </div>
        </div>

        <button class="btn btn-lg btn-primary mt-3">Register</button>
      </form>

      <p class="sign-up-label">
        Already have an account? <router-link to="/login">Log in</router-link>
      </p>
    </div>
  </div>
</template>
